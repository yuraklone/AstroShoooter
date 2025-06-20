using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BGMのタイプ

public enum BGMType
{
    None, //なし
    Title,　//タイトル
    InGame, //ゲーム中
    InBoss　//ボス戦
}

//SEのタイプ
public enum SEType
{
    GameClear, //ゲームクリア
    GameOver, //ゲームオーバー
    Shoot //発砲時
}


public class SoundController : MonoBehaviour
{
    public AudioClip bgmInTitle;
    public AudioClip bgmInGame;
    public AudioClip bgmInBoss;

    public AudioClip meGameClear;
    public AudioClip meGameOver;
    public AudioClip seShoot;

    public static SoundController soundController;
    public static BGMType playingBGM = BGMType.None;

    void Awake()
    {
        if (soundController == null)
        {
            soundController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBgm(BGMType type)
    {
        if (type != playingBGM)
        {
            playingBGM = type;
            AudioSource audio = GetComponent<AudioSource>();

            if (type == BGMType.Title)
            {
                audio.clip = bgmInTitle;
            }
            else if (type == BGMType.InGame)
            {
                audio.clip = bgmInGame;
            }
            else if (type == BGMType.InBoss)
            {
                audio.clip = bgmInBoss;
            }
            audio.Play();
        }

    }


    public void StopBgm()
    {
        GetComponent<AudioSource>().Stop();
        playingBGM = BGMType.None;
    }

    public void SEPlay(SEType type)
    {
        if(type == SEType.GameClear)
        {
            GetComponent<AudioSource>().PlayOneShot(meGameClear);
        }
        else if (type == SEType.GameOver)
        {
            GetComponent<AudioSource>().PlayOneShot(meGameOver);
        }
        else if (type == SEType.Shoot)
        {
            GetComponent<AudioSource>().PlayOneShot(seShoot);
        }


    }


}
