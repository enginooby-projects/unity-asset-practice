using UnityEngine;
using static UnityEngine.GameObject;

namespace Project.ShapeTunnel {
  public class PlayerParticleController : MonoBehaviour {
    private ParticleSystem _basicParticle, _deathParticle;

    private void Start() => InitializeParticles();

    public void InitializeParticles() {
      _basicParticle = FindGameObjectWithTag("BasicParticle").GetComponent<ParticleSystem>(); //Gets the basicParticle
      _deathParticle = FindGameObjectWithTag("DeathParticle").GetComponent<ParticleSystem>(); //Gets the deathParticle
      _basicParticle.Play();
    }

    public void SetBasicParticleColor(GameObject obj) {
      var tempColor = GetColor(obj); //TempColor is the same color as the colorChanger's color
      tempColor.a = 1f;
      SetColor(_basicParticle, tempColor);
    }

    public void PlayDeathParticle() {
      SetColor(_deathParticle,
        GetColor(_basicParticle)); //Sets deathParticle's color identical to basicParticle's color
      _basicParticle.Stop();
      _deathParticle.Play();
    }

    //COLOR FUNCTIONS----------------------------------
    private Color GetColor(GameObject obj) => obj.GetComponent<Renderer>().material.color;

    private Color GetColor(ParticleSystem obj) => obj.GetComponent<Renderer>().material.color;

    public void SetColor(GameObject obj, Color color) => obj.GetComponent<Renderer>().material.color = color;

    public void SetColor(ParticleSystem obj, Color color) => obj.GetComponent<Renderer>().material.color = color;
  }
}