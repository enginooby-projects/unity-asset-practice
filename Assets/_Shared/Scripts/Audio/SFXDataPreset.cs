using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enginoobz.Audio {
  [CreateAssetMenu(fileName = "SFXPreset_", menuName = "Audio/SFX Preset", order = 0)]
  public class SFXDataPreset : ScriptableObject {
    [SerializeField, InlineEditor]
    private List<SFXData> _sfxDatas = new List<SFXData>();

    public void PlayRandom(AudioSource audioSource, SFXTarget sfxTarget, SFXAction sfxAction) {
      // TODO: improve matching (target is optional, target Any cover all)
      _sfxDatas.Find(sfxData => sfxData.Target == sfxTarget && sfxData.Action == sfxAction).PlayRandom(audioSource);
    }
  }
}
