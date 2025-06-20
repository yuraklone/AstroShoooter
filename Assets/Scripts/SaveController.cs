using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    // 自分自身をstatic化して顕現することで自分の中の変数を他のシーンへ持ち越す準備
    public static SaveController Instance;

    //HashSetはListの亜種のようなもの、複数情報格納可
    public HashSet<(string tag, int arrangeId)> consumedEvent = new HashSet<(string tag, int arrangeId)>();

    public void Awake()
    {
        if (Instance == null)
        {
            //プログラム自身の情報をstatic変数に格納
            Instance = this;

            DontDestroyOnLoad(gameObject);　//シーンが切り替わってもオブジェクトを引き継ぐ
        }
        else
        {
            //2つ目以降のシーンのSaveControllerであることが確定
            Destroy(gameObject); //一つのシーンで存在が競合しないように自己廃棄
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
    //リストに加えるメソッド
    public void ConsumedEvent(string tag, int arrangeId)
    {
        consumedEvent.Add((tag, arrangeId));
    }

    //すでにリストにあるかどうかのチェックメソッド
    public bool IsConsumed(string tag, int arrangeId)
    {
        //引数に指定したタグと識別番号の組み合わせがすでにリストにあるかどうかを返す
        return consumedEvent.Contains((tag, arrangeId));
    }
}
