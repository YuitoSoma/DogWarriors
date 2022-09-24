using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancer : MonoBehaviour
{
    public int attack = 50;
    public Collider playerCollider;
    AudioSource attackSound;

    void Start()
    {
        attackSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            attackSound.Play();
        }
    }
}
