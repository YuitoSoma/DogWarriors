using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    Collider swordCollider;
    PlayerUIManager playerUIManager;
    Transform target;
    AudioSource swordSound;

    public GameObject finishPanel;

    float x;
    float z;
    public float moveSpeed;
    public int maxHp;
    int hp;
    public int maxStamina;
    int stamina;
    int swordAttack;
    bool isDie;

    // Update�֐��̑O�Ɉ�x�������s�����F�ݒ�
    void Start()
    {
        GameObject swordObject = GameObject.Find("SwordPolyart");
        swordAttack = swordObject.GetComponent<Damager>().damage;
        swordCollider = swordObject.GetComponent<MeshCollider>();
        swordSound = swordObject.GetComponent<AudioSource>();
        playerUIManager = GameObject.Find("PlayerUICanvas").GetComponent<PlayerUIManager>();
        target = GameObject.Find("Enemy").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        EnemyManager.counter = 0;
        hp = maxHp;
        stamina = maxStamina;
        playerUIManager.Init(this);
        HideColliderWeapon();
    }

    // ��0.02�b�Ɉ����s�����
    void Update()
    {
        if (isDie)
            return;
        // �L�[�{�[�h���͈ړ���������
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // �U������
        if (Input.GetKeyDown(KeyCode.Space))
            Attack();

        Increase();
    }

    void Increase()
    {
        stamina++;
        if (stamina > maxStamina)
            stamina = maxStamina;
        playerUIManager.UpdateStamina(stamina);
    }

    void Attack()
    {
        if (stamina >= 20)
        {
            stamina -= 10;
            playerUIManager.UpdateStamina(stamina);
            LookAtTarget();
            animator.SetTrigger("Attack");
        }
    }

    void LookAtTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 2.0f)
            transform.LookAt(target);
    }

    private void FixedUpdate()
    {
        if (isDie)
            return;
        Vector3 direction = transform.position + new Vector3(x, 0, z);
        transform.LookAt(direction);
        // ���x�ݒ�
        rb.velocity = new Vector3(x, 0, z) * moveSpeed;
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    // ����̔����L���ɂ�����E�������肷��֐�
    public void HideColliderWeapon()
    {
        swordCollider.enabled = false;
    }

    public void ShowColliderWeapon()
    {
        swordSound.Play();
        swordCollider.enabled = true;
    }

    void Damage(int damage)
    {
        if (isDie)
            return;
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            isDie = true;
            animator.SetTrigger("Die");
            rb.velocity = Vector3.zero;
            finishPanel.SetActive(true);
        }
        playerUIManager.UpdateHP(hp);
    }

    void Heal(int heal)
    {
        // hp��heal�𑫂������l��maxHp�ȉ��̏ꍇ�C�񕜂���D
        if (hp + heal <= maxHp)
        {
            hp += heal;
        }
        // hp��50���傫���ꍇ��maxHp�ɂ���
        else
        {
            hp = maxHp;
        }
        
        playerUIManager.UpdateHP(hp);
    }

    void Enhance(int attack)
    {
        swordAttack = attack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDie)
            return;
        Healer healer = other.GetComponent<Healer>();
        if (healer != null)
        {
            Heal(healer.heal);
            healer.gameObject.SetActive(false);
        }

        Enhancer enhancer = other.GetComponent<Enhancer>();
        if (enhancer != null)
        {
            Enhance(enhancer.attack);
        }

        Damager damager = other.GetComponent<Damager>();
        if (damager != null)
        {
            // �_���[�W��^������̂ɂԂ�������
            animator.SetTrigger("Hurt");
            Damage(damager.damage);
        }
    }
}