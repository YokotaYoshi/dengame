using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;//移動速度
    public bool isToRight = false;//trueなら右向きfalseなら左向き
    public float revTime = 0;//反転時間
    public LayerMask groundLayer;//地面レイヤー

    float time=0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isToRight)
        {
            transform.localScale = new Vector2(-1,1);//向きの変更
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (revTime > 0)
        {
            time += Time.deltaTime;
            if (time>= revTime)
            {
                isToRight = !isToRight;//フラグを反転
                time = 0;//タイマー初期化
                if (isToRight)
                {
                    transform.localScale = new Vector2(-1,1);//向きの変更
                }
                else
                {
                    transform.localScale = new Vector2(1,1);//向きの変更
                }
            }
        }
    }

    void FixedUpdate()
    {
        //地上判定
        bool onGround = Physics2D.CircleCast(transform.position,//発射位置
        0.5f,//円の半径
        Vector2.down,//発射方向
        0.5f,//発射距離
        groundLayer);//検出するレイヤー

        if (onGround)
        {
            //速度を更新する
            //Rigidbody2Dを取得する
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if(isToRight)
            {
                Debug.Log("右");
                rbody.linearVelocity = new Vector2(speed, rbody.linearVelocity.y);
            }
            else
            {
                rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocity.y);
            }
        }
    }

    //接触
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isToRight = !isToRight;//フラグを反転
        time = 0;//タイマーを初期化
        if (isToRight)
        {
            transform.localScale = new Vector2(-1,1);//向きの変更
        }
        else
        {
            transform.localScale = new Vector2(1,1);//向きの変更
        }
    }
}
