using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    //接触ではなく領域に入ったかどうかのフラグ
    bool inSavePointArea;

    //トークパネルを認識できるようにする
    GameObject messageCanvas;
    GameObject messagePanel;
    TextMeshProUGUI messageText;

    bool talking; //会話発生中かどうか

    // Start is called before the first frame update
    void Start()
    {
        //TalkCanvasを見つける
        messageCanvas = GameObject.FindGameObjectWithTag("Talk");

        //TalkCanvasの子からTalkPanel、TalkTextを見つける
        messagePanel = messageCanvas.transform.Find("TalkPanel").gameObject;
        messageText = messagePanel.transform.Find("TalkText").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //セーブポイントの近くにいるなら
            if (inSavePointArea)
            {
                SaveSystem.SaveGame(); //セーブ処理

                GameController.gameState = "talk";

                string message = "セーブされました"; //トークウィンドウに出すメッセージの作成
                messagePanel.SetActive(true); //UIパネルを表示
                messageText.text = message; //テキストの内容を反映
                talking = true; //会話が開始されている
                Time.timeScale = 0f; //ゲーム進行を止める

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
            }
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inSavePointArea = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inSavePointArea = false; 
        }
    }
}
