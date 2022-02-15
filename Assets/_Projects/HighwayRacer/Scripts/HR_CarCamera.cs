using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.HighwayRacer {
  public class HR_CarCamera : MonoBehaviourBase {
    public CameraMode cameraMode;

    public enum CameraMode {
      Top,
      TPS,
      FPS
    }

    private GameObject _audioListener;

    private int _cameraSwitchCount;
    private RCC_HoodCamera _hoodCam;
    private RCC_WheelCamera _tpsCam;

    private float _targetFieldOfView = 50f;
    public float topFOV = 48f, tpsFOV = 55f, fpsFOV = 65f;

    // The target we are following
    public Transform playerCar;
    private Rigidbody _playerRigid;
    public bool gameover;

    private Camera _cam;
    private Vector3 _targetPosition = new Vector3(0, 0, 50);
    private Vector3 _pastFollowerPosition, _pastTargetPosition;

    // The distance in the x-z plane to the target
    public float distance = 8f;

    // the height we want the camera to be above the target
    public float height = 8.5f;

    private const float Rotation = 30f;
    private float _currentT, _oldT;

    private float _speed;
    private const float MaxShakeAmount = 0.00025f;

    public GameObject mirrors;

    protected override void Start() {
      // BASE: TryDestroy
      if (TryGetComponent(out AudioListener audioListener)) {
        Destroy(audioListener);
      }

      _cam = GetComponent<Camera>();
      _position = new Vector3(2f, 1f, 55f);
      _rotation = Quaternion.Euler(new Vector3(0f, -40f, 0f));

      _audioListener = new GameObject("Audio Listener");
      _audioListener.transform.SetParent(transform, false);
      _audioListener.AddComponent<AudioListener>();
    }

    private void OnEnable() {
      HR_PlayerHandler.OnPlayerSpawned += OnPlayerSpawned;
      HR_PlayerHandler.OnPlayerDied += OnPlayerCrashed;
    }

    private void OnDisable() {
      HR_PlayerHandler.OnPlayerSpawned -= OnPlayerSpawned;
      HR_PlayerHandler.OnPlayerDied -= OnPlayerCrashed;
    }

    private void OnPlayerSpawned(HR_PlayerHandler player) {
      playerCar = player.transform;
      _playerRigid = player.GetComponent<Rigidbody>();
      _hoodCam = player.GetComponentInChildren<RCC_HoodCamera>();
      _tpsCam = player.GetComponentInChildren<RCC_WheelCamera>();
      mirrors = GameObject.Find("Mirrors") ?? mirrors;
    }

    private void OnPlayerCrashed(HR_PlayerHandler player) => gameover = true;

    // CACHE
    protected override void LateUpdate() {
      if (!playerCar) {
        playerCar = FindObjectOfType<RCC_CarControllerV3>().transform;
        _playerRigid = playerCar.GetComponent<Rigidbody>();
        return;
      }

      _playerRigid = playerCar.GetComponent<Rigidbody>();
      _cam ??= GetComponent<Camera>();

      if (!playerCar || !_playerRigid || Time.timeSinceLevelLoad < 1.5f) {
        _position += Quaternion.identity * Vector3.forward * (Time.deltaTime * 3f);
      }
      else if (playerCar && _playerRigid) {
        //targetPosition = playerCar.position;
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _targetFieldOfView, Time.deltaTime * 3f);

        if (!gameover) {
          SetupCameraMode();
          if (HR_HighwayRacerProperties.Instance._shakeCamera)
            _targetPosition += (Random.insideUnitSphere * _speed * MaxShakeAmount);
        }
        else if (Time.timeScale >= 1) {
          _position = new Vector3(_position.x, _position.y, _position.z + Mathf.Clamp(_currentT, 0f, Mathf.Infinity));
        }

        cameraMode = _cameraSwitchCount switch {
          0 => CameraMode.Top,
          1 => CameraMode.TPS,
          2 => CameraMode.FPS,
          _ => cameraMode
        };
      }

      _audioListener.transform.position = new Vector3(playerCar.position.x, _position.y, _position.z);
      _pastFollowerPosition = _position;
      _pastTargetPosition = _targetPosition;
      _currentT = (_position.z - _oldT);
      _oldT = _position.z;
    }

    private void SetupCameraMode() {
      switch (cameraMode) {
        case CameraMode.Top:
          SetupTopMode();
          if (mirrors)
            mirrors.SetActive(false);
          break;
        case CameraMode.TPS:
          if (_tpsCam) {
            SetupTpsMode();
          }
          else {
            _cameraSwitchCount++;
            ChangeCamera();
          }

          break;
        case CameraMode.FPS:
          if (_hoodCam) {
            SetupFpsMode();
            if (mirrors)
              mirrors.SetActive(true);
          }
          else {
            _cameraSwitchCount++;
            ChangeCamera();
          }

          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public void ChangeCamera() {
      if (++_cameraSwitchCount >= 3)
        _cameraSwitchCount = 0;
    }

    protected override void Update() {
      if (Input.GetKeyDown(RCC_Settings.Instance.changeCameraKB))
        ChangeCamera();
    }

    protected override void FixedUpdate() {
      if (!_playerRigid) return;

      _speed = Mathf.Lerp(_speed, (_playerRigid.velocity.magnitude * 3.6f), Time.deltaTime * 1.5f);
    }

    private void SetupTopMode() {
      _rotation = Quaternion.Lerp(_rotation, Quaternion.Euler(Rotation, 0f, 0f), Time.deltaTime * 2f);
      _targetPosition = playerCar.position.WithX(0f);
      _targetPosition -= _rotation * Vector3.forward * distance;
      _targetPosition = new Vector3(_targetPosition.x, height, _targetPosition.z);

      if (Time.timeSinceLevelLoad < 3f)
        Transform.SmoothApproach(_pastFollowerPosition, _pastTargetPosition, _targetPosition,
          (_speed / 2f) * Mathf.Clamp(Time.timeSinceLevelLoad - 1.5f, 0f, 10f));
      else
        _position = _targetPosition;

      _targetFieldOfView = topFOV;
    }

    private void SetupTpsMode() {
      _rotation = Quaternion.Lerp(_rotation, Quaternion.Euler(Rotation / 3f, 0f, 0f), Time.deltaTime * 2f);
      _position = _targetPosition = _tpsCam.transform.position.WithX(playerCar.position.x);
      _targetFieldOfView = tpsFOV;
    }

    private void SetupFpsMode() {
      _rotation = Quaternion.Lerp(_rotation, Quaternion.identity, Time.deltaTime * 2f);

      if (HR_HighwayRacerProperties.Instance._tiltCamera)
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
          transform.InverseTransformDirection(_playerRigid.velocity).x / 2f,
          -transform.InverseTransformDirection(_playerRigid.velocity).x / 2f);

      _targetPosition = _hoodCam.transform.position;
      _position = _targetPosition;
      _targetFieldOfView = fpsFOV;
    }
  }
}