using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Animator animator;
    EnemyResponcer enemyResponcer;
    Collider axeCollider;
    Collider swordCollider;
    Collider enemyCollider;
    EnemyUIManager enemyUIManager;
    GameObject gameClearText;

    public static int counter;
    public int maxHp = 50;
    int hp;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        enemyResponcer = GameObject.Find("SceneManager").GetComponent<EnemyResponcer>();
        axeCollider = GameObject.Find("Axe").GetComponent<BoxCollider>();
        swordCollider = GameObject.Find("SwordPolyart").GetComponent<MeshCollider>();
        enemyUIManager = transform.Find("EnemyUICanvas").gameObject.GetComponent<EnemyUIManager>();
        gameClearText = GameObject.Find("GameClearText");
        enemyCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        hp = maxHp;
        enemyUIManager.Init(this);
        agent.destination = target.position;
        HideColliderWeapon();
    }

    void Update()
    {
        agent.destination = target.position * 0.5f;
        animator.SetFloat("Distance", agent.remainingDistance);
        LookAtTarget();
    }

    public void LookAtTarget()
    {
        transform.LookAt(target);
    }

    // ïêäÌÇÃîªíËÇóLå¯Ç…ÇµÇΩÇËÅEè¡ÇµÇΩÇËÇ∑ÇÈä÷êî
    public void HideColliderWeapon()
    {
        axeCollider.enabled = false;
    }

    public void ShowColliderWeapon()
    {
        axeCollider.enabled = true;
    }

    public void EnemyCollider()
    {
        enemyCollider.enabled = false;
    }

    public void EnemyActive()
    {
        gameObject.SetActive(false);
    }

    void Damage(int damage)
    {
        hp -= damage;
        Debug.Log("HPÅF" + hp);
        enemyUIManager.UpdateHP(hp);
        if (hp < 1)
        {
            counter++;
            hp = 0;
            animator.SetTrigger("Die");
            enemyResponcer.Responce();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            if (other == swordCollider)
            {
                animator.SetTrigger("Hurt");
                Damage(damager.damage);
            }
        }
    }
}
