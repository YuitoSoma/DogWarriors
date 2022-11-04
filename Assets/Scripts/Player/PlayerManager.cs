using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    Animator animator;                  // アニメーター変数
    Rigidbody rb;                       // リジッドボディ変数
    Collider playerCollider;            // プレイヤーのコライダ変数
    Collider swordCollider;             // 武器のコライダ変数
    PlayerUIManager playerUIManager;    // PlayerUIManager変数
    AudioSource swordSound;             // 攻撃時の音を格納した変数
    GameObject swordObject;             // プレイヤーの武器オブジェクト
    Transform target;                   // 敵の位置情報
    AudioSource effectSound;            // サウンド再生用変数
    AudioSource audioSource;            // 足音再生用変数

    public AudioClip healSound;         // 回復サウンド
    public AudioClip attackSound;       // 攻撃アップ時のサウンド
    public AudioClip speedSound;        // スピードアップ時のサウンド
    public AudioClip audioClip;         // 足音を格納する変数
    public Text attackText;             //　UIの攻撃パラメータのテキスト
    public Text speedText;              //　UIのスピードパラメータのテキスト
    public Text hpParamater;            //　HPゲージ上のUIテキスト

    public float enemyDistance;         //　enemyとの距離
    public float moveSpeed;             //　現在のスピード
    public float currentSpeed;          //　初期の移動スピード
    public float speed;                 //　現在のスピード（UI用）
    public int maxHp;                   //　最大の体力
    public int maxStamina;              //　最大のスタミナ
    public int attackStamina;           //　攻撃時の消費スタミナ
    public int defendStamina;           //　防御時の消費スタミナ

    float x;                            //　ｘ軸の移動方向
    float z;                            //　ｚ軸の移動方向
    float speedup = 0;                  //　スピード上昇率
    int hp;                             //　体力
    int stamina;                        //　スタミナ
    bool isDie;                         //　負け判定
    void Start()
    {
        // 必要なコンポーネントを自動取得
        swordObject = GameObject.Find("SwordPolyart");
        swordCollider = swordObject.GetComponent<MeshCollider>();
        swordSound = swordObject.GetComponent<AudioSource>();
        playerUIManager = GameObject.Find("PlayerUICanvas").GetComponent<PlayerUIManager>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
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

        // キーボード入力移動
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // 攻撃入力
        if (Input.GetKeyDown(KeyCode.Space))
            Attack();

        if (Input.GetKeyDown(KeyCode.Return))
            Defend();

        Increase();

        //　UIの更新
        attackText.text = "     AT       : " + swordObject.GetComponent<Damager>().damage;
        speedText.text  = "     SP       : " + speed;
    }

    void FixedUpdate()
    {
        if (isDie)
            return;

        // 移動方向
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

    // 攻撃
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

    // 防御
    void Defend()
    {
        if (stamina >= defendStamina)
        {
            stamina -= defendStamina;
            playerUIManager.UpdateStamina(stamina);
            LookAtTarget();
            animator.SetTrigger("Defend");
        }
    }
    // 方向転換
    void LookAtTarget()
    {
        //最も近かったオブジェクトを取得
        target = serchTag(gameObject, "Enemy");
        Debug.Log(target.name);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= enemyDistance)
            transform.LookAt(target);
    }

    //指定されたタグの中で最も近いものを取得
    Transform serchTag(GameObject nowObj, string tagName)
    {
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
        GameObject targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;
            }

        }
        //最も近かったオブジェクトの位置情報を返す
        return targetObj.transform;
    }

    // プレイヤ―のコライダの無効
    public void HideColliderPlayer()
    {
        playerCollider.enabled = false;
    }

    // プレイヤ―のコライダの有効
    public void ShowColliderPlayer()
    {
        playerCollider.enabled = true;
    }

    // 武器のコライダーを無効
    public void HideColliderWeapon()
    {
        swordSound.enabled = false;
        swordCollider.enabled = false;
    }

    // 武器のコライダを有効
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
        playerUIManager.UpdateHP(hp);
        if (hp <= 0)
        {
            hp = 0;
            isDie = true;
            animator.SetTrigger("Die");
            rb.velocity = Vector3.zero;
        }
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
        effectSound.PlayOneShot(healSound);
    }

    // 攻撃上昇計算
    void Enhance(int attack)
    {
        swordObject.GetComponent<Damager>().damage += attack;
        effectSound.PlayOneShot(attackSound);
    }

    // 移動速度上昇計算
    void SpeedUp(float movepara)
    {
        speedup += movepara;
        speed += movepara;
        effectSound.PlayOneShot(speedSound);
    }

    // 足音生成
    AudioSource CreateAudioSource()
    {
        var audioGameObject = new GameObject();
        audioGameObject.transform.SetParent(gameObject.transform);

        var audioSource = audioGameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;

        return audioSource;
    }

    // 足音再生
    public void Play()
    {
        audioSource.Play();
    }

    // 当たり判定（enemyの武器・各種アイテムに接触した時のイベント発生）
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