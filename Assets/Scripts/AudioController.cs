using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    public AudioClip boom, fire, pick, click;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayBoom()
    {
        audioSource.clip = boom;
        audioSource.Play();
    }
    public void PlayFire()
    {
        audioSource.clip = fire;
        audioSource.Play();
    }
    public void PlayPick()
    {
        audioSource.clip = pick;
        audioSource.Play();
    }
    public void PlayClick()
    {
        audioSource.clip = click;
        audioSource.Play();
    }
}
