using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Project.ShapeTunnel {
  public class ScoreManager : MonoBehaviour {
    public TextMeshProUGUI scoreText, tokenText;

    [HideInInspector] public int score;

    private void Start() => AddTokenAndUpdateText(0);

    private void AddTokenAndUpdateText(int amount) {
      PlayerPrefs.SetInt("Token", PlayerPrefs.GetInt("Token", 0) + amount);
      tokenText.text = PlayerPrefs.GetInt("Token", 0).ToString();
    }

    // in game play
    public void IncrementScore() {
      if (!FindObjectOfType<Collision>().gameIsOver)
        scoreText.text = (++score).ToString();
    }

    // in game play
    public void IncrementToken() {
      if (!FindObjectOfType<Collision>().gameIsOver) {
        AddTokenAndUpdateText(1);
      }
    }

    // by ad
    public void IncrementToken(int countOfToken) {
      if (countOfToken > 0) FindObjectOfType<AudioManager>().PlayTokenSound();
      AddTokenAndUpdateText(countOfToken);
    }
  }
}