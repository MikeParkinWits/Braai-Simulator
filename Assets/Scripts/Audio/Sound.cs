using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float Volume;

    [Range(0.1f, 3f)]
    public float Pitch;

    [Range(0f, 1f)]
    public float Spatial;

    public float Min;
    public float Max;

    public bool Loop;
    public bool Awake;

    [HideInInspector]
    public AudioSource source;
}
