using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    public int heal = 50;
    AudioSource healSound;

    void Start()
    {
        healSound = GetComponent<AudioSource>();
    }

    public void SoundPlay()
    {
        healSound.Play();
    }
}
