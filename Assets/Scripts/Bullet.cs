using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float deleteTime = 1.0f; //一定時間で削除

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Wall"))
        {
            transform.SetParent(collision.transform); //当たった相手を親オブジェクトに
            GetComponent<BoxCollider2D>().enabled = false; //あたり判定をOFF
            GetComponent<Rigidbody2D>().simulated = false; //物理処理を無効
        }

        Destroy(gameObject, 0.1f);


    }


}
