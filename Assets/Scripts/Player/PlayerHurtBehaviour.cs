using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBehaviour : StateMachineBehaviour
{
    // アニメーション開始時に実行される：Start関数のようなもの
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Hurt");
        // 速度を0にしたい
        animator.GetComponent<PlayerManager>().moveSpeed = 0.4f;
        animator.GetComponent<PlayerManager>().HideColliderWeapon();
    }

    // アニメーション中に実行される：Update関数のようなもの
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 速度を0にしたい
        animator.GetComponent<PlayerManager>().moveSpeed = 0.4f;
    }

    // アニメーションの遷移が行われる時
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Hurt");
        // 速度を戻す
        animator.GetComponent<PlayerManager>().moveSpeed = 3;
    }
}