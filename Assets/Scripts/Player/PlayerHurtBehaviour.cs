using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBehaviour : StateMachineBehaviour
{
    // �A�j���[�V�����J�n���Ɏ��s�����FStart�֐��̂悤�Ȃ���
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Hurt");
        // ���x��0�ɂ�����
        animator.GetComponent<PlayerManager>().moveSpeed = 0.4f;
        animator.GetComponent<PlayerManager>().HideColliderWeapon();
    }

    // �A�j���[�V�������Ɏ��s�����FUpdate�֐��̂悤�Ȃ���
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���x��0�ɂ�����
        animator.GetComponent<PlayerManager>().moveSpeed = 0.4f;
    }

    // �A�j���[�V�����̑J�ڂ��s���鎞
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Hurt");
        // ���x��߂�
        animator.GetComponent<PlayerManager>().moveSpeed = 3;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }
}
