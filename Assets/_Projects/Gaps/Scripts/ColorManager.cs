using System.Collections;
using Enginooby.Utils;
using UnityEngine;

namespace Project.Gaps {
  public class ColorManager : MonoBehaviour {
    public Material backgroundMainMat;

    public Material backgroundSubMat;

    public Material playerMainMat;

    public Material ground1Mat;

    public Material ground2Mat;

    public Material obstacleMat;

    public Player playerScript;

    public ColorList[] colorSets;

    private ColorList _currentColorSet;

    private void Start() => ChangeColors();

    public void ChangeColors() {
      _currentColorSet = colorSets.GetRandomOtherThan(_currentColorSet);
      ChangeColor(ground1Mat, _currentColorSet.Ground1);
      ChangeColor(ground2Mat, _currentColorSet.Ground2);
      ChangeColor(obstacleMat, _currentColorSet.Obstacle);
      ChangeColor(playerMainMat, _currentColorSet.PlayerMain);
      ChangeColor(backgroundMainMat, _currentColorSet.backgroundMain);
      ChangeColor(backgroundSubMat, _currentColorSet.backgroundSub);
    }

    private void ChangeColor(Material mat, Color endColor) {
      StartCoroutine(TweenColorCoroutine(mat, mat.color, endColor, Time.time, 1));
    }

    // UTIL: Decouple Player
    private IEnumerator TweenColorCoroutine(
      Material mat,
      Color startColor,
      Color endColor,
      float time,
      float alpha = 1f) {
      var t = 0f;

      while (t < 1f && !playerScript.IsDead) {
        t += Time.deltaTime;
        mat.color = Color.Lerp(startColor, endColor, t);
        var color = mat.color;
        mat.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1f, alpha, t));
        yield return 0;
      }
    }
  }
}