using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heart : MonoBehaviour
{

    private Text heartNumText = null;//残機の情報を入れておく変数、.textでテキストの情報を引き出せる
    private int oldheartNum = 0;//古い残機
    // Start is called before the first frame update
    void Start()
    {
        heartNumText = GetComponent<Text>();
        if (GM.instance != null)
        {
            heartNumText.text = "×" + GM.instance.heartNum;
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
        if (oldheartNum != GM.instance.heartNum)
        {
            heartNumText.text = "×" + GM.instance.heartNum;
            oldheartNum = GM.instance.heartNum;
        }
    }
}
