using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rbody;
    Animator anime;
    float axisH, axisV; //横軸、縦軸
    
    public float speed = 3.0f; //スピード
    public float angleZ = -90; //角度
    int direction = 0; //アニメの方向番号

    public static int hp = 5; //プレイヤーの体力
    bool inDamage; //ダメージ中フラグ
    bool isMobileInput; //スマホ操作中かどうか

    public float knockBack = 4.0f; //プレイヤーのノックバック


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();

        anime.SetInteger("direction",0); //デフォルトを下向きに


    }

    // Update is called once per frame
    void Update()
    {

        if (GameController.gameState != "playing" || inDamage) return;

        if (!isMobileInput) //モバイル入力がないとき
        {
            axisH = Input.GetAxisRaw("Horizontal");　//水平方向を検知
            axisV = Input.GetAxisRaw("Vertical");　//垂直方向を検知

        }

        VectorAnime(axisH,axisV); //方向アニメを決めるメソッド
    }

    private void FixedUpdate()
    {
        if (GameController.gameState != "playing") return;
        
        if(inDamage)
        {
            //点滅処理
            float value = Mathf.Sin(Time.time * 50); //valueに正負の波を作る ※Timeは経過時間
            if(value > 0) GetComponent<SpriteRenderer>().enabled = true; //正の間には絵を表示
            else GetComponent<SpriteRenderer>().enabled = false; //負の間には絵を非表示
            

            return;
        }

        rbody.velocity = new Vector2(axisH, axisV).normalized * speed;
    }

    void VectorAnime(float h,float v)
    {
        angleZ = GetAngle();

        //アニメ番号を一時的に記録
        int dir = direction;

        //if (angleZ > -135 && angleZ < -45) dir = 0; //下
        //else if (angleZ >= -45 && angleZ <= 45) dir = 3; //右
        //else if (angleZ > 45 && angleZ < 135) dir = 1; // 上
        ////else if (angleZ >= 135 && angleZ <= -135) dir = 2; //左
        //else dir = 2;

        if(Mathf.Abs(h) >= Mathf.Abs(v))
        {
            if (h > 0) dir = 3; //右
            else if (h < 0) dir = 2; //左
        }
        else
        {
            if (v > 0) dir = 1; //上
            else if (v < 0) dir = 0; //下                
        }



        //前フレームのdirectionと今あるべきアニメ番号が異なっていなければそのまま
        if (dir != direction)
        {
            direction = dir;
            anime.SetInteger("direction", direction);
        }
    }

    public float GetAngle()
    {
        Vector2 fromPos = transform.position; //現在地
        Vector2 toPos = new Vector2(fromPos.x + axisH, fromPos.y + axisV);

        float angle;

        if(axisH !=0 || axisV != 0)
        {
            //キーが押された方向と現在地の差分
            float dirX = toPos.x - fromPos.x;
            float dirY = toPos.y - fromPos.y;

            //arc tanに高さ、底辺を代入し角度を出す(ラジアン値)
            float rad = Mathf.Atan2(dirY,dirX);
            angle = rad * Mathf.Rad2Deg; //ラジアン値をオイラー値に変換
        }
        else
        {
            angle = angleZ;
        }

        return angle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetDamage(collision.gameObject);
        }
    }

    void GetDamage(GameObject enemy)
    {
        hp--;
        if(hp > 0)
        {
            //動きが止まる
            rbody.velocity = Vector2.zero;

            //ノックバック
            Vector3 v = (transform.position - enemy.transform.position).normalized * knockBack;
            rbody.AddForce(v, ForceMode2D.Impulse);

            //ダメージフラグをONにする
            inDamage = true;

            //時間差でダメージフラグをOFFに解除
            Invoke("DamageEnd", 0.25f);
        }
        else
        {
            GameOver();
        }
    }

    void DamageEnd()
    {
        inDamage = false;　//フラグ解除
        GetComponent<SpriteRenderer>().enabled = true;　//点滅が非表示で終わらないようにする
    }

    void GameOver()
    {
        GameController.gameState = "gameover";

        GetComponent<CircleCollider2D>().enabled = false; //コライダーをオフにする
        rbody.gravityScale = 1; //下へ移動
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);　//跳ね上げ
        anime.SetTrigger("death");　//死亡アニメの開始
    }

    public void PlayerDestroy()
    {
        Destroy(gameObject);
    }

    //モバイル操作
    public void MobileAxis(float x,float y)
    {
        axisH = x;
        axisV = y;
        if(axisH == 0 &&  axisV == 0)
        {
            isMobileInput = false;
        }
        else
        {
            isMobileInput = true;
        }
    }




}

