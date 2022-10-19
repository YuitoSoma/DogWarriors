using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancer : MonoBehaviour
{
    public int attack = 5;
    public Collider playerCollider;
    public AudioSource attackSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            attackSound = GetComponent<AudioSource>();
            attackSound.Play();
            
        }
    }
}
