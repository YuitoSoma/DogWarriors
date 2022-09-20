using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("����������G�t�F�N�g(�p�[�e�B�N��)")]
    ParticleSystem particle;
    Transform target;
    NavMeshAgent agent;
    Animator animator;
    BattleSceneManager battleSceneManager;
    Collider axeCollider;
    Collider swordCollider;
    Collider enemyCollider;
    EnemyUIManager enemyUIManager;
    AudioSource swordSound;
    public GameObject healItem;

    public static int counter;
    public int maxHp = 50;
    int hp;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        battleSceneManager = GameObject.Find("BattleSceneManager").GetComponent<BattleSceneManager>();
        axeCollider = GameObject.Find("Axe").GetComponent<BoxCollider>();
        swordCollider = GameObject.Find("SwordPolyart").GetComponent<MeshCollider>();
        enemyUIManager = transform.Find("EnemyUICanvas").gameObject.GetComponent<EnemyUIManager>();
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
        agent.destination = target.position;
        animator.SetFloat("Distance", agent.remainingDistance);
        LookAtTarget();
    }

    public void LookAtTarget()
    {
        transform.LookAt(target);
    }

    // ����̔����L���ɂ�����E�������肷��֐�
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
        enemyUIManager.UpdateHP(hp);
        if (hp < 1)
        {
            counter++;
            hp = 0;
            animator.SetTrigger("Die");
            battleSceneManager.EnemyResponce();
            Instantiate(healItem, gameObject.transform.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
            if (other == swordCollider)
            {
                ParticleSystem newParticle = Instantiate(particle);                             // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
                newParticle.transform.position = this.transform.position;                       // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���B
                newParticle.Play();
                Destroy(newParticle.gameObject, 1.0f);

                animator.SetTrigger("Hurt");
                Damage(damager.damage);
            }
    }
}
