using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public static bool getLight = false; //ライトを入手しているかどうかのフラグ
    public static bool onLight = false; //ライトのON・OFF
    public GameObject playerLight; //ライトのオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        playerLight.SetActive(onLight);
    }

    // Update is called once per frame
    void Update()
    {
       if (!getLight) return;

       if(Input.GetKeyDown(KeyCode.T))
            {
            onLight = !onLight;
            playerLight.SetActive(onLight);
        }
    }
}
