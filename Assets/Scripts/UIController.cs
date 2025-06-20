using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //各パネルを宣言
    public TextMeshProUGUI bulletText;
    public TextMeshProUGUI goldKeyText;
    public TextMeshProUGUI silverKeyText;
    public GameObject lightPanel;
    public GameObject[] lifes;

    int hasBullet;
    int hasKeyG;
    int hasKeyS;
    int hasLife;
    bool hasLight;

    public GameObject gameOverPanel; //ゲームオーバー時に出すパネル


    // Start is called before the first frame update
    void Start()
    {
        UIDisplay(); //弾丸、鍵、ライトの初期表示
        hasLife = PlayerController.hp; //一度HPを取得して合わせる
        LifeDisplay();
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(GameController.gameState == "gameover")
        {
            gameOverPanel.SetActive(true);
        }



        //もしUIControllerが把握していた数が各stattic変数に差が出たらUIをそれぞれ更新

        if(hasBullet != GameController.hasBullet)
        {
            hasBullet = GameController.hasBullet;
            bulletText.text = hasBullet.ToString();
        }

        if (hasKeyG != GameController.hasGoldKey)
        {
            hasKeyG = GameController.hasGoldKey;
            goldKeyText.text = hasKeyG.ToString();
        }

        if (hasKeyS != GameController.hasSilverKey)
        {
            hasKeyS = GameController.hasSilverKey;
            silverKeyText.text = hasKeyS.ToString();
        }
 
        if (!hasLight)
        {
            if (LightController.getLight) //staticがtrueになるタイミングを見る
            {
                hasLight = true;
                lightPanel.SetActive(true);
            }
        }

        if(hasLife != PlayerController.hp)
        {
            hasLife = PlayerController.hp;
            LifeDisplay();
        }



    }

    void UIDisplay()
    {
        //各static変数を取得し、text欄に反映
        hasBullet = GameController.hasBullet;
        bulletText.text = hasBullet.ToString();

        hasKeyG = GameController.hasGoldKey;
        goldKeyText.text = hasKeyG.ToString();

        hasKeyS = GameController.hasSilverKey;
        silverKeyText.text = hasKeyS.ToString();

        //ライト取得済みかどうかの把握
        hasLight = LightController.getLight;
        lightPanel.SetActive(hasLight);

    }

    void LifeReset()
    {
        for (int i = 0; i < lifes.Length; i++)
        {
            lifes[i].SetActive(false);
        }

    }

    void LifeDisplay()
    {
        LifeReset(); //Lifeを一度リセット

        for(int i = 0;i < hasLife; i++)
        {
            lifes[i].SetActive(true);
        }
    }

    public void toTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void Retry()
    {
        //BGMをストップ
        SoundController.soundController.PlayBgm(BGMType.None);
        SaveSystem.LoadGame();
    }




            
}
