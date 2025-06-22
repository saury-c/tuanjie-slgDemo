using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManager 
{

    public static readonly RoleManager Instance = new RoleManager();
    public List<string> cardList; //ÓµÓÐµÄ¿¨ÅÆid
    public void Init()
    {
        cardList = new List<string>();
        for (int i = 0; i < 4; ++i)
        {
            cardList.Add("1001");
            cardList.Add("1003");
            if (i % 2 == 0) cardList.Add("1002");
        }

    }

}
