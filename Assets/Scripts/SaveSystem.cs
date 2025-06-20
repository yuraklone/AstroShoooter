using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//シリアライズ可とする→JSON化ができるようになる
[System.Serializable]
public class SaveData
{
    public int arrrangeId; //識別ID
    public string tag; //タグ名

    //コンストラクタ
    public SaveData(string tag, int arrangeId)
    {
        this.tag = tag;
        this.arrrangeId = arrangeId;
    }
}

//JSON化するためにリストをラッピングするクラス
[System.Serializable]
class Wrapper
{
    //SaveDataクラスをまとめたリストを扱えるクラス
    public List<SaveData> items;
}


public class SaveSystem
{
    public static void SaveGame()
    {
        //Player情報の獲得
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //複製先となるリスト情報を宣言
        List<SaveData> dataList = new List<SaveData>();

        //普段運用しているConsumeEventリストをdataListに変換
        foreach(var obj in SaveController.Instance.consumedEvent)
        {
            dataList.Add(new SaveData(obj.tag, obj.arrangeId));
        }
        Wrapper wrapper = new Wrapper(); //設計したWrapperクラスの実体化
        wrapper.items = dataList; //JSON化に耐えうる形で再構築したdataListの中身をwrapperクラスのitemsリストに代入

        //情報をJSON変換して文字列型の変数に格納
        string json = JsonUtility.ToJson(wrapper);

        //PCのライブラリに情報を記録
        PlayerPrefs.SetString("ConsumedJson", json); //消費リストを文字列(JSON)化した情報を特定のキーワードで保存

        PlayerPrefs.SetInt("GoldKey", GameController.hasGoldKey);
        PlayerPrefs.SetInt("SilverKey", GameController.hasSilverKey);
        PlayerPrefs.SetInt("Bullet", GameController.hasBullet);
        PlayerPrefs.SetInt("Life", PlayerController.hp);

        int lightCount = 0;
        if(LightController.getLight)lightCount = 1; //ライトフラグが立っていれば1、そうでなければ０
        PlayerPrefs.SetInt("Light", lightCount);

        PlayerPrefs.SetString("SaveScene",SceneManager.GetActiveScene().name);　//セーブした時のシーン名の保存

        PlayerPrefs.SetFloat("posX", player.transform.position.x);
        PlayerPrefs.SetFloat("posY", player.transform.position.y);

        PlayerPrefs.Save(); //ディスクへ反映させる


    }

    //ゲームを再開する
    public static void LoadGame()
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");

        //復元先のリストをまっさらにして準備
        SaveController.Instance.consumedEvent.Clear();

        string json = PlayerPrefs.GetString("ConsumedJson");
        if(!string.IsNullOrEmpty(json))
        {
            //Wrapperクラスのitemsリストに復元された情報をconsumedEventリストをそれぞれ再現
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
            foreach(var item in wrapper.items)
            {
                SaveController.Instance.ConsumedEvent(item.tag, item.arrrangeId);
            }
        }

        GameController.hasGoldKey = PlayerPrefs.GetInt("GoldKey");
        GameController.hasSilverKey = PlayerPrefs.GetInt("SilverKey");
        GameController.hasBullet = PlayerPrefs.GetInt("Bullet");
        PlayerController.hp = PlayerPrefs.GetInt("Life");
        int lightCount = PlayerPrefs.GetInt("Light");
        if (lightCount == 1) LightController.getLight = true;

        string sceneName = PlayerPrefs.GetString("SaveScene");
        if (string.IsNullOrEmpty(sceneName)) sceneName = "Title";

        RoomController.isContinue = true; //コンテニューしたというフラグ

        SceneManager.LoadScene(sceneName);
    }





}
