using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Project.Birthday.NA {
  public class OnCandleBlowed : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip blowSfx;
    public AudioClip coffetiSfx;
    public ParticleSystem coffetiVfx;
    public GameObject candle1Flame;
    public GameObject candle2Flame;
    public Transform player;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController playerController;
    public Transform lookAtPoint;


    void Start() {
    }

    private IEnumerator OnMouseDown() {
      GetComponent<SkinnedMeshRenderer>().enabled = false;
      GetComponent<Collider>().enabled = false;
      playerController.enabled = false;
      player.DOLookAt(lookAtPoint.position, 1);
      yield return new WaitForSeconds(1);

      AudioSource.PlayClipAtPoint(blowSfx, player.position);
      yield return new WaitForSeconds(1);
      Destroy(candle1Flame);

      AudioSource.PlayClipAtPoint(blowSfx, player.position);
      yield return new WaitForSeconds(1);
      Destroy(candle2Flame);

      AudioSource.PlayClipAtPoint(coffetiSfx, player.position);
      coffetiVfx.Play();

      Invoke(nameof(DisableCoffetiVfx), 5);
      audioSource.Play();

      yield return new WaitForSeconds(1);
      playerController.enabled = true;
    }

    private void DisableCoffetiVfx() {
      coffetiVfx.Stop();
    }
  }
}
