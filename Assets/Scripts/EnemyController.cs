using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int hp = 10;
    public float speed = 1.0f; //追跡スピード
    public float searchDistance = 4.0f; //探索距離
   
    
    float axisH, axisV; //横軸、縦軸の値
    Rigidbody2D rbody;

    bool isActive; //追跡モードのフラグ

    //セーブデータの管理用
    public int arrangeId; //識別ID

    GameObject player; //プレイヤー情報

    //float testX, testY; //直接代入テスト

    // Start is called before the first frame update
    void Start()
    {
        ExistCheck();

        //Playerオブジェクトの取得
        player = GameObject.FindGameObjectWithTag("Player");
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーがいないときは何もしない
        if (player == null) return;
        if(isActive)
        {
            float dirX = player.transform.position.x - this.transform.position.x;
            //testX = player.transform.position.x - this.transform.position.x;
            float dirY = player.transform.position.y - this.transform.position.y;
            //testY = player.transform.position.y - this.transform.position.y;

            float angle = Mathf.Atan2(dirY, dirX);

            axisH = Mathf.Cos(angle); //X方向
            axisV = Mathf.Sin(angle); //Y方向

            SearchPlayer();

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
            //Updateで求めた方角に動かす
            rbody.velocity = new Vector2(axisH, axisV) * speed;
            //rbody.velocity = new Vector2(testX, testY).normalized * speed;
            Debug.Log("追跡中");
        }
    }

    void SearchPlayer()
    {
        float distance = Vector2.Distance(this.transform.position,player.transform.position);
        
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
                GetComponent<CapsuleCollider2D>().enabled = false;
                rbody.velocity = Vector2.zero;
                GetComponent<Animator>().SetTrigger("death");
            }
        }
    }

    public void EnemyDestroy()
    {
        Destroy(gameObject);
    }

    void ExistCheck()
    {
        if (SaveController.Instance.IsConsumed(this.tag, arrangeId))
        {
            Destroy(gameObject);
        }
    }
}
