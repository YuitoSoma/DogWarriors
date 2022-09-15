using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    public Collider weaponCollider;
    public PlayerUIManager playerUIManager;
    public GameObject gameOverText;
    public Transform target;

    float x;
    float z;
    public float moveSpeed;
    public int maxHp;
    int hp;
    public int maxStamina;
    int stamina;

    bool isDie;

    // Update�֐��̑O�Ɉ�x�������s�����F�ݒ�
    void Start()
    {
        hp = maxHp;
        stamina = maxStamina;
        playerUIManager.Init(this);

        rb = GetComponent < Rigidbody>();
        animator = GetComponent<Animator>();
        HideColliderWeapon();
    }

    // ��0.02�b�Ɉ����s�����
    void Update()
    {
        if (isDie)
        {
            return;
        }
        // �L�[�{�[�h���͈ړ���������
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // �U������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        Increase();
    }

    void Increase()
    {
        stamina++;

        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }

        playerUIManager.UpdateStamina(stamina);
    }

    void Attack()
    {
        if (stamina >= 40)
        {
            stamina -= 40;
            playerUIManager.UpdateStamina(stamina);
            LookAtTarget();
            animator.SetTrigger("Attack");
        }
    }

    void LookAtTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 2.0f)
        {
            transform.LookAt(target);
        }
    }

    private void FixedUpdate()
    {
        if (isDie)
        {
            return;
        }

        Vector3 direction = transform.position + new Vector3(x, 0, z);
        transform.LookAt(direction);

        // ���x�ݒ�
        rb.velocity = new Vector3(x, 0, z) * moveSpeed;
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    // ����̔����L���ɂ�����E�������肷��֐�
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
        if (isDie)
        {
            return;
        }

        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            isDie = true;
            animator.SetTrigger("Die");
            gameOverText.SetActive(true);
            rb.velocity = Vector3.zero;
        }

        playerUIManager.UpdateHP(hp);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDie)
        {
            return;
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