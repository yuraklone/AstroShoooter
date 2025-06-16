using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isGoldDoor; //金の扉にするかどうか
    public Sprite goldDoorImage; //金の扉の絵

    public bool isEkey = true; //Eキーを使用するかどうか
    bool inDoorArea;
    bool isDelete;

    //トークパネルを認識できるようにする
    GameObject messageCanvas;
    GameObject messagePanel;
    TextMeshProUGUI messageText;

    bool talking; //会話発生中かどうか

    public int arrangeId; //セーブデータ用の識別ID

    // Start is called before the first frame update
    void Start()
    {
        if (isGoldDoor)
        {
            GetComponent<SpriteRenderer>().sprite = goldDoorImage;
        }

        //TalkCanvasを見つける
        messageCanvas = GameObject.FindGameObjectWithTag("Talk");

        //TalkCanvasの子からTalkPanel、TalkTextを見つける
        messagePanel = messageCanvas.transform.Find("TalkPanel").gameObject;
        messageText = messagePanel.transform.Find("TalkText").gameObject.GetComponent<TextMeshProUGUI>();



    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //キーイベントのフラグがONでDoorの近くにいる場合
            if(isEkey && inDoorArea)
            {
                if (isGoldDoor && GameController.hasGoldKey > 0)
                {
                    GameController.hasGoldKey--;
                    isDelete = true; //削除フラグを立てる
                    messagePanel.SetActive(true);
                    messageText.text = "金色のカギ　を　つかった!";
                    talking = true; //会話モードON
                    GameController.gameState = "talk";
                    Time.timeScale = 0; //ゲーム進行を止める
                }
                else if (!isGoldDoor && GameController.hasSilverKey > 0)
                {
                    GameController.hasSilverKey--;
                    isDelete = true; //削除フラグを立てる
                    messagePanel.SetActive(true);
                    messageText.text = "銀色のカギ　を　つかった!";
                    talking = true; //会話モードON
                    GameController.gameState = "talk";
                    Time.timeScale = 0; //ゲーム進行を止める
                }
                //カギがないとき
                else
                {
                    messagePanel.SetActive(true);
                    messageText.text = "カギ　が　ひつようなようだ…";
                    talking = true; //会話モードON
                    GameController.gameState = "talk";
                    Time.timeScale = 0; //ゲーム進行を止める
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //すでにトークウィンドウが表示されているなら、スペースキーで閉じる
            if (talking && GameController.gameState == "talk")
            {
                messagePanel.SetActive(false); //パネルを閉じる
                talking = false; //会話中フラグをOFF
                GameController.gameState = "playing"; //ステータスを元に戻す
                Time.timeScale = 1f; //時の流れを元に戻す

                if (isDelete)
                {
                    Destroy(gameObject); //カギを開けた場合のみ削除　
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isEkey && collision.gameObject.CompareTag("Player"))
        {
            if (!isGoldDoor && GameController.hasSilverKey > 0)
            {
                GameController.hasSilverKey--;
                Destroy(gameObject);
            }

            if (isGoldDoor && GameController.hasGoldKey > 0)
            {
                GameController.hasGoldKey--;
                Destroy(gameObject);
            }



        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //キーイベントあり、相手がプレイヤーだったら
        if(isEkey && collision.gameObject.CompareTag("Player"))
        {
            inDoorArea = true; //ドアの領域内に侵入
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //キーイベントあり、相手がプレイヤーだったら
        if (isEkey && collision.gameObject.CompareTag("Player"))
        {
            inDoorArea = false; //ドアの領域内に侵入
        }
    }

}
