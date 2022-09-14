using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogManager : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            animator.SetFloat("MoveSpeed", 1.1f);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetFloat("MoveSpeed", 0.0f);
        }
    }
}
