﻿using UnityEngine;

namespace Project.HighwayRacer {
  public class HR_ReflectiveShaderSwitcher : MonoBehaviour {
    private Cubemap reflectiveCube;
    public Material[] reflectiveMaterials;
    public float reflectionIntensity = 1f;

    void Start() {
      reflectiveCube = RenderSettings.customReflection;

      for (int i = 0; i < reflectiveMaterials.Length; i++) {
        reflectiveMaterials[i].SetTexture("_Cube", reflectiveCube);
        reflectiveMaterials[i].SetColor("_ReflectColor", Color.white * reflectionIntensity);
      }
    }
  }
}