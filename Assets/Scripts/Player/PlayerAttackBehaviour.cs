using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 速度を0
        animator.GetComponent<PlayerManager>().currentSpeed = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 速度を0
        animator.GetComponent<PlayerManager>().currentSpeed = 0;
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float speed = animator.GetComponent<PlayerManager>().speed;
        animator.GetComponent<PlayerManager>().currentSpeed = speed;
    }
}