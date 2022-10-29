using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;                             // �v���C���[
    private Vector3 offset;                                         // �I�t�Z�b�g

    void Start()
    {
        offset = transform.position - player.transform.position;     // �I�t�Z�b�g�̏�����
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;    // �I�t�Z�b�g��ێ������J�����ړ�
    }
}
