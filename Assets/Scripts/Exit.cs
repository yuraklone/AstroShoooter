using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//出口として機能した時のプレイヤーの位置
public enum ExitDirection
{
    right,
    left,
    up,
    down
}


public class Exit : MonoBehaviour
{
    public string sceneName; //切替先のシーン名
    public int doorNumber; //切替先の出入口との連動番号

    //自作した列挙型でプレイヤーをどの位置に置く出口なのか決めておく変数
    public ExitDirection direction = ExitDirection.down;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //RoomControllerのシーン切替メソッド発動
            RoomController.ChangeScene(sceneName, doorNumber);
        }
    }
}
