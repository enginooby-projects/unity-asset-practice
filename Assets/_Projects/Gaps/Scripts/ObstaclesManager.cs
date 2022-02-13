using UnityEngine;

namespace Project.Gaps {
	public class ObstaclesManager : MonoBehaviour {
		public GameObject player;

		[Space(10f)] public GameObject[] obstacles;

		[Space(10f)] public float distanceBetweenObstacles = 8f;

		[Space(10f)] public GameObject doorPrefab;

		private GameObject _currentObstacle;

		private GameObject _newObstacle;

		private GameObject _door;

		private int _obstacleIndex;

		private void Start() {
			var original = obstacles[_obstacleIndex];
			var position = obstacles[_obstacleIndex].transform.position;
			
			_currentObstacle = Instantiate(original, new Vector3(0f, position.y, distanceBetweenObstacles),
				Quaternion.identity);
			_currentObstacle.transform.SetParent(transform);
			_currentObstacle.transform.GetComponent<Obstacle>().SetPlayerObject(player, 0);

			var position1 = _currentObstacle.transform.position;
			var position2 = position1;
			var y = position2.y;
			var position3 = position1;
			position1 = new Vector3(0f, y, position3.z);
			_currentObstacle.transform.position = position1;
			var position4 = doorPrefab.transform.position;
			float y2 = position4.y;
			Vector3 position5 = position1;
			_door = Instantiate(doorPrefab, new Vector3(0f, y2, position5.z - distanceBetweenObstacles / 2f),
				Quaternion.identity);
		}

		private void CreateObstacle() {
			_obstacleIndex = UnityEngine.Random.Range(0, obstacles.Length);
			GameObject original = obstacles[_obstacleIndex];
			Vector3 position = obstacles[_obstacleIndex].transform.position;
			float y = position.y;
			Vector3 position2 = _currentObstacle.transform.position;
			_newObstacle = Instantiate(original, new Vector3(0f, y, position2.z + 8f), Quaternion.identity);
			_newObstacle.transform.SetParent(base.transform);
			_newObstacle.transform.GetComponent<Obstacle>().SetPlayerObject(player, 1);
			GameObject original2 = doorPrefab;
			Vector3 position3 = doorPrefab.transform.position;
			float y2 = position3.y;
			Vector3 position4 = _newObstacle.transform.position;
			_door = UnityEngine.Object.Instantiate(original2, new Vector3(0f, y2, position4.z - distanceBetweenObstacles / 2f),
				Quaternion.identity);
			Transform transform = _door.transform;
			Vector3 position5 = _newObstacle.transform.position;
			float x = position5.x;
			Vector3 position6 = _currentObstacle.transform.position;
			float x2 = (x + position6.x) / 2f;
			Vector3 position7 = _door.transform.position;
			float y3 = position7.y;
			Vector3 position8 = _door.transform.position;
			transform.position = new Vector3(x2, y3, position8.z);
			_currentObstacle = _newObstacle;
		}

		private void Update() {
			Vector3 position = player.transform.position;
			float z = position.z;
			Vector3 position2 = _currentObstacle.transform.position;
			if (z > position2.z - 30f) {
				CreateObstacle();
			}
		}
	}
}