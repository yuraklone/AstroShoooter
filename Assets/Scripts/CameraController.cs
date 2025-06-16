using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject otherTarget; //中間地点モードの対象
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        
        if(otherTarget != null) //もし中間モードのエネミーがセットされていれば
        {
            //線形補間の第三引数の進捗を常に50％にして中間座標の算出
            Vector2 pos = Vector2.Lerp(player.transform.position, otherTarget.transform.position,0.5f);
            //posに確保していた値を自分の座標に反映
            transform.position = new Vector3(pos.x,pos.y,transform.position.z);
        }
        else
        {   //プレイヤーに追従
            transform.position = new Vector3 (player.transform.position.x,player.transform.position.y, transform.position.z);
        }

    }
}
