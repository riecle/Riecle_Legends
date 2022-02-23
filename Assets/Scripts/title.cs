using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class title : MonoBehaviour
{
    [Header("フェード")] public Fade fade;

    private bool firstPush = false;//ボタンが押されたらtrue
    private bool goNextScene = false;//次のシーンへ行くかどうかのフラグ

    //スタートボタンが押されたら呼ばれる
    public void PushStart()
    {
        //ボタンが押されてない時
        if (!firstPush)
        {
            Debug.Log("次のシーンへ");
            //ここに次のシーンへ行く命令を書く
            fade.StartFadeOut();            
            firstPush = true;
        }
    }

    private void Update()
    {
        //フェードアウトが完了してからシーンを移す&一回ここを通ったらもう呼ばれない
        if(!goNextScene && fade.IsFadeOutComplete())
        {
            SceneManager.LoadScene("stage1");
            goNextScene = true;
        }
    }
}

