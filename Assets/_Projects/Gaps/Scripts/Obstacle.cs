using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Gaps {
  public class Obstacle : MonoBehaviourBase {
    [Space(10f)] public float rotationSpeedMin;

    public float rotationSpeedMax;

    [Space(10f)] public float randomXMin;

    public float randomXMax;

    [Space(10f)] [Range(0f, 12f)] public int maxNumberOfSpawnedLittleBalls;

    public GameObject littleBallPrefab;

    public float littleBallDistanceFromCenter;

    private GameObject _playerGo;

    private GameObject _tempLittleBall;

    private GameObject _obstacle;

    private float _rotationSpeedY;

    private bool _changed;

    protected override void Start() => InitObstacle();

    public void SetPlayerObject(GameObject po, int index) {
      _playerGo = po;
      if (index == 0) randomXMax = randomXMin = 0f;
    }

    protected override void Update() {
      _obstacle.transform.Rotate(0f, _rotationSpeedY, 0f);
      var playerPosition = _playerGo.transform.position;

      if (_position.z < playerPosition.z - 10f) {
        var enumerator = Transform.GetEnumerator(); // ? children
        try {
          while (enumerator.MoveNext()) {
            var transform = (Transform) enumerator.Current;
            Destroy(transform.gameObject);
          }
        }
        finally {
          var disposable = enumerator as IDisposable;
          disposable?.Dispose();
        }

        Destroy(gameObject);
      }
    }

    private void InitObstacle() {
      _obstacle = Transform.GetChild(0).gameObject;
      _position = _position.WithXRandom(randomXMin, randomXMax);
      var isClockwise = Random.Range(0, 100) < 50;
      _rotationSpeedY = Random.Range(rotationSpeedMin, rotationSpeedMax) * isClockwise.ToInt(1,-1);
      InitLittleBalls();
    }

    private void InitLittleBalls() {
      if (maxNumberOfSpawnedLittleBalls <= 0) return;

      var littleBallAmount = Random.Range(1, maxNumberOfSpawnedLittleBalls);
      for (var i = 0; i <= littleBallAmount; i++) {
        float angle = 360 / littleBallAmount * i;
        float posX = _position.x + littleBallDistanceFromCenter * Mathf.Sin(angle.FromDegreeToRadian());
        float poxZ = _position.z + littleBallDistanceFromCenter * Mathf.Cos(angle.FromDegreeToRadian());

        _tempLittleBall = Instantiate(littleBallPrefab, Transform);
        _tempLittleBall.transform.eulerAngles = new Vector3(0f, angle, 0f);
        _tempLittleBall.transform.position = new Vector3(posX, .8f, poxZ);
      }
    }
  }
}