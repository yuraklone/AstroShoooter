using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public string sceneName; //ゲームスタートのシーン名
    public Button continueButton;

    // Start is called before the first frame update
    void Start()
    {
        //セーブシーンの保存があるかどうか
        string lastScene = PlayerPrefs.GetString("SaveScene");
        //なければコンテニューボタンの無効化
        if (lastScene == "") continueButton.interactable = false;

        SoundController.soundController.PlayBgm(BGMType.Title);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //データ削除専用のメソッド
    void SaveReset()
    {
        //セーブデータの消去
        PlayerPrefs.DeleteAll();
        continueButton.interactable = false;

    }

    public void GameStart()
    {
        PlayerPrefs.DeleteAll(); //全消し
        SceneManager.LoadScene(sceneName);
    }

    public void ContinueStart()
    {
        SaveSystem.LoadGame();
    }
}
