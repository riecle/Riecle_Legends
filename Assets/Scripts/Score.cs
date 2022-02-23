using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    private Text scoreText = null;//スの情報を入れておく変数、.textでテキストの情報を引き出せる
    private int oldScore = 0;//古いスコア
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        if(GM.instance != null)
        {
            scoreText.text = "Score" + GM.instance.score;
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
        if(oldScore != GM.instance.score)
        {
            scoreText.text = "Score" + GM.instance.score;
            oldScore = GM.instance.score;
        }
    }
}
