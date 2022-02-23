using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [Header("最初からフェードインが完了しているかどうか")] public bool firstFadeInComp;

    private Image img = null;//コンポーネントを取得する変数
    private int frameCount = 0;//フェードしてから何フレームたったか
    private float timer = 0.0f;//時間を計測する変数
    private bool fadeIn = false;//フェードインするかどうかのフラグ
    private bool fadeOut = false;//フェードアウトするかどうかのフラグ
    private bool compFadeIn = false;//フェードインが完了したかどうかのフラグ
    private bool compFadeOut = false;

    /// <summary>
    /// フェードインを開始するための入口のメソッド
    /// </summary>
    public void StartFadeIn()
    {

        //fadeinorfadeout中は即座に値を返して下のコードが実行されないようにする
        if(fadeIn || fadeOut)
        {
            return;
        }
        fadeIn = true;
        compFadeIn = false;
        timer = 0f;
        img.color = new Color(1, 1, 1, 1);
        img.fillAmount = 1;
        img.raycastTarget = true;
    }

    /// <summary>
    /// フェードアウトを開始するための入口のメソッド
    /// </summary>
    public void StartFadeOut()
    {

        //fadeinとfadeout中は即座に値を返して下のコードが実行されないようにする
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeOut = true;
        compFadeOut = false;
        timer = 0f;
        img.color = new Color(1, 1, 1, 0);
        img.fillAmount = 0;
        img.raycastTarget = true;
    }

    /// <summary>
    /// フェードインが完了したかどうか
    /// </summary>
    /// <returns>フェードインが完了したかどうかのフラグ</returns>
    public bool IsFadeInComplete()
    {
        return compFadeIn;
    }

    /// <summary>
    /// フェードアウトが完了したかどうか
    /// </summary>
    /// <returns>フェードアウトが完了したかどうかのフラグ</returns>
    public bool IsFadeOutComplete()
    {
        return compFadeOut;
    }

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();

        if(firstFadeInComp)
        {
            fadeInComplete();
        }
        else
        {
            StartFadeIn();
        }
    }

    // Update is called once per frame
    void Update()
    {

    
        //シーンが出てきてから2フレーム後に実行
        if(frameCount > 2)
        {
            //フェードインがtrueの場合
            if(fadeIn)
            {
                FadeInUpdate();
            }

            else if(fadeOut)
            {
                FadeOutUpdate();
            }
        }

        ++frameCount;
    }

    /// <summary>
    /// フェードインの処理
    /// </summary>
    private void FadeInUpdate()
    {
        //フェード中・フェードインしてからの時間が1秒未満の時
        if (timer < 1f)
        {
            img.color = new Color(1, 1, 1, 1 - timer);
            img.fillAmount = 1 - timer;
        }
        //フェード完了・フェードインしてからの時間が1秒たった時
        else
        {
            fadeInComplete();
        }
        timer += Time.deltaTime;
    }

    /// <summary>
    /// フェードイン完了時の処理
    /// </summary>
    private void fadeInComplete()
    {
        img.color = new Color(1, 1, 1, 0);
        img.fillAmount = 0;
        img.raycastTarget = false;
        timer = 0f;
        fadeIn = false;
        compFadeIn = true;
    }

    /// <summary>
    /// フェードアウトの処理
    /// </summary>
    private void FadeOutUpdate()
    {
        //フェード中・フェードアウトしてからの時間が1秒未満の時
        if (timer < 1f)
        {
            img.color = new Color(1, 1, 1, timer);
            img.fillAmount = timer;
        }
        //フェード完了・フェードインしてからの時間が1秒たった時
        else
        {
            fadeOutComplete();
        }
        timer += Time.deltaTime;
    }

    /// <summary>
    /// フェードアウト完了時の処理
    /// </summary>
    private void fadeOutComplete()
    {
        img.color = new Color(1, 1, 1, 1);
        img.fillAmount = 1;
        img.raycastTarget = true;
        timer = 0f;
        fadeOut = false;
        compFadeOut = true;
    }
}
