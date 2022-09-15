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

    float x;
    float z;
    public float moveSpeed;
    public int maxHp = 10;
    int hp;
    bool isDie;

    // Update関数の前に一度だけ実行される：設定
    void Start()
    {
        hp = maxHp;
        playerUIManager.Init(this);

        rb = GetComponent < Rigidbody>();
        animator = GetComponent<Animator>();
        HideColliderWeapon();
    }

    // 約0.02秒に一回実行される
    void Update()
    {
        if (isDie)
        {
            return;
        }
        // キーボード入力移動させたい
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // 攻撃入力
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
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

        // 速度設定
        rb.velocity = new Vector3(x, 0, z) * moveSpeed;
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    // 武器の判定を有効にしたり・消したりする関数
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
        }
        Debug.Log("残りHP:" + hp);

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
            // ダメージを与えるものにぶつかったら
            animator.SetTrigger("Hurt");
            Damage(damager.damage);
        }
    }
}