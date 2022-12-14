using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("発生させるエフェクト(パーティクル)")]
    ParticleSystem particle;
    Transform target;
    NavMeshAgent agent;
    Animator animator;
    BattleSceneManager battleSceneManager;
    Collider axeCollider;
    Collider swordCollider;
    Collider enemyCollider;
    Collider shieldCollider;
    EnemyUIManager enemyUIManager;
    AudioSource axeSound;
    public GameObject[] Items;
    public AudioClip axesound;

    public static int counter;
    public int maxHp = 50;
    int hp;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        battleSceneManager = GameObject.Find("BattleSceneManager").GetComponent<BattleSceneManager>();
        axeCollider = GameObject.Find("Axe").GetComponent<BoxCollider>();
        swordCollider = GameObject.Find("SwordPolyart").GetComponent<MeshCollider>();
        shieldCollider = GameObject.Find("ShieldPolyart").GetComponent<BoxCollider>();
        enemyUIManager = transform.Find("EnemyUICanvas").gameObject.GetComponent<EnemyUIManager>();

        enemyCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        axeSound = GetComponent<AudioSource>();

        hp = maxHp;
        enemyUIManager.Init(this);
        agent.destination = target.position;
        HideColliderWeapon();
    }

    void Update()
    {
        agent.destination = target.position;
        animator.SetFloat("Distance", agent.remainingDistance);
        LookAtTarget();
    }

    public void LookAtTarget()
    {
        transform.LookAt(target);
    }

    // 武器の判定を有効にしたり・消したりする関数
    public void HideColliderWeapon()
    {
        axeCollider.enabled = false;
    }

    public void ShowColliderWeapon()
    {
        axeCollider.enabled = true;
        axeSound.PlayOneShot(axesound);
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
        enemyUIManager.UpdateHP(hp);
        if (hp < 1)
        {
            counter++;
            hp = 0;
            animator.SetTrigger("Die");
            battleSceneManager.EnemyResponce();
            Instantiate(Items[Random.Range(0, Items.Length)], new Vector3(gameObject.transform.position.x, 1.0f, gameObject.transform.position.z), Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
            if (other == swordCollider)
            {
                ParticleSystem newParticle = Instantiate(particle);                             // パーティクルシステムのインスタンスを生成する。
                newParticle.transform.position = this.transform.position;                       // パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
                newParticle.Play();
                Destroy(newParticle.gameObject, 1.0f);

                animator.SetTrigger("Hurt");
                Damage(damager.damage);
            }

        if (other == shieldCollider)
        {
            animator.SetTrigger("Hurt");
        }
    }
}
