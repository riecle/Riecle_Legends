using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class title : MonoBehaviour
{
    [Header("フェード")] public Fade fade;
    [Header("スタートボイス")] public AudioClip StartVoice;
    [Header("スタート音")] public AudioClip StartSE;
    [Header("プッシュしてから何フレーム後に再生されるか")] public int flameCountOver;
    [Header("PCのボタン")] public GameObject PCButton;
    [Header("スマホのボタン")] public GameObject PhoneButton;

    private bool firstPush = false;//ボタンが押されたらtrue
    private bool goNextScene = false;//次のシーンへ行くかどうかのフラグ
    private bool pushOn = false;
    private int frameCount = 0;//何フレームたったか

    //スタートボタンが押されたら呼ばれる(スマホ版)
    public void PushStartPhone()
    {
        //ボタンが押されてない時
        if (!firstPush)
        {
            Debug.Log("次のシーンへ");
            //ここに次のシーンへ行く命令を書く
            GM.instance.smartmode = true;
            GM.instance.PlaySE(StartSE);
            GM.instance.PlaySE(StartVoice);
            pushOn = true;
            firstPush = true;
            PCButton.SetActive(false);
        }
    }

    //スタートボタンが押されたら呼ばれる(PC版)
    public void PushStart()
    {
        //ボタンが押されてない時
        if (!firstPush)
        {
            Debug.Log("次のシーンへ");
            //ここに次のシーンへ行く命令を書く
            GM.instance.PlaySE(StartSE);
            GM.instance.PlaySE(StartVoice);
            pushOn = true;
            firstPush = true;
            PhoneButton.SetActive(false);
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

        if(pushOn)
        {
            if (frameCount > flameCountOver)
            {
                fade.StartFadeOut();
            }
            ++frameCount;
        }
    }
}

