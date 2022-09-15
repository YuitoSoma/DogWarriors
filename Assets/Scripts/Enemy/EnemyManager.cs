using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    Animator animator;
    public Collider weaponCollider;
    public EnemyUIManager enemyUIManager;
    public GameObject gameClearText;

    public int maxHp;
    int hp;

    void Start()
    {
        hp = maxHp;
        enemyUIManager.Init(this);

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;
        HideColliderWeapon();
    }

    void Update()
    {
        agent.destination = target.position * 0.5f;
        animator.SetFloat("Distance", agent.remainingDistance);
    }

    public void LookAtTarget()
    {
        transform.LookAt(target);
    }

    // •Ší‚Ì”»’è‚ğ—LŒø‚É‚µ‚½‚èEÁ‚µ‚½‚è‚·‚éŠÖ”
    public void HideColliderWeapon()
    {
        weaponCollider.enabled = false;
    }

    public void ShowColliderWeapon()
    {
        weaponCollider.enabled = true;
    }

    void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            animator.SetTrigger("Die");
            Destroy(gameObject, 2.0f);
            gameClearText.SetActive(true);
        }
        enemyUIManager.UpdateHP(hp);
    }

    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            animator.SetTrigger("Hurt");
            Damage(damager.damage);
        }
    }
}
