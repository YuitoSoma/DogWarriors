using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;                             // プレイヤー
    private Vector3 offset;                                         // オフセット

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;     // オフセットの初期化
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;    // オフセットを保持したカメラ移動
    }
}
