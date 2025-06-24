using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum FightType
{
    None, Init, Player, Enemy, Win, Loss
}

public class FightManager : MonoBehaviour
{
    public static FightManager Instance;
    public FightUnit fightUnit; // 当前回合状态


    public int MaxHp;
    public int CurHp;
    public int MaxPowerCount;
    public int CurPowerCount;
    public int DefenseCount;
    public int Coin;

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        MaxHp = 80;
        CurHp = 80;
        MaxPowerCount = 3;
        CurPowerCount = 3;
        DefenseCount = 10;
        Coin = 0;

        UIManager.Instance.ShowUI<ChooseUI>("ChooseUI");
        UIManager.Instance.GetUI<ChooseUI>("ChooseUI").Init("p1");
    }



    public void ChangeType(FightType type)
    {
        switch (type)
        {
            case FightType.None:
                break;
            case FightType.Init:
                fightUnit = new FightInit();
                break;
            case FightType.Player:
                fightUnit = new Fight_PlayerTurn();
                break;
            case FightType.Enemy:
                fightUnit = new Fight_EnemyTurn();
                break;
            case FightType.Win:
                fightUnit = new Fight_Win();
                break;
            case FightType.Loss:
                fightUnit = new Fight_Loss();
                break;
        }
        fightUnit.Init();
    }

    public void GetPlayerHit(int hit)
    {
        if (DefenseCount >= hit)
        {
            DefenseCount -= hit;
        }
        else
        {
            hit -= DefenseCount;
            DefenseCount = 0;
            CurHp -= hit;
            if (CurHp <= 0)
            {
                CurHp = 0;
                ChangeType(FightType.Loss);
                return;
            }
            else
            {

            }
        }
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdataDefense();
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHP();

    }

    private void Update()
    {
        if (fightUnit != null)
        {
            fightUnit.OnUpdate();
        }
    }


}
