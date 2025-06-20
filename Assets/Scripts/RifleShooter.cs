using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : MonoBehaviour
{
    public GameObject riflePrefab;　//生成されるべきライフル
    GameObject rifleObj; //生成されたライフルオブジェクトの情報
    
    PlayerController playerCnt;
    
    public GameObject bulletPrefab; //生成されるべき弾丸
    public float shootSpeed = 12.0f; //弾丸の速度
    public float shootDelay = 0.25f; //発射インターバル時間
    bool inAttack; //攻撃中かどうかのフラグ

    Transform gate; //ライフルの子オブジェクトの発射口


    // Start is called before the first frame update
    void Start()
    {
        Vector3 playerPos =transform.position; //Playerの位置取得
        //ライフルを生成しつつrifleObjに情報を格納
        rifleObj = Instantiate(riflePrefab,playerPos,Quaternion.identity);
        rifleObj.transform.SetParent(transform); //生成したライフルはPlayerの子オブジェクトになる

        gate = rifleObj.transform.Find("Gate"); //ライフルの子オブジェクトのGateのTransform情報を取得
    
        playerCnt = GetComponent<PlayerController>(); //PlayerControllerを取得
    }

    // Update is called once per frame
    void Update()
    {
        //ライフルの回転とZ軸の優先順位
        float rifleZ = -1; //基本は手前に映るように値をセッティング
        PlayerController playerCnt = GetComponent<PlayerController>();

        if(playerCnt.angleZ >45 && playerCnt.angleZ < 135) //ライフルが上向きの時
        {
            rifleZ = 1; //ライフルが奥に写るようにセッティング
        }
        //絵が90度ずれるためangleZ取得+90
        rifleObj.transform.rotation = Quaternion.Euler(0, 0, (playerCnt.angleZ + 90)); 
        
        //ライフルのZ座標(手前、奥)の位置を調整
        rifleObj.transform.position = new Vector3(transform.position.x, transform.position.y, rifleZ);


        if (Input.GetKeyDown(KeyCode.R)) Attack(); //キーが押されたら弾丸発射

    
    }
    
    void Attack()
    {
        //すでに攻撃中であれば何もしない
        if (inAttack || GameController.hasBullet <=0) return;

        GameController.hasBullet--;
        inAttack = true; //攻撃フラグをON

        float angleZ = playerCnt.angleZ; //Playerの角度を取得
                                             //これから生成する弾丸の角度をセッティング
        Quaternion bulletRotate = Quaternion.Euler(0, 0, angleZ);

        //生成してbulletObjに情報を格納
        GameObject bulletrObj = Instantiate(bulletPrefab, gate.position, bulletRotate);

        //angleZと三角関数を使ってVactor3のx成分とy成分を作っていく
        float x = Mathf.Cos(angleZ * Mathf.Deg2Rad); //角度(ラジアン値)からx成分を算出
        float y = Mathf.Sin(angleZ * Mathf.Deg2Rad); //角度(ラジアン値)からy成分を算出

        //向かうべき単位ベクトルと弾速をかけたベクトルで打ち出す
        Vector3 v = new Vector3(x, y).normalized * shootSpeed; //Zは省略→0を入れる

       　//生成した弾丸のRigidbody2Dを取得
        Rigidbody2D bulletRigid = bulletrObj.GetComponent<Rigidbody2D>();
        bulletRigid.AddForce(v, ForceMode2D.Impulse);

        //攻撃中フラグを時間差でOFF
        Invoke("StopAttack",shootDelay);

        //攻撃のSEを再生
        SoundController.soundController.SEPlay(SEType.Shoot);

    }

    void StopAttack()
    {
        inAttack = false;
    }
}
