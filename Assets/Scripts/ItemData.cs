using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public enum ItemType //自作する列挙型のデータ型 
{
    light,
    bullet,
    goldkey,
    silverkey,
    life
}

public class ItemData : MonoBehaviour
{
    public ItemType type; //アイテムの種類
    public int stockCount; //入手数

    //セーブデータの時の識別番号
    public int arrangeId;

    Rigidbody2D rbody;

    public bool isTalk; //トークパネルでアナウンスするかどうか

    [TextArea]
    public string message1; //メッセージ1
    [TextArea]
    public string message2; //メッセージ2

    //トークパネルを認識できるようにする
    GameObject messageCanvas;
    GameObject messagePanel;
    TextMeshProUGUI messageText;

    bool talking; //会話発生中かどうか

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        //TalkCanvasを見つける
        messageCanvas = GameObject.FindGameObjectWithTag("Talk");

        //TalkCanvasの子からTalkPanel、TalkTextを見つける
        messagePanel = messageCanvas.transform.Find("TalkPanel").gameObject;
        messageText = messagePanel.transform.Find("TalkText").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //すでにトークウィンドウが表示されているなら、スペースキーで閉じる
            if(talking && GameController.gameState == "talk")
            {
                messagePanel.SetActive(false); //パネルを閉じる
                talking = false; //会話中フラグをOFF
                GameController.gameState = "playing"; //ステータスを元に戻す
                Time.timeScale = 1f; //時の流れを元に戻す

                ItemDestroy();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case ItemType.light:
                    LightController.getLight = true;
                    break;

                case ItemType.bullet:
                    GameController.hasBullet += stockCount;
                    break;
                
                case ItemType.goldkey:
                    GameController.hasGoldKey += stockCount;
                    break;

                case ItemType.silverkey:
                    GameController.hasSilverKey += stockCount;
                    break;

                case ItemType.life:
                    if(PlayerController.hp< 5)
                    {
                        PlayerController.hp++;
                    }
                    break;
                
                default:
                    break;

            }


        }

        if (!isTalk)
        {
            ItemDestroy();　//取ったらアイテム消滅
        }
        else
        {
            GameController.gameState = "talk";
            
            string message = message1 + stockCount + message2; //トークウィンドウに出すメッセージの作成
            messagePanel.SetActive(true); //UIパネルを表示
            messageText.text = message;　//テキストの内容を反映
            talking = true; //会話が開始されている
            Time.timeScale = 0f; //ゲーム進行を止める


        }

    }

    void ItemDestroy()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        rbody.gravityScale = 2.5f;
        rbody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
        Destroy(gameObject, 0.5f);
    }
}
