using UnityEngine;
using UnityEngine.Audio;

namespace Project.HighwayRacer
{
  public class HR_CreateAudioSource : MonoBehaviour
  {
    /// <summary>
    /// Creates new audio source with specified settings.
    /// </summary>
    public static AudioSource NewAudioSource(AudioMixerGroup audioMixer, GameObject go, string audioName,
      float minDistance, float maxDistance, float volume, AudioClip audioClip, bool loop, bool playNow,
      bool destroyAfterFinished)
    {
      var audioSourceObject = new GameObject(audioName);

      if (go.transform.Find("All Audio Sources"))
      {
        audioSourceObject.transform.SetParent(go.transform.Find("All Audio Sources"));
      }
      else
      {
        var allAudioSources = new GameObject("All Audio Sources");
        allAudioSources.transform.SetParent(go.transform, false);
        audioSourceObject.transform.SetParent(allAudioSources.transform, false);
      }

      audioSourceObject.transform.position = go.transform.position;
      audioSourceObject.transform.rotation = go.transform.rotation;

      audioSourceObject.AddComponent<AudioSource>();
      var source = audioSourceObject.GetComponent<AudioSource>();

      if (audioMixer)
        source.outputAudioMixerGroup = audioMixer;

      //audioSource.GetComponent<AudioSource>().priority =1;
      source.minDistance = minDistance;
      source.maxDistance = maxDistance;
      source.volume = volume;
      source.clip = audioClip;
      source.loop = loop;
      source.dopplerLevel = .5f;

      if (minDistance == 0 && maxDistance == 0)
        source.spatialBlend = 0f;
      else
        source.spatialBlend = 1f;

      if (playNow)
      {
        source.playOnAwake = true;
        source.Play();
      }
      else
      {
        source.playOnAwake = false;
      }

      if (destroyAfterFinished)
      {
        if (audioClip)
          Destroy(audioSourceObject, audioClip.length);
        else
          Destroy(audioSourceObject);
      }

      return source;
    }

    /// <summary>
    /// Creates new audio source with specified settings.
    /// </summary>
    public static AudioSource NewAudioSource(GameObject go, string audioName, float minDistance, float maxDistance,
      float volume, AudioClip audioClip, bool loop, bool playNow, bool destroyAfterFinished)
    {
      var audioSourceObject = new GameObject(audioName);

      if (go.transform.Find("All Audio Sources"))
      {
        audioSourceObject.transform.SetParent(go.transform.Find("All Audio Sources"));
      }
      else
      {
        GameObject allAudioSources = new GameObject("All Audio Sources");
        allAudioSources.transform.SetParent(go.transform, false);
        audioSourceObject.transform.SetParent(allAudioSources.transform, false);
      }

      audioSourceObject.transform.position = go.transform.position;
      audioSourceObject.transform.rotation = go.transform.rotation;

      audioSourceObject.AddComponent<AudioSource>();
      var source = audioSourceObject.GetComponent<AudioSource>();

      //audioSource.GetComponent<AudioSource>().priority =1;
      source.minDistance = minDistance;
      source.maxDistance = maxDistance;
      source.volume = volume;
      source.clip = audioClip;
      source.loop = loop;
      source.dopplerLevel = .5f;

      if (minDistance == 0 && maxDistance == 0)
        source.spatialBlend = 0f;
      else
        source.spatialBlend = 1f;

      if (playNow)
      {
        source.playOnAwake = true;
        source.Play();
      }
      else
      {
        source.playOnAwake = false;
      }

      if (destroyAfterFinished)
      {
        if (audioClip)
          Destroy(audioSourceObject, audioClip.length);
        else
          Destroy(audioSourceObject);
      }

      return source;
    }

    /// <summary>
    /// Adds High Pass Filter to audio source. Used for turbo.
    /// </summary>
    public static void NewHighPassFilter(AudioSource source, float freq, int level)
    {
      if (source == null) return;

      var highFilter = source.gameObject.AddComponent<AudioHighPassFilter>();
      highFilter.cutoffFrequency = freq;
      highFilter.highpassResonanceQ = level;
    }

    /// <summary>
    /// Adds Low Pass Filter to audio source. Used for engine off sounds
    /// </summary>
    public static void NewLowPassFilter(AudioSource source, float freq)
    {
      if (source == null) return;

      var lowFilter = source.gameObject.AddComponent<AudioLowPassFilter>();
      lowFilter.cutoffFrequency = freq;
    }
  }
}