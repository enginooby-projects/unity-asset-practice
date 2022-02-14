using UnityEngine;

namespace Project.ShapeTunnel {
  /// <summary>
  /// On the player
  /// </summary>
  public class Collision : MonoBehaviourBase {
    public Mesh[] meshes;
    public Material[] playerMaterials;
    public ParticleSystem collisionParticle, tokenParticle;
    public ParticleSystemRenderer basicParticleRenderer, deathParticleRenderer; // ? Necessary

    [HideInInspector] public bool gameIsOver; // ? Move to GameManager

    public void OnTriggerEnter(Collider other) {
      // PARAM: Serialize tags
      if (other.CompareTag("Cube Instance", "Sphere Instance", "Prism Instance")) {
        HitShapeObstacle(other);
      }
      else if (other.CompareTag("Token")) {
        CollectToken(other);
      }
    }

    private void HitShapeObstacle(Collider other) => other.CompareTag(_meshFilter.mesh.name,
      trueAction: HitCorrectObstacle, falseAction: HitWrongObstacle);

    private void HitCorrectObstacle() {
      basicParticleRenderer.material = _meshRenderer.material = playerMaterials.GetRandom();
      _meshFilter.mesh = meshes.GetRandom();
      DisableColliderForSecs(.1f); // To avoid multiple collisions at the same time
      SpawnCollisionVfx();
      // OPTI: Cache
      FindObjectOfType<ScoreManager>().IncrementScore();
      FindObjectOfType<AudioManager>().PlayScoreSound();
    }

    private void HitWrongObstacle() {
      gameIsOver = true;
      deathParticleRenderer.material = _meshRenderer.material;
      FindObjectOfType<PipeMove>().StopPipes();
      FindObjectOfType<AudioManager>().PlayDeathSound();
      FindObjectOfType<GameManager>().EndPanelActivation();
      FindObjectOfType<Spawner>().enabled = false;
      StopPlayer();
      _collider.enabled = false;
      //particleRen.enabled = trailRen.enabled = playerRen.enabled = false;
      //FindObjectOfType<JumpManager>().enabled = false;
    }

    private void CollectToken(Collider token) {
      FindObjectOfType<AudioManager>().PlayTokenSound();
      FindObjectOfType<ScoreManager>().IncrementToken();
      Destroy(token.gameObject);
      SpawnTokenVfx();
    }

    public void StopPlayer() {
      _meshRenderer.enabled = false;
      _collider.enabled = false;
      _rigidbody.isKinematic = true;
      FindObjectOfType<PlayerMovement>().enabled = false;
      FindObjectOfType<PlayerParticleController>().PlayDeathParticle();
    }

    public void SpawnCollisionVfx() => Instantiate(collisionParticle, lifespan: 1f).WithColorOf(_meshRenderer).Play();

    public void SpawnTokenVfx() => Instantiate(tokenParticle, lifespan: 1f);
  }
}