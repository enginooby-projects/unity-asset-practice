using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.HighwayRacer {
  public class HR_CarCamera : MonoBehaviour {
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
    public float topFOV = 48f;
    public float tpsFOV = 55f;
    public float fpsFOV = 65f;

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

    private void Start() {
      _cam = GetComponent<Camera>();

      transform.position = new Vector3(2f, 1f, 55f);
      transform.rotation = Quaternion.Euler(new Vector3(0f, -40f, 0f));

      if (GetComponent<AudioListener>())
        Destroy(GetComponent<AudioListener>());

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

      if (GameObject.Find("Mirrors"))
        mirrors = GameObject.Find("Mirrors").gameObject;
    }

    private void OnPlayerCrashed(HR_PlayerHandler player) => gameover = true;

    private void LateUpdate() {
      if (!playerCar) {
        playerCar = FindObjectOfType<RCC_CarControllerV3>().transform;
        _playerRigid = playerCar.GetComponent<Rigidbody>();
        return;
      }

      // if (_playerRigid != playerCar.GetComponent<Rigidbody>())
      _playerRigid = playerCar.GetComponent<Rigidbody>();

      _cam ??= GetComponent<Camera>();

      if (!playerCar || !_playerRigid || Time.timeSinceLevelLoad < 1.5f) {
        transform.position += Quaternion.identity * Vector3.forward * (Time.deltaTime * 3f);
      }
      else if (playerCar && _playerRigid) {
        //targetPosition = playerCar.position;
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _targetFieldOfView, Time.deltaTime * 3f);

        if (!gameover) {
          switch (cameraMode) {
            case CameraMode.Top:
              TOP();
              if (mirrors)
                mirrors.SetActive(false);
              break;

            case CameraMode.TPS:
              if (_tpsCam) {
                TPS();
              }
              else {
                _cameraSwitchCount++;
                ChangeCamera();
              }

              break;

            case CameraMode.FPS:
              if (_hoodCam) {
                FPS();
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

          if (HR_HighwayRacerProperties.Instance._shakeCamera)
            _targetPosition += (Random.insideUnitSphere * _speed * MaxShakeAmount);
        }
        else {
          if (Time.timeScale >= 1)
            transform.position = new Vector3(transform.position.x, transform.position.y,
              transform.position.z + Mathf.Clamp(_currentT, 0f, Mathf.Infinity));
        }

        cameraMode = _cameraSwitchCount switch {
          0 => CameraMode.Top,
          1 => CameraMode.TPS,
          2 => CameraMode.FPS,
          _ => cameraMode
        };
      }

      _audioListener.transform.position = new Vector3(playerCar.position.x, transform.position.y, transform.position.z);

      _pastFollowerPosition = transform.position;
      _pastTargetPosition = _targetPosition;

      _currentT = (transform.position.z - _oldT);
      _oldT = transform.position.z;
    }

    private void Update() {
      if (Input.GetKeyDown(RCC_Settings.Instance.changeCameraKB))
        ChangeCamera();
    }

    public void ChangeCamera() {
      _cameraSwitchCount++;

      if (_cameraSwitchCount >= 3)
        _cameraSwitchCount = 0;
    }

    private void TOP() {
      transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Rotation, 0f, 0f), Time.deltaTime * 2f);

      _targetPosition = new Vector3(0f, playerCar.position.y, playerCar.position.z);
      _targetPosition -= transform.rotation * Vector3.forward * distance;

      _targetPosition = new Vector3(_targetPosition.x, height, _targetPosition.z);

      if (Time.timeSinceLevelLoad < 3f)
        transform.position = SmoothApproach(_pastFollowerPosition, _pastTargetPosition, _targetPosition,
          (_speed / 2f) * Mathf.Clamp(Time.timeSinceLevelLoad - 1.5f, 0f, 10f));
      else
        transform.position = _targetPosition;

      _targetFieldOfView = topFOV;
    }

    void TPS() {
      transform.rotation =
        Quaternion.Lerp(transform.rotation, Quaternion.Euler(Rotation / 3f, 0f, 0f), Time.deltaTime * 2f);
      _targetPosition = new Vector3(playerCar.position.x, _tpsCam.transform.position.y, _tpsCam.transform.position.z);
      transform.position = _targetPosition;

      _targetFieldOfView = tpsFOV;
    }

    void FPS() {
      transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 2f);

      if (HR_HighwayRacerProperties.Instance._tiltCamera)
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
          transform.InverseTransformDirection(_playerRigid.velocity).x / 2f,
          -transform.InverseTransformDirection(_playerRigid.velocity).x / 2f);

      _targetPosition = _hoodCam.transform.position;
      transform.position = _targetPosition;
      _targetFieldOfView = fpsFOV;
    }

    // Used for smooth position lerping.
    private Vector3 SmoothApproach(Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition,
      float delta) {
      if (Time.timeScale == 0 || float.IsNaN(delta) || float.IsInfinity(delta) || delta == 0 ||
          pastPosition == Vector3.zero || pastTargetPosition == Vector3.zero || targetPosition == Vector3.zero)
        return transform.position;

      var t = (Time.deltaTime * delta) + .00001f;
      var v = (targetPosition - pastTargetPosition) / t;
      var f = pastPosition - pastTargetPosition + v;
      var l = targetPosition - v + f * Mathf.Exp(-t);

#if UNITY_2017_1_OR_NEWER
      if (l != Vector3.negativeInfinity && l != Vector3.positiveInfinity && l != Vector3.zero)
        return l;
      else
        return transform.position;
#else
		return l;
#endif
    }

    private void FixedUpdate() {
      if (!_playerRigid) return;

      _speed = Mathf.Lerp(_speed, (_playerRigid.velocity.magnitude * 3.6f), Time.deltaTime * 1.5f);
    }
  }
}