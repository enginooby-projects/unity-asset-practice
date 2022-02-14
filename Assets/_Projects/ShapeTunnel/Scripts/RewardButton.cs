using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Project.ShapeTunnel {
  public class RewardButton : MonoBehaviour {
    public int rewardCount = 5;

    //For example, if the rewardCount equals 5, then the text is: +5
    private void Start() => transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + rewardCount;

    public void AddReward() => FindObjectOfType<ScoreManager>().IncrementToken(rewardCount);

    public void ShowRewardVideo() {
      //UNCOMMENT THE FOLLOWING LINES IF YOU ENABLED UNITY ADS AT UNITY SERVICES AND REOPENED THE PROJECT!
      //if (FindObjectOfType<AdManager>().unityAds)
      //    FindObjectOfType<AdManager>().ShowUnityRewardVideoAd();       //Shows Unity Reward Video ad
      //else
      FindObjectOfType<AdManager>().ShowAdmobRewardVideo(); //Shows Admob Reward Video ad
    }
  }
}