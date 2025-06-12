using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    //エディター上でコンポーネントをアタッチ
    public PlayerController playerCnt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerControllerが持っているangleZの数値だけRotationbodyを回転
        transform.rotation = Quaternion.Euler(0, 0, playerCnt.angleZ);
    }
}
