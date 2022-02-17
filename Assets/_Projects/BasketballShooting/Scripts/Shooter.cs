using System;
using UnityEngine;

namespace Project.BasketballShooting {
  public class Shooter : MonoBehaviour {
    public Camera cameraForShooter;
    public GameObject ballPrefab;
    public Transform shotPoint;

    [SerializeField] private float _targetZ = 12.0f; //screen point z to world point
    [SerializeField] private float _shotPowerMin = 3.0f; //minimum shot power
    [SerializeField] private float _shotPowerMax = 12.0f; //maximum shot power
    [SerializeField] private float _shotTimeMin = 0.2f; //minimum time till to release finger
    [SerializeField] private float _shotTimeMax = 0.55f; //maximum time till to release finger
    [SerializeField] private float _torque = 30.0f; //torque (backspin)
    [SerializeField] private float _offsetY = 100.0f; //offset Y for trajectory
    [SerializeField] private float _offsetZShotPoint = 1.0f; //for rolling ball
    [SerializeField] private float _powerToRoll = 2.0f; // roll new ball to the shoot point
    [SerializeField] private float _timeoutForShot = 5.0f; //for error handling

    // for demo
    public float ShotPower { get; private set; } //shot power (initial velocity)
    public Vector3 Direction { get; private set; } //shot direction (normalized)

    private enum ShotState {
      Charging,
      Ready,
      Swiping,
    }

    private ShotState _state = ShotState.Charging;
    private float _startTime;
    private Vector2 _touchPos = new Vector2(-1f, 0f);
    private GameObject _ballGo;
    private Rigidbody _ballRigidbody;

    private void Update() {
      switch (_state) {
        case ShotState.Charging:
          ChargeBall();
          CheckTrigger();
          break;
        case ShotState.Ready:
          CheckTrigger();
          break;
        case ShotState.Swiping:
          CheckShot();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void FixedUpdate() {
      if (_state != ShotState.Charging) {
        _ballRigidbody.velocity = Vector3.zero;
        _ballRigidbody.angularVelocity = Vector3.zero;
      }
    }

    private void ChargeBall() {
      if (_ballGo == null) {
        // spawn ball
        _ballGo = Instantiate(ballPrefab);
        _ballGo.AddComponent<ShotBall>();
        _ballRigidbody = _ballGo.GetComponent<Rigidbody>();

        // roll ball to shoot point
        var shotPos = shotPoint.transform.localPosition.OffsetZ(-_offsetZShotPoint);
        _ballGo.transform.position = shotPoint.transform.TransformPoint(shotPos);
        _ballGo.transform.eulerAngles = shotPoint.transform.eulerAngles;
        _ballRigidbody.velocity = Vector3.zero;
        _ballRigidbody.AddForce(shotPoint.transform.TransformDirection(0.0f, 0.0f, _powerToRoll), ForceMode.Impulse);
      }

      // snap ball to shoot point position and change state
      var ballToShotPointDist = Vector3.Distance(shotPoint.transform.position, _ballGo.transform.position);
      if (ballToShotPointDist <= 0.2f) {
        _state = ShotState.Ready;
        _ballGo.transform.position = shotPoint.transform.position;
      }
    }

    private void CheckTrigger() {
      if (_touchPos.x < 0) {
        if (Input.GetMouseButtonDown(0)) {
          // UTIL: Get component at mouse position
          Ray ray = cameraForShooter.ScreenPointToRay(Input.mousePosition);
          RaycastHit hit;
          if (Physics.Raycast(ray, out hit, 100)) {
            var ball = hit.collider.transform.GetComponent<ShotBall>();
            if (ball != null && !ball.IsActive) {
              ball.SetActive();
              _touchPos = Input.mousePosition;
              ShotPower = 0.0f;
            }
          }
        }
      }
      else if (Math.Abs(_touchPos.x - Input.mousePosition.x) > .1f ||
               Math.Abs(_touchPos.y - Input.mousePosition.y) > .1f) {
        _touchPos.x = -1.0f;
        _startTime = Time.time;
        _state = ShotState.Swiping;
      }
    }

    private void CheckShot() {
      var elapseTime = Time.time - _startTime;

      if (Input.GetMouseButtonUp(0)) {
        if (_ballGo != null) {
          ShootBall(elapseTime);
        }

        _state = ShotState.Charging;
        _ballGo = null;
      }

      if (_timeoutForShot < elapseTime) {
        Destroy(_ballGo);
        _state = ShotState.Charging;
        _ballGo = null;
      }
    }

    private void ShootBall(float elapseTime) {
      if (elapseTime < _shotTimeMin) {
        ShotPower = _shotPowerMax;
      }
      else if (_shotTimeMax < elapseTime) {
        ShotPower = _shotPowerMin;
      }
      else {
        var tmin100 = _shotTimeMin * 10000.0f;
        var tmax100 = _shotTimeMax * 10000.0f;
        var ep100 = elapseTime * 10000.0f;
        var rate = (ep100 - tmin100) / (tmax100 - tmin100);
        ShotPower = _shotPowerMax - (_shotPowerMax - _shotPowerMin) * rate;
      }

      var screenPoint = Input.mousePosition.WithZ(_targetZ);
      var worldPoint = cameraForShooter.ScreenToWorldPoint(screenPoint);
      worldPoint.y += (_offsetY / ShotPower);
      Direction = (worldPoint - shotPoint.transform.position).normalized;

      _ballRigidbody.velocity = Direction * ShotPower;
      _ballRigidbody.AddTorque(-shotPoint.transform.right * _torque);
    }
  }
}