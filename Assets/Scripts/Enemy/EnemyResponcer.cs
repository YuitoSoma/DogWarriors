using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResponcer : MonoBehaviour
{
    public GameObject enemy;
    public void Responce()
    {
        for (int i = 1; i <= 2;i++)
        {
            Instantiate(enemy, new Vector3(Random.Range(-45.0f,45.0f), 0.0f, Random.Range(-45.0f, 45.0f)), Quaternion.identity);
        }
    }
}
