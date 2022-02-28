using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("接地判定")]public Groundcheck ground;//Inspectorで設定したスクリプトを変数に入れ込んでる形
    [Header("頭の接地判定")] public Groundcheck head;//Inspectorで設定したスクリプトを変数に入れ込んでる形
    [Header("ジャンプ速度")] public float jumpSpeed;
    [Header("飛べる限界高度")]public float jumpHeight;
    [Header("ジャンプ制限時間")] public float jumpLimitTime;
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;
    [Header("走る曲線")] public AnimationCurve RunCurve;
    [Header("ジャンプ曲線")]public AnimationCurve JumpCurve;

    //音声
    [Header("ジャンプ音")] public AudioClip jumpSE;
    [Header("ダウン音")] public AudioClip downSE;
    #endregion

    #region//プライベート変数
    private Animator anim = null;//アニメーターのインスタンスを入れておく変数
    private Rigidbody2D rb = null;//物理演算の変数
    private CapsuleCollider2D capcol = null;//カプセルコライダー2Dを入れておく変数
    private SpriteRenderer sr = null;//絵を入れておく変数
    private MoveObject moveObj = null;
    private bool isGround = false;//接地判定を受け取る変数
    private bool isHead = false;//頭の接地判定を受け取る変数
    private bool isJump = false;//ジャンプ判定を受け取る変数
    private bool isOtherJump = false;//objectcollisionを踏んだ時のジャンプ判定を受け取る変数
    private bool isContinue = false;
    private bool nonDownAnim = false;
    private bool pushUpKey2 = false;
    private bool pushRightKey2 = false;
    private bool pushLeftKey2 = false;
    private float continueTime = 0.0f;//コンティニューしているかどうかのフラグ
    private float blinkTime = 0.0f;
    private bool isRun = false;//走ってるかどうか
    private bool isDown= false;//倒れたかどうか
    private float jumpPos = 0.0f;//ジャンプした場所を記録するための変数
    private float otherJumpHeight = 0.0f;//ジャンプした高さを記録する変数
    private float jumpTime = 0.0f;
    private float RunTime = 0.0f;
    private float beforeKey = 0.0f;
    private string EnemyTag = "Enemy";//Ememy判定するための変数
    private string deadAreaTag = "DeadArea";
    private string hitAreaTag = "HitArea";
    private string moveFloorTag = "MoveFloor";
    private string fallFloorTag = "FallFloor";
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        //コンポーネントのインスタンスを捕まえる
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capcol = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
       
    }

    private void Update()
    {
        if(isContinue)
        {
            //明滅　ついている時の戻る
            if(blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            //明滅　消えている時
            else if(blinkTime > 0.1f)
            {
                sr.enabled = false;
            }
            //明滅　ついている時
            else
            {
                sr.enabled = true;
            }

            //1秒たったら明滅終わり
            if(continueTime > 1.0f)
            {
                isContinue = false;
                blinkTime = 0f;
                continueTime = 0f;
                sr.enabled = true;
            }
            else
            {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDown && !GM.instance.isGameOver)
        {
            //接地判定を得る
            isGround = ground.IsGround();
            isHead = head.IsGround();

            //各種座標軸の速度を求める
            float xspeed = GetXSpeed();
            float yspeed = GetYSpeed();

            //アニメーションを適用する
            SetAnimation();

            //移動速度を設定する
            Vector2 addVelocity = Vector2.zero;
            if(moveObj != null)
            {
                addVelocity = moveObj.GetVelocity();
            }
            rb.velocity = new Vector2(xspeed, yspeed) + addVelocity;
        }
        else
        {
            rb.velocity = new Vector2(0f, -gravity);
        }
    }

    /// <summary>
    /// Y成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>上方向の速度</returns>
    private float GetYSpeed()
    {
        float verticalKey = Input.GetAxis("Vertical");//上下方向キーが入力されたかどうか
        bool pushUpKey = verticalKey > 0;//上方向キーを押しているか
        float yspeed = -gravity;//重力設定

        //地面に接地している時
        if (isOtherJump)
        {
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y;//現在の高さが飛べる高さより下か
            bool canTime = jumpLimitTime > jumpTime;//ジャンプしている時間が制限時間を超えていないか
            if (canHeight && !isHead && canTime)
            {
                yspeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isOtherJump = false;
                jumpTime = 0f;
            }
        }
        //地面にいるとき
        else if (isGround)
        {
            if (pushUpKey || pushUpKey2)
            {
                if (!isJump)
                {
                    GM.instance.PlaySE(jumpSE);
                }
                yspeed = jumpSpeed;
                jumpPos = transform.position.y;//ジャンプした位置を記録する
                isJump = true;
                jumpTime = 0f;
            }
            else
            {
                isJump = false;
                pushUpKey2 = false;
            }
        }

        //ジャンプ中かつ飛べる高さより下にいる場合
        else if (isJump)
        {
            bool canHeight = jumpPos + jumpHeight > transform.position.y;//現在の高さが飛べる高さより下か
            bool canTime = jumpLimitTime > jumpTime;//ジャンプしている時間が制限時間を超えていないか
            if (canHeight && !isHead && canTime && (pushUpKey || pushUpKey2))
            {
                yspeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                pushUpKey2 = false;
                jumpTime = 0f;
            }
        }

        //アニメーションカーブを速度に適用
        if (isJump || isOtherJump)
        {
            yspeed *= JumpCurve.Evaluate(jumpTime);
        }

        return yspeed;
    }

    /// <summary>
    /// X成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>左右方向の速度</returns>
    private float GetXSpeed()
    {
        //左右方向
        float horizontalKey = Input.GetAxis("Horizontal");
        float xspeed = 0.0f;

        //キー入力に応じて左右移動
        if (horizontalKey > 0 || pushRightKey2)
        {

            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            isRun = true;
            RunTime += Time.deltaTime;
            xspeed = speed;
        }
        else if (horizontalKey < 0 || pushLeftKey2)
        {

            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
            isRun = true;
            RunTime += Time.deltaTime;
            xspeed = -speed;
        }

        else
        {
            isRun = false;
            RunTime = 0f;
            xspeed = 0.0f;
        }

        //前回の入力からダッシュの反転を判断して速度を変える（キー入力反対で加速リセット）
        if (horizontalKey > 0 && beforeKey < 0)
        {
            RunTime = 0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            RunTime = 0f;
        }
        beforeKey = horizontalKey;

        //アニメーションカーブを速度に適用
        xspeed *= RunCurve.Evaluate(RunTime);

        return xspeed;
    }

    //ダウンアニメーションが完了しているかどうかを外から呼べるようにする
    public bool IsContinueWaiting()
    {
        if (GM.instance.isGameOver)
        {
            return false;
        }
        else
        {
            return IsDownAnimEnd() || nonDownAnim;
        }
    }

    //ダウンアニメーションが完了しているかどうか
    private bool IsDownAnimEnd()
    {
        if(isDown && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if(currentState.IsName("Down"))
            {
                if(currentState.normalizedTime >= 1)
                {
                    return true;
                }

            }
        }
        return false;
    }

    /// <summary>
    /// コンティニューする
    /// </summary>
    public void ContinuePlayer()
    {
        isDown = false;
        anim.Play("Stand");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        isContinue = true;
        nonDownAnim = false;
    }

    private void ReceiveDamage(bool downAnim)
    {
        if (isDown)
        {
            return;
        }
        else
        {
            if(downAnim)
            {
                anim.Play("Down");
            }
            else
            {
                nonDownAnim = true;
            }

            if (GM.instance.heartNum > 0)
            {
                GM.instance.PlaySE(downSE);
            }
            isDown = true;
            GM.instance.SubHeartNum();
        }
    }

    /// <summary>
    /// 敵との接触判定
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool enemy = (collision.collider.tag == EnemyTag);
        bool moveFloor = (collision.collider.tag == moveFloorTag);
        bool fallFloor = (collision.collider.tag == fallFloorTag);

        if (enemy || moveFloor || fallFloor)
        {
            //踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));

            //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach (ContactPoint2D p in collision.contacts)
            {

                if(p.point.y < judgePos)
                {
                    if (enemy || fallFloor)
                    {
                        ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                        if (o != null)
                        {
                            if (enemy)
                            {
                                otherJumpHeight = o.boundHeight;//踏んづけたものから跳ねる高さを取得する
                                o.playerStepJdg = true;//プレイヤーが踏んづけた判定にする
                                jumpPos = transform.position.y;//ジャンプした位置を記録
                                isOtherJump = true;
                                isJump = false;
                                jumpTime = 0.0f;
                            }
                            else if(fallFloor)
                            {
                                o.playerStepJdg = true;
                            }
                        }
                        else
                        {
                            Debug.Log("ObjectCollisionが付いてないよ！");
                        }
                    }
                    else if (moveFloor)
                    {
                                moveObj = collision.gameObject.GetComponent<MoveObject>();
                    }
                }
                else
                {
                    if (enemy)
                    {
                        ReceiveDamage(true);
                        break;
                    }
                }
            }
      
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == moveFloorTag)
        {
            //動く床から離れた
            moveObj = null;
        }
    }

    /// <summary>
    /// アニメーションを適用する
    /// </summary>
    private void SetAnimation()

    {
        anim.SetBool("Jump", isJump || isOtherJump);
        anim.SetBool("Ground", isGround);
        anim.SetBool("Run", isRun);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == deadAreaTag)
        {
            ReceiveDamage(false);
        }
        else if(collision.tag == hitAreaTag)
        {
            ReceiveDamage(true);
        }
    }

    public void PushUp()
    {
        pushUpKey2 = true;
    }

    public void PushDownLeft()
    {
        pushLeftKey2 = true;
    }

    public void PushUpLeft()
    {
        pushLeftKey2 = false;
    }

    public void PushDownRight()
    {
        pushRightKey2 = true;
    }

    public void PushUpRight()
    {
        pushRightKey2 = false;
    }
}
