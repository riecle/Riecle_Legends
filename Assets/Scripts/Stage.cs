using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    private Text stageNumText = null;//ステージの情報を入れておく変数、.textでテキストの情報を引き出せる
    private int oldstageNum = 0;//古いステージ
    // Start is called before the first frame update
    void Start()
    {
        stageNumText = GetComponent<Text>();
        if (GM.instance != null)
        {
            stageNumText.text = "Stage" + GM.instance.stageNum;
        }
        //GMがない場合動かないので警告する
        else
        {
            Debug.Log("ゲームマネージャー置き忘れてるよ！");
            Destroy(this);//自身を消す
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldstageNum != GM.instance.stageNum)
        {
            stageNumText.text = "Stage" + GM.instance.stageNum;
            oldstageNum = GM.instance.stageNum;
        }
    }
}
