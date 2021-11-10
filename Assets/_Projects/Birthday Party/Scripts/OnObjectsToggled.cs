using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Birthday.NA {
  [RequireComponent(typeof(Collider))]
  public class OnObjectsToggled : MonoBehaviour {
    public List<GameObject> targets = new List<GameObject>();
    public AudioClip sfx;
    Camera mainCam;

    private void Start() {
      mainCam = Camera.main;
    }

    private void OnMouseDown() {
      targets.ToggleActive();
      AudioSource.PlayClipAtPoint(sfx, mainCam.transform.position);
    }
  }
}
