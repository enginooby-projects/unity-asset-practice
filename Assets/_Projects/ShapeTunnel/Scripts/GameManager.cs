using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Project.ShapeTunnel {
  public class GameManager : MonoBehaviour {
    public GameObject startPanel, endPanel, muteImage;
    public TextMeshProUGUI scoreText, highScoreText, endScoreText, endHighScoreText;

    private GameObject actPlayer;

    void Start() {
      //UNCOMMENT THE FOLLOWING LINES IF YOU ENABLED UNITY ADS AT UNITY SERVICES AND REOPENED THE PROJECT!
      //if (FindObjectOfType<AdManager>().unityAds)
      //    CallUnityAds();     //Calls Unity Ads
      //else
      CallAdmobAds(); //Calls Admob Ads

      StartPanelActivation();
      HighScoreCheck();
      AudioCheck();
    }

    //UNCOMMENT THE FOLLOWING LINES IF YOU ENABLED UNITY ADS AT UNITY SERVICES AND REOPENED THE PROJECT!
    //public void CallUnityAds()
    //{
    //    if (Time.time != Time.timeSinceLevelLoad)
    //        FindObjectOfType<AdManager>().ShowUnityVideoAd();      //Shows Interstitial Ad when game starts (except for the first time)
    //    FindObjectOfType<AdManager>().HideAdmobBanner();
    //}

    public void CallAdmobAds() {
      FindObjectOfType<AdManager>().ShowAdmobBanner(); //Shows Banner Ad when game starts
      if (Math.Abs(Time.time - Time.timeSinceLevelLoad) > .1f)
        FindObjectOfType<AdManager>().ShowAdmobInterstitial(); //Shows Interstitial Ad when game starts (except for the first time)
    }

    public void Initialize() {
      actPlayer = GameObject.FindGameObjectWithTag("Player");
      actPlayer.GetComponent<Rigidbody>().isKinematic = true;
      FindObjectOfType<PlayerMovement>().enabled = false;
      FindObjectOfType<PlayerParticleController>().enabled = false;
      FindObjectOfType<Spawner>().enabled = false;
      FindObjectOfType<PipeMove>().enabled = false;
      scoreText.enabled = false;
    }

    public void StartPanelActivation() {
      startPanel.SetActive(true);
      endPanel.SetActive(false);
    }

    public void EndPanelActivation() {
      startPanel.SetActive(false);
      endPanel.SetActive(true);
      scoreText.enabled = false;
      endScoreText.text = scoreText.text;
      HighScoreCheck();
    }

    public void SkinsPanelActivation() => startPanel.SetActive(false);

    public void HighScoreCheck() {
      if (FindObjectOfType<ScoreManager>().score > PlayerPrefs.GetInt("HighScore", 0)) {
        PlayerPrefs.SetInt("HighScore", FindObjectOfType<ScoreManager>().score);
      }

      highScoreText.text = "BEST " + PlayerPrefs.GetInt("HighScore", 0).ToString();
      endHighScoreText.text = "BEST " + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    public void AudioCheck() {
      if (PlayerPrefs.GetInt("Audio", 0) == 0) {
        muteImage.SetActive(false);
        FindObjectOfType<AudioManager>().soundIsOn = true;
        FindObjectOfType<AudioManager>().PlayBgm();
      }
      else {
        muteImage.SetActive(true);
        FindObjectOfType<AudioManager>().soundIsOn = false;
        FindObjectOfType<AudioManager>().StopBgm();
      }
    }

    public void StartButton() {
      actPlayer = GameObject.FindGameObjectWithTag("Player");
      actPlayer.GetComponent<Rigidbody>().isKinematic = false;
      FindObjectOfType<PlayerMovement>().enabled = true;
      FindObjectOfType<Spawner>().enabled = true;
      FindObjectOfType<PlayerParticleController>().enabled = true;
      FindObjectOfType<PipeMove>().enabled = true;
      scoreText.enabled = true;
      startPanel.SetActive(false);
    }

    public void RestartButton() {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SkinsBackButton() {
      StartPanelActivation();
    }

    public void AudioButton() {
      if (PlayerPrefs.GetInt("Audio", 0) == 0)
        PlayerPrefs.SetInt("Audio", 1);
      else
        PlayerPrefs.SetInt("Audio", 0);
      AudioCheck();
    }

    public void SkinsButton() {
      SkinsPanelActivation();
    }
  }
}