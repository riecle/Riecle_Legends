using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動するか")] public bool nonvisible;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;
    [Header("倒して加算されるスコア")] public int score;
    //音声
    [Header("倒された音")] public AudioClip enemyDownSE;

    private Rigidbody2D rb = null;//物理演算の変数
    private SpriteRenderer sr = null;//絵の変数
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false;//右に進んでいるときにtrue
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!oc.playerStepJdg)
        {
            if (sr.isVisible || nonvisible)
            {
                if(checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF;
                }

                int xVector = -1;
                if (rightTleftF)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                rb.velocity = new Vector2(xVector * speed, -gravity);
            }
            //画面外でスリープ状態
            else
            {
                rb.Sleep();
            }
        }
        else
        {
            if(!isDead)
            {
                GM.instance.PlaySE(enemyDownSE);
                anim.Play("dead");
                rb.velocity = new Vector2(0, -gravity);
                isDead = true;
                col.enabled = false;//ボックスコライダー2Dを無効化している
                if (GM.instance.score != null)
                {
                    GM.instance.score += score;
                }
                Destroy(gameObject, 3f);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 5));
            }
        }
     }
}
