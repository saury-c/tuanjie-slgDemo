using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    void Start()
    {
        GameConfigManager.Instance.Init();
        AudioManager.Instance.Init();
        RoleManager.Instance.Init();
        FightCardManager.Instance.Init();

        UIManager.Instance.ShowUI<LoginUI>("LoginUI");
        AudioManager.Instance.PlayBGM("bgm1");

    }


}
