using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrl : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;//プレイヤーのゲームオブジェクトを入れておく
    [Header("コンティニュー位置")] public GameObject[] continuePoint;
    [Header("ゲームオーバー")]public GameObject gameOverObj;
    [Header("フェード")]public Fade fade;

    //音声
    [Header("リトライ音")] public AudioClip RetrySE;
    [Header("ゲームオーバー音")] public AudioClip GameOverSE;
    [Header("止めたいBGM")]public AudioSource audioSource;

    private player p;
    private int nextStageNum;
    private bool startFade = false;
    private bool doGameOver = false;
    private bool retryGame = false;
    private bool doSceneChange = false;

    // Start is called before the first frame update
    void Start()
    {

        if (playerObj != null && continuePoint != null && continuePoint.Length > 0 && gameOverObj != null && fade != null)
        {
            gameOverObj.SetActive(false);
            //設定したプレイヤーの位置を設定したcontinuepoint[0]の位置へ移動させる
            playerObj.transform.position = continuePoint[0].transform.position;

            p = playerObj.GetComponent<player>();
            if(p == null)
            {
                Debug.Log("プレイヤーじゃない物がアタッチされているよ");
            }
        }
        else
        {
            Debug.Log("設定が足りてないよ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバー時の処理
        if (GM.instance.isGameOver && !doGameOver)
        {
            audioSource.Stop();
            GM.instance.PlaySE(GameOverSE);
            gameOverObj.SetActive(true);
            doGameOver = true;
        }

        //プレイヤーがやられた時の処理
         else if (p != null && p.IsContinueWaiting() && !doGameOver)
         {
                //コンティニューしたい位置の目印の設定が足りているか
                if (continuePoint.Length > GM.instance.continueNum)
                {
                    playerObj.transform.position = continuePoint[GM.instance.continueNum].transform.position;
                    p.ContinuePlayer();
                }
                else
                {
                    Debug.Log("コンティニューポイントの設定が足りてないよ！");
                }
          }
            //ステージを切り替える
            if(fade != null && startFade && !doSceneChange)
            {
                if(fade.IsFadeInComplete())
                {
                    //ゲームリトライ
                    if(retryGame)
                    {
                        GM.instance.RetryGame();
                    }
                    //次のステージ
                    else
                    {
                        GM.instance.stageNum = nextStageNum;
                    }
                    SceneManager.LoadScene("stage" + nextStageNum);
                    doSceneChange = true;
                }
           }
    }

    /// <summary>
    /// 最初から始める
    /// </summary>
    public void Retry()
    {
        GM.instance.PlaySE(RetrySE);
        ChangeScene(1);
        retryGame = true;
    }

    /// <summary>
    /// ステージを切り替えます
    /// </summary>
    /// <param name="num">ステージ番号</param>
    public void ChangeScene(int num)
    {
        if(fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
    }
}
