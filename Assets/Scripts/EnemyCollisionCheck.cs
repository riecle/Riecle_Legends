using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionCheck : MonoBehaviour
{
    [HideInInspector] public bool isOn = false;//接地判定のための変数

    private string GroundTag = "Ground";//タグ判定のための変数
    private string EnemyTag = "Enemy";//タグ判定のための変数

    //接地してる時に呼ばれる
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GroundTag || collision.tag == EnemyTag)
        {
            isOn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GroundTag || collision.tag == EnemyTag)
        {
            isOn = false;
        }
    }
}
