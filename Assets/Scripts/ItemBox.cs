using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public GameObject itemPrefab; //中身となるアイテム
    public Sprite openImage; //開封された宝箱の絵
    public bool isClosed = true; //未開封かどうか

    //アイテムがPlayerのもとに届くように
    GameObject player; //プレイヤーの情報

    //接触ではなく領域に入ったかどうかのフラグ
    bool inBoxArea;

    public int arrangeId; //セーブデータ用の識別ID

    //Eキーで開ける仕様かどうか
    public bool isEKey;


    // Start is called before the first frame update
    void Start()
    {
        ExistCheck();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //キーイベントで開く・Playerが近くにいる・未開封
            if(isEKey && inBoxArea && isClosed)
            {
                BoxOpen(); //開封処理
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isEKey && isClosed && collision.gameObject.CompareTag("Player"))　//もし接触するだけで入手可能な状態だったら
        {
            BoxOpen(); //開封処理
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEKey && collision.gameObject.CompareTag("Player"))
        {
            inBoxArea = true; //箱を開けられる範囲に入っているというフラグをON
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isEKey && collision.gameObject.CompareTag("Player"))
        {
            inBoxArea = false; //箱を開けられる範囲から出たらフラグをOFF
        }
    }


    void BoxOpen()
    {

        GetComponent<SpriteRenderer>().sprite = openImage; //開封済の絵に差し替え
        isClosed = false; //未開封ではない
        if (itemPrefab != null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); //プレイヤー検索
            Instantiate(itemPrefab, player.transform.position, Quaternion.identity); //プレイヤーの位置にアイテム生成


            //SaveControllerの消費リストにまだ掲載されていなければ
            if (!SaveController.Instance.IsConsumed(this.tag, arrangeId))
            {
                //リストに追加
                SaveController.Instance.ConsumedEvent(this.tag, arrangeId);
            }

        }

    }

    void ExistCheck()
    {
        if (SaveController.Instance.IsConsumed(this.tag, arrangeId))
        {
            isClosed = false;　//フラグをオープン状態に
            GetComponent<SpriteRenderer>().sprite = openImage; //見た目を開いた箱
        }
    }


}
