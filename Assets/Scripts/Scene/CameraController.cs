using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;                             // プレイヤー
    private Vector3 offset;                                         // オフセット

    void Start()
    {
        offset = transform.position - player.transform.position;     // オフセットの初期化
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;    // オフセットを保持したカメラ移動
    }
}
