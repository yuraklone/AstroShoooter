using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    public int hp = 20;
    public float speed = 1.0f; //追跡スピード
    public float searchDistance = 10.0f; //探索距離
    public float shootDistance = 7.0f;

    float axisH, axisV; //横軸、縦軸の値
    Rigidbody2D rbody;

    bool isActive; //追跡モードのフラグ

    //セーブデータの管理用
    public int arrangeId; //識別ID

    GameObject player; //プレイヤー情報
    GameObject gate; //発射口

    public GameObject ballPrefab; //ボールのプレハブを取得
    public float shootSpeed = 5.0f; //ボールの射出速度

    //攻撃中フラグ

    bool inBossAttack = false;

    Animator anime;



    //float testX, testY; //直接代入テスト

    // Start is called before the first frame update
    void Start()
    {
        //ExistCheck();

        //Playerオブジェクトの取得
        player = GameObject.FindGameObjectWithTag("Player");
        rbody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();


        //発射口オブジェクトの取得
        Transform tr = transform.Find("BallGate"); 
        gate = tr.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーがいないときは何もしない
        if (player == null) return;
        if (isActive)
        {
            float dirX = player.transform.position.x - this.transform.position.x;
            //testX = player.transform.position.x - this.transform.position.x;
            float dirY = player.transform.position.y - this.transform.position.y;
            //testY = player.transform.position.y - this.transform.position.y;

            float angle = Mathf.Atan2(dirY, dirX);

            axisH = Mathf.Cos(angle); //X方向
            axisV = Mathf.Sin(angle); //Y方向

            SearchPlayer();

            if (inBossAttack)
            {
                ShootPlayer();
            }
            else
            {
                ShootPlayer();
            }
        }
        else
        {
            SearchPlayer();
        }

    }

    void FixedUpdate()
    {
        if (isActive == true && hp > 0)
        {
            if (!inBossAttack)
            {
                anime.SetBool("inBossAttack", false);
                //Updateで求めた方角に動かす
                rbody.velocity = new Vector2(axisH, axisV) * speed;
                //rbody.velocity = new Vector2(testX, testY).normalized * speed;
                Debug.Log("追跡中");

            }
            else
            {
                rbody.velocity = Vector2.zero;
                anime.SetBool("inBossAttack", true);
                Debug.Log("攻撃ON");
            }
        }
    }

    void SearchPlayer()
    {
        float distance = Vector2.Distance(this.transform.position, player.transform.position);

        if (distance < searchDistance)
        {
            isActive = true; //追跡モードON
            Debug.Log("追跡ON");
        }
        else
        {
            isActive = false; //追跡モードOFF
            rbody.velocity = Vector2.zero; //移動をストップ
            Debug.Log("追跡OFF");
        }
    }

    void ShootPlayer()
    {
        float distance = Vector2.Distance(this.transform.position, player.transform.position);

        if (distance < shootDistance)
        {
            inBossAttack = true;
            rbody.velocity = Vector2.zero;
            Debug.Log("攻撃ON");
        }
        else
        {
            inBossAttack = false; 

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp--;

            if (hp <= 0)
            {
                //SaveControllerの消費リストにまだ掲載されていなければ
                if (!SaveController.Instance.IsConsumed(this.tag, arrangeId))
                {
                    //リストに追加
                    SaveController.Instance.ConsumedEvent(this.tag, arrangeId);
                }


                //撃破演出
                
                hp = 0;
                GetComponent<CircleCollider2D>().enabled = false;
                rbody.velocity = Vector2.zero;
                GetComponent<Animator>().SetTrigger("bossDeath");
                inBossAttack = false;
                Debug.Log("ボス撃破");
            }
        }
    }

    void BossBallAttack()
    {
        rbody.velocity = Vector2.zero; //移動をストップ
        if (player != null)
            {
            float dX = player.transform.position.x - gate.transform.position.x;
            //testX = player.transform.position.x - this.transform.position.x;
            float dY = player.transform.position.y - gate.transform.position.y;
            //testY = player.transform.position.y - this.transform.position.y;

            float shootrad = Mathf.Atan2(dY, dX);

            float shootaim = shootrad * Mathf.Rad2Deg;
            Quaternion r = Quaternion.Euler(0, 0, shootaim);
            GameObject ball = Instantiate(ballPrefab, gate.transform.position, r);

            float aimx = Mathf.Cos(shootrad); //X方向
            float aimy = Mathf.Sin(shootrad); //Y方向
            Vector3 v = new Vector3(aimx, aimy) * shootSpeed;

            Rigidbody2D rbody = ball.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);

        }


    }




    public void EnemyDestroy()
    {
        SoundController.soundController.StopBgm();
        SoundController.soundController.SEPlay(SEType.GameClear);

        //Destroy(gameObject);

        //ステータスを切り替える
        GameController.gameState = "gameclear";

        Invoke("GameClear", 10);
    }
    //ボス撃破後タイトルに戻す
    void GameClear()
    {
        SceneManager.LoadScene("Ending");
    }


    //void ExistCheck()
    //{
    //    if (SaveController.Instance.IsConsumed(this.tag, arrangeId))
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
