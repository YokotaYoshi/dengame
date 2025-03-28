using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseController : MonoBehaviour
{
    GameObject player;//プレイヤー
    public float speed = 3.0f;//追跡速度
    bool isMoving = false;//動いているかどうか
    Rigidbody2D rb2d;//Rigidbody2D;
    CircleCollider2D enemyCollider;//CircleCollider2D;
    
    float moveTime;//移動にかかる時間
    Vector2 playerDirection;//自分から見たプレイヤーの位置
    float playerDirectionDegree;//自分から見たプレイヤーの角度
    Vector2 moveDirection;//実際に動く方向

    //-------------何かに衝突した時に使う--------------
    public float waitTime = 1.5f;//一時停止時間
    bool breakCoroutine = false;//コルーチン脱出フラグ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        rb2d = GetComponent<Rigidbody2D>();//Rigidbody2Dを取得
        enemyCollider = GetComponent<CircleCollider2D>();//CircleCollider2Dを取得
        //1マスの移動にかかる時間を計算
        moveTime = 1.0f / speed;
    }

    // Update is called once per frame
    void Update()
    {

        //自分から見たプレイヤーの位置
        Vector2 playerDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

        //自分から見たプレイヤーの角度
        float playerDirectionDegree = Mathf.Atan2(playerDirection.y, playerDirection.x)*Mathf.Rad2Deg;
        //Debug.Log(playerDirectionDegree);

        //実際に動く方向を決定
        if (playerDirectionDegree >= -45 && playerDirectionDegree < 45)
        {
            //プレイヤーが右のほうにいる
            //右に移動
            moveDirection = new Vector2(1.0f, 0f);
        }
        else if (playerDirectionDegree >= 45 && playerDirectionDegree < 135)
        {
            //プレイヤーが上のほうにいる
            //上に移動
            moveDirection = new Vector2(0f, 1.0f);
        }
        else if (playerDirectionDegree >= -135 && playerDirectionDegree < -45)
        {
            //プレイヤーが下のほうにいる
            //下に移動
            moveDirection = new Vector2(0f, -1.0f);
        }
        else
        {
            //プレイヤーが左のほうにいる
            //左に移動
            moveDirection = new Vector2(-1.0f, 0f);
        }

        Debug.Log(breakCoroutine);
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            //コルーチンが終了したら、コルーチンをスタートさせる
            if (!isMoving)
            {
                StartCoroutine(Move(moveDirection));
            }
        }
    }

    //つっかえたときとか
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //プレイヤーに衝突したら
            breakCoroutine = true;
            rb2d.linearVelocity = Vector2.zero;//停止
            enemyCollider.enabled = false;//当たり判定を消去
            StartCoroutine(Restart());//再度追いかける。当たり判定を復活する
        }
        else
        {
            //別のなにかに衝突したら
            breakCoroutine = true;
            rb2d.linearVelocity = Vector2.zero;//停止
            //近くの格子点に移動
            transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
            StartCoroutine(Restart());//再度追いかける。当たり判定を復活する
        }
    }

    IEnumerator Move(Vector2 moveDirection)
    {
        //格子点にいたる毎にプレイヤーの位置を参照し、今の格子点の隣であり
        //かつプレイヤーに最も近くの格子点をゴールとして移動する

        //動いているフラグ立て
        isMoving = true;

        //時間計測
        float time = 0.0f;

        //移動方向に合わせて速度ベクトルを設定
        if (moveDirection.x > 0.5f)
        {
            rb2d.linearVelocity = new Vector2(speed, 0.0f);
        }
        else if (moveDirection.x < -0.5f)
        {
            rb2d.linearVelocity = new Vector2(-speed, 0.0f);
        }
        else if (moveDirection.y > 0.5f)
        {
            rb2d.linearVelocity = new Vector2(0.0f, speed);
        }
        else if (moveDirection.y < -0.5f)
        {
            rb2d.linearVelocity = new Vector2(0.0f, -speed);
        }

        
        
        while(true)
        {
            time += Time.deltaTime;//時間計測

            if (time >= moveTime)
            {
                break;//時間的に移動し終わったら解除
            }
            if (breakCoroutine)
            {
                Debug.Log("停止");
                yield break;//コルーチン停止命令が出たら停止
            }

            yield return null;
        }

        //Debug.Log(time);
        //動いているフラグおろし
        isMoving = false;
        //速度をゼロに
        rb2d.linearVelocity = Vector2.zero;
        //座標を格子点に
        transform.position = new Vector2(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y));
    }

    IEnumerator Restart()
    {
        isMoving = true;//動いていることにしてMove()の起動阻止
        
        //数フレーム待機した後、当たり判定を復活させ追跡を再開する。
        yield return new WaitForSeconds(waitTime);

        enemyCollider.enabled = true;
        breakCoroutine = false;//コルーチン脱出フラグoff
        isMoving = false;
    }
}
