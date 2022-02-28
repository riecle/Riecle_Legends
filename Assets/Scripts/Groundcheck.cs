using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundcheck : MonoBehaviour
{

    [Header("エフェクトがついた床を判定するか")]public bool checkPlatformGround;

    private string groundTag = "Ground";//タグ判定のための変数
    private string platformTag = "GroundPlatform";
    private string moveFloorTag = "MoveFloor";
    private string fallFloorTag = "FallFloor";
    private bool isGround = false;//接地判定のための変数
    private bool isGroundEnter, isGroundStay, isGroundExit;//接地判定のための変数完全版

    //物理判定の更新毎によぶ必要がある
    /// <summary>
    /// 接地判定
    /// </summary>
    /// <returns>接地判定の正誤</returns>
    public bool IsGround()
    {
        if(isGroundEnter || isGroundStay)
        {
            isGround = true;

        }
        else if(isGroundExit)
        {
            isGround = false;
        }
        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    //接地してる時に呼ばれる
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == groundTag)
        {
            isGround = true;
            isGroundEnter = true;
            //Debug.Log("接地");
        }
        else if(checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundEnter = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundStay = true;
            //Debug.Log("継続");
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundStay = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGround = false;
            isGroundExit = true;
            //Debug.Log("出た");
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundExit = true;
        }
    }
}
