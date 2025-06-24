using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public enum ActionType
{
    None, Defend, Attack,
}

/// <summary>
/// 敌人脚本
/// </summary>
public class Enemy : MonoBehaviour
{

    public Dictionary<string, string> data;
    public ActionType type;
    public GameObject hpItemObj;


    public List<string> actions;

    [Header("UI")]
    public TextMeshProUGUI defendTxt;
    public TextMeshProUGUI hpTex;
    public Image hpImg;

    [Header("数值")]
    public int Defend;
    public int Attack;
    public int MaxHp;
    public int CurHp;
    public int CurPow;
    public int MaxPow;

    SkinnedMeshRenderer _meshRenderer;
    public Animator ani;


    public void Init(Dictionary<string, string> data)
    {
        this.data = data;
        actions = new List<string>();
        InitAction();

    }

    private void Start()
    {
        _meshRenderer = transform.GetComponentInChildren<SkinnedMeshRenderer>();
        ani = transform.GetComponent<Animator>();

        type = ActionType.None;
        hpItemObj = UIManager.Instance.CreateHpItem();

        defendTxt = hpItemObj.transform.Find("txt_defend").GetComponent<TextMeshProUGUI>();
        hpTex = hpItemObj.transform.Find("txt_hp").GetComponent<TextMeshProUGUI>();
        hpImg = hpItemObj.transform.Find("sp_hp").GetComponent<Image>();

        hpItemObj.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.down * 0.25f);
        SetRandomAction();

        //初始化
        Attack = int.Parse(data["Attack"]);
        CurHp = int.Parse(data["Hp"]);
        MaxHp = CurHp;
        Defend = int.Parse(data["Defend"]);
        MaxPow = int.Parse(data["Pow"]);
        CurPow = MaxPow;
        UpdateHp();
        UpdateDefend();
    }

    public void SetRandomAction()
    {
        int ran = Random.Range(0, 3);
        if (CurPow == -1)
        {
            CurPow = 0;
        }
        else
        {
            CurPow = MaxPow;
        }

        type = (ActionType)ran;

    }


    public void UpdateHp()
    {
        hpTex.text = CurHp + "/" + MaxHp;
        hpImg.fillAmount = (float)CurHp / (float)MaxHp;
    }

    public void UpdateDefend()
    {
        defendTxt.text = Defend.ToString();
    }


    public void Hit(int val)
    {
        if (gameObject.GetComponent<HurtAddDefence>())
        {
            gameObject.GetComponent<HurtAddDefence>().addDefence();
            UpdateDefend();
        }

        if (Defend >= val)
        {
            Defend -= val;
            ani.Play("hit", 0, 0);
        }
        else
        {
            val = val - Defend;
            Defend = 0;
            CurHp -= val;
            if (CurHp <= 0)
            {
                CurHp = 0;
                //ani.Play("die");
                EnemyManager.Instacne.DeleteEnemy(this);
                MonoBehaviour[] scripts = gameObject.GetComponents<MonoBehaviour>();

                // 遍历每个脚本并删除
                foreach (MonoBehaviour script in scripts)
                {
                    Destroy(script);
                }

                Destroy(gameObject);
                Destroy(hpItemObj);
            }
            else
            {
                ani.Play("hit", 0, 0);
            }
        }
        UpdateDefend();
        UpdateHp();
    }


    public IEnumerator DoAction()
    {
        //ani.Play("attack");
        yield return new WaitForSeconds(0.5f);
        int doCount = 0;
        while (doCount <= 15 || actions.Count > 0)
        {

            string actionId = actions[0];

            Dictionary<string, string> data = GameConfigManager.Instance.GetById(ConfigType.EnemyAction, actionId);
            int pow = int.Parse(data["Pow"]);
            if (pow > CurPow)
            {
                break;
            }
            actions.RemoveAt(0);
            if (data["Range"].Equals("0"))
            {
                actions.Add(actionId);
            }
            EnemyAction item = this.gameObject.AddComponent(System.Type.GetType(data["Script"])) as EnemyAction;

            item.Init(data, this.gameObject);
            item.TackAction();
            //yield return new WaitForSeconds(10);
            doCount++;
            CurPow -= pow;

            Destroy(item);
            yield return new WaitForSeconds(0.5f);

        }


        yield return new WaitForSeconds(1);
        //ani.Play("idle");
    }

    public void InitAction()
    {
        string[] actionList = data["EnemyAciton"].Split("/");
        for (int i = 0; i < actionList.Length; ++i)
        {
            //Debug.Log("Enemy:" + gameObject.name + "Add num" + i + " Action id " + actionList[i]);
            actions.Add(actionList[i]);
        }
    }

}
