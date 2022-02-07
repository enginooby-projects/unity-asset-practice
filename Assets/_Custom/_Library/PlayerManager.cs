using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

// FIX: Wolf GameObject make itself not child of Player Wolf anymore when reactivate 
// FIX: Wolf does not turn following its camera after switching 
public class PlayerManager : MonoBehaviour {
  [SerializeField] private KeyCode switchPlayerKey = KeyCode.P;
  [SerializeField] private List<GameObject> players;
  [SerializeField] [ValueDropdown(nameof(players))] private GameObject currentPlayer;

  private void Start() {
    players.ForEach(DeactivateIfNotCurrent);
    // currentPlayer.gameObject.SetActive(true);
  }

  private void DeactivateIfNotCurrent(GameObject go) {
    if (go != currentPlayer) go.SetActive(false);
  }

  void Update() {
    if (switchPlayerKey.IsUp()) SwitchNextCamera();
  }

  public void SwitchNextCamera() {
    // Vector3 currentPos = currentPlayer.transform.position;
    // Vector3 dest = players.NavNext(currentPlayer).transform.position;

    // currentPlayer.transform.DOMove(dest, 1f);
    currentPlayer.gameObject.SetActive(false);
    // currentPlayer.transform.DOMove(currentPos, 1f);
    currentPlayer = players.NavNext(currentPlayer);
    currentPlayer.gameObject.SetActive(true);
  }
}
