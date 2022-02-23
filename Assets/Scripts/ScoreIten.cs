using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIten : MonoBehaviour
{
    [Header("加算するスコア")] public int myScore;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck PlayerCheck;

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが判定内に入った場合
        if(PlayerCheck.isOn)
        {
            if(GM.instance != null)
            GM.instance.score += myScore;
            Destroy(this.gameObject);
        }
    }
}
