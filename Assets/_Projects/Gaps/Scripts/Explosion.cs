using UnityEngine;

namespace Project.Gaps {
  public class Explosion : MonoBehaviourBase {
    public enum ExplosionType {
      Sphere,
      Cube
    }

    public float elementSize = 0.2f;

    public int elementsInRow = 5;

    public float explosionForce = 50f;

    public float explosionRadius = 4f;

    public float explosionUpward = 0.4f;

    public ExplosionType explosionElement;

    public Material material;

    private float _cubesPivotDistance;

    private Vector3 _cubesPivot;

    protected override void Start() {
      _cubesPivotDistance = elementSize * elementsInRow / 2f;
      _cubesPivot = _cubesPivotDistance.ToVector3();
    }

    public void Explode() {
      gameObject.SetActive(value: false);

      for (var i = 0; i < elementsInRow; i++) {
        for (var j = 0; j < elementsInRow; j++) {
          for (var k = 0; k < elementsInRow; k++) {
            CreatePiece(i, j, k);
          }
        }
      }

      var array = Physics.OverlapSphere(_position, explosionRadius);
      foreach (var theCollider in array) {
        if (theCollider.TryGetComponent(out Rigidbody theRigidBody)) {
          theRigidBody.AddExplosionForce(explosionForce, _position, explosionRadius, explosionUpward);
        }
      }
    }

    private void CreatePiece(int x, int y, int z) {
      var pieceGo = PrimitiveUtils.CreatePrimitive(explosionElement);

      pieceGo.transform.position = _position + new Vector3(x, y, z) * elementSize - _cubesPivot;
      pieceGo.transform.localScale = new Vector3(elementSize, elementSize, elementSize);
      pieceGo.AddComponent<Rigidbody>().mass = elementSize;
      pieceGo.GetComponent<MeshRenderer>().material = material;
    }
  }
}