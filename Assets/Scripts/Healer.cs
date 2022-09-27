using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    public int heal = 20;
    public Collider playerCollider;
    AudioSource healSound;

    void Start()
    {
        healSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
            healSound.Play();
    }
}
