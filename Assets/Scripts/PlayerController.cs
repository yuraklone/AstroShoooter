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
    public float angleZ; //角度
    int direction = 0; //アニメの方向番号

    public static int hp = 5; //プレイヤーの体力
    bool inDamage; //ダメージ中フラグ
    bool isMobileInput; //スマホ操作中かどうか


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
        if (!isMobileInput) //モバイル入力がないとき
        {
            axisH = Input.GetAxisRaw("Horizontal");　//水平方向を検知
            axisV = Input.GetAxisRaw("Vertical");　//垂直方向を検知

        }

        VectorAnime(axisH,axisV); //方向アニメを決めるメソッド
    }

        private void FixedUpdate()
    {
        rbody.velocity = new Vector2(axisH, axisV).normalized * speed;
    }

    void VectorAnime(float h,float v)
    {
        angleZ = GetAngle();

        //アニメ番号を一時的に記録
        int dir;

        if(angleZ > -135 && angleZ < -45) dir = 0; //下
        else if(angleZ >= -45 && angleZ <= 45) dir = 3; //右
        else if(angleZ > 45 && angleZ < 135) dir = 1; // 上
        else if(angleZ >= 135 && angleZ <= -135) dir = 2; //左

        //前フレームのdirectionと今あるべきアニメ番号が異なっていなければそのまま
        if(dir != direction)
        {
            direction = dir;
            anime.SetInteger("direction",direction);
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




}

