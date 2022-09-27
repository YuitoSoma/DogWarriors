using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speeder : MonoBehaviour
{
    public float speed = 5.0f;
    public Collider playerCollider;
    AudioSource speedSound;

    void Start()
    {
        speedSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
            speedSound.Play();
    }
}
