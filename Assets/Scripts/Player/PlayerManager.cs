using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    Animator animator;                  // �A�j���[�^�[�ϐ�
    Rigidbody rb;                       // ���W�b�h�{�f�B�ϐ�
    Collider swordCollider;             // ����̃R���C�_�ϐ�
    PlayerUIManager playerUIManager;    // PlayerUIManager�ϐ�
    Transform target;                   // enemy�̈ʒu���
    AudioSource swordSound;             // �U�����̉����i�[�����ϐ�
    GameObject swordObject;             // �v���C���[�̕���I�u�W�F�N�g

    public AudioClip healSound;         // �񕜃T�E���h
    public AudioClip attackSound;       // �U���A�b�v���̃T�E���h
    public AudioClip speedSound;        // �X�s�[�h�A�b�v���̃T�E���h
    public AudioClip audioClip;         // �������i�[����ϐ�

    AudioSource effectSound;            // �T�E���h�Đ��p�ϐ�

    AudioSource audioSource;            // �����Đ��p�ϐ�

    public Text attackText;             //�@UI�̍U���p�����[�^�̃e�L�X�g
    public Text speedText;              //�@UI�̃X�s�[�h�p�����[�^�̃e�L�X�g
    public Text hpParamater;            //�@HP�Q�[�W���UI�e�L�X�g
    public float enemyDistance;         //�@enemy�Ƃ̋���
    public float moveSpeed;             //�@���݂̃X�s�[�h
    public float currentSpeed;          //�@�����̈ړ��X�s�[�h
    public float speed;                 //�@���݂̃X�s�[�h�iUI�p�j
    public int maxHp;                   //�@�ő�̗̑�
    public int maxStamina;              //�@�ő�̃X�^�~�i
    public int attackStamina;           //�@�U�����̏���X�^�~�i

    float x;                            //�@�����̈ړ�����
    float z;                            //�@�����̈ړ�����
    float speedup = 0;                  //�@�X�s�[�h�㏸��
    int hp;                             //�@�̗�
    int stamina;                        //�@�X�^�~�i
    bool isDie;                         //�@��������
    void Start()
    {
        //�@�K�v�ȃR���|�[�l���g�������擾
        swordObject = GameObject.Find("SwordPolyart");
        swordCollider = swordObject.GetComponent<MeshCollider>();
        swordSound = swordObject.GetComponent<AudioSource>();
        playerUIManager = GameObject.Find("PlayerUICanvas").GetComponent<PlayerUIManager>();
        target = GameObject.Find("Enemy").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        effectSound = GetComponent<AudioSource>();
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

        // �L�[�{�[�h���͈ړ�
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // �U������
        if (Input.GetKeyDown(KeyCode.Space))
            Attack();

        Increase();

        //�@UI�̍X�V
        attackText.text = "     AT       : " + swordObject.GetComponent<Damager>().damage;
        speedText.text = "     SP       : " + speed;
    }

    void FixedUpdate()
    {
        if (isDie)
            return;

        // �ړ�����
        Vector3 direction = transform.position + new Vector3(x, 0, z);
        transform.LookAt(direction);
        // ���x�ݒ�
        moveSpeed = currentSpeed + speedup;
        rb.velocity = new Vector3(x, 0, z) * moveSpeed;
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    // �X�^�~�i�v�Z
    void Increase()
    {
        stamina++;
        if (stamina > maxStamina)
            stamina = maxStamina;
        playerUIManager.UpdateStamina(stamina);
    }

    // �U���̃X�^�~�i����
    void Attack()
    {
        if (stamina >= attackStamina)
        {
            stamina -= attackStamina;
            playerUIManager.UpdateStamina(stamina);
            LookAtTarget();
            animator.SetTrigger("Attack");
        }
    }

    // �U���̕����]��
    void LookAtTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= enemyDistance)
            transform.LookAt(target);
    }

    // ����̃R���C�_�[�𖳌�
    public void HideColliderWeapon()
    {
        swordSound.enabled = false;
        swordCollider.enabled = false;
    }

    // ����̃R���C�_��L��
    public void ShowColliderWeapon()
    {
        swordSound.enabled = true;
        swordCollider.enabled = true;
    }

    // ��_���[�W�̌v�Z
    void Damage(int damage)
    {
        if (isDie)
            return;

        hp -= damage;
        hpParamater.text = hp + "/100";
        playerUIManager.UpdateHP(hp);
        if (hp <= 0)
        {
            hp = 0;
            isDie = true;
            animator.SetTrigger("Die");
            rb.velocity = Vector3.zero;
        }
    }

    // �񕜌v�Z
    void Heal(int heal)
    {
        // hp��heal�𑫂������l��maxHp�ȉ��̏ꍇ�C�񕜂���D
        if (hp + heal <= maxHp)
        {
            hp += heal;
            hpParamater.text = hp + "/100";
        }
        // hp��50���傫���ꍇ��maxHp�ɂ���
        else
        {
            hp = maxHp;
            hpParamater.text = hp + "/100";
        }
        
        playerUIManager.UpdateHP(hp);
        effectSound.PlayOneShot(healSound);
    }

    // �U���㏸�v�Z
    void Enhance(int attack)
    {
        swordObject.GetComponent<Damager>().damage += attack;
        effectSound.PlayOneShot(attackSound);
    }

    // �ړ����x�㏸�v�Z
    void SpeedUp(float movepara)
    {
        speedup += movepara;
        speed += movepara;
        effectSound.PlayOneShot(speedSound);
    }

    // ��������
    AudioSource CreateAudioSource()
    {
        var audioGameObject = new GameObject();
        audioGameObject.transform.SetParent(gameObject.transform);

        var audioSource = audioGameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;

        return audioSource;
    }

    // �����Đ�
    public void Play(string eventName)
    {
        audioSource.Play();
    }

    // �����蔻��ienemy�̕���E�e��A�C�e���ɐڐG�������̃C�x���g�����j
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
            // �_���[�W��^������̂ɂԂ�������
            animator.SetTrigger("Hurt");
            Damage(damager.damage);
        }
    }
}