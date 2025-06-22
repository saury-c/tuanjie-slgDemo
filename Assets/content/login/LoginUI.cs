using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoginUI : UIBase
{
    void Start()
    {
        Register("btn_startGame").onClick = onStartGameBtn;

    }
    private void onStartGameBtn(GameObject obj, PointerEventData pData)
    {
        Close();
        FightManager.Instance.Init();
        FightManager.Instance.ChangeType(FightType.Init);
    }


}
