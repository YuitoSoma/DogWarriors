using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    Collider swordCollider;
    PlayerUIManager playerUIManager;
    Transform target;
    AudioSource swordSound;
    GameObject swordObject;

    public AudioClip healSound;
    public AudioClip attackSound;
    public AudioClip speedSound;
    public AudioClip audioClip;

    AudioSource healsound;
    AudioSource attacksound;
    AudioSource speedsound;
    AudioSource audioSource;

    public Text attackText;
    public Text speedText;
    public Text hpParamater;

    public float moveSpeed;
    public float currentSpeed;
    public float speed;
    public int maxHp;
    public int maxStamina;

    float x;
    float z;
    float speedup = 0;

    int hp;
    int stamina;

    bool isDie;

    void Start()
    {
        swordObject = GameObject.Find("SwordPolyart");
        swordCollider = swordObject.GetComponent<MeshCollider>();
        swordSound = swordObject.GetComponent<AudioSource>();
        playerUIManager = GameObject.Find("PlayerUICanvas").GetComponent<PlayerUIManager>();
        target = GameObject.Find("Enemy").GetComponent<Transform>();

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        healsound = GetComponent<AudioSource>();
        attacksound = GetComponent<AudioSource>();
        speedsound = GetComponent<AudioSource>();
        audioSource = CreateAudioSource();

        EnemyManager.counter = 0;
        hp = maxHp;
        stamina = maxStamina;
        speed = currentSpeed;

        playerUIManager.Init(this);
        HideColliderWeapon();
    }

    void Update()
    {
        if (isDie)
            return;
        // キーボード入力移動
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // 攻撃入力
        if (Input.GetKeyDown(KeyCode.Space))
            Attack();

        Increase();

        attackText.text = "     AT       : " + swordObject.GetComponent<Damager>().damage;
        speedText.text = "     SP       : " + speed;
    }

    void FixedUpdate()
    {
        if (isDie)
            return;
        Vector3 direction = transform.position + new Vector3(x, 0, z);
        transform.LookAt(direction);
        // 速度設定
        moveSpeed = currentSpeed + speedup;
        rb.velocity = new Vector3(x, 0, z) * moveSpeed;
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    // スタミナ計算
    void Increase()
    {
        stamina++;
        if (stamina > maxStamina)
            stamina = maxStamina;
        playerUIManager.UpdateStamina(stamina);
    }

    // 攻撃のスタミナ消費
    void Attack()
    {
        if (stamina >= 20)
        {
            stamina -= 20;
            playerUIManager.UpdateStamina(stamina);
            LookAtTarget();
            animator.SetTrigger("Attack");
        }
    }

    // 攻撃の方向転換
    void LookAtTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 2.0f)
            transform.LookAt(target);
    }

    // 武器の判定を有効にしたり・消したりする関数
    public void HideColliderWeapon()
    {
        swordSound.enabled = false;
        swordCollider.enabled = false;
    }

    public void ShowColliderWeapon()
    {
        swordSound.enabled = true;
        swordCollider.enabled = true;
    }

    // 被ダメージの計算
    void Damage(int damage)
    {
        if (isDie)
            return;
        hp -= damage;
        hpParamater.text = hp + "/100";
        if (hp <= 0)
        {
            hp = 0;
            isDie = true;
            animator.SetTrigger("Die");
            rb.velocity = Vector3.zero;
        }
        playerUIManager.UpdateHP(hp);
    }

    // 回復計算
    void Heal(int heal)
    {
        // hpとhealを足した数値がmaxHp以下の場合，回復する．
        if (hp + heal <= maxHp)
        {
            hp += heal;
            hpParamater.text = hp + "/100";
        }
        // hpが50より大きい場合はmaxHpにする
        else
        {
            hp = maxHp;
            hpParamater.text = hp + "/100";
        }
        
        playerUIManager.UpdateHP(hp);
        healsound.PlayOneShot(healSound);
    }

    // 攻撃上昇計算
    void Enhance(int attack)
    {
        swordObject.GetComponent<Damager>().damage += attack;
        attacksound.PlayOneShot(attackSound);
    }

    // 移動速度上昇計算
    void SpeedUp(float movepara)
    {
        speedup += movepara;
        speed += movepara;
        speedsound.PlayOneShot(speedSound);
    }

    // 足音生成
    AudioSource CreateAudioSource()
    {
        var audioGameObject = new GameObject();
        audioGameObject.name = "AnimationEventSEPlayer";
        audioGameObject.transform.SetParent(gameObject.transform);

        var audioSource = audioGameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;

        return audioSource;
    }

    public void Play(string eventName)
    {
        audioSource.Play();
    }

    void OnTriggerEnter(Collider other)
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
            enhancer.gameObject.SetActive(false);
        }

        Speeder speeder = other.GetComponent<Speeder>();
        if (speeder != null)
        {
            SpeedUp(speeder.speed);
            speeder.gameObject.SetActive(false);
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