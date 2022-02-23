using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    [Header("これを踏んだ時にプレイヤーが跳ねる高さ")] public float boundHeight;

    [HideInInspector] public bool playerStepJdg = false;//プレイヤーがこれを踏んだかどうか
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStepJdg)
        {
            //回転しながら下に落ちていく
        }
    }
}
