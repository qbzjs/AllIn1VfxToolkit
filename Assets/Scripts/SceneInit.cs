using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class SceneInit : MonoBehaviour
{
    void Start()
    {
        UIPackage.AddPackage("FairyGUI/GameInit");
        GameInit.GameInitBinder.BindAll();

        GComponent view = UIPackage.CreateObject("GameInit", "GameInitView").asCom;
        GRoot.inst.AddChild(view);

        int width = Screen.width;
        int height = Screen.height;
        width = Mathf.CeilToInt(width / UIContentScaler.scaleFactor);
        height = Mathf.CeilToInt(height / UIContentScaler.scaleFactor);
        view.SetSize(width, height);
        view.SetXY(0, 0, true);

        // TODO : Main Camera can view the object
    }
}
