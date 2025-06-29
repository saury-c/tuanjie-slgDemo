using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VictoryUI : UIBase
{
    public int card1;
    public int card2;
    private void Awake()
    {
        Register("continue").onClick = OnContinueGame;
        Register("card01").onClick = GetCard1;
        Register("card02").onClick = GetCard2;
        VictoryEvent();
        //
        TextMeshProUGUI txt = transform.Find("txt_des").GetComponent<TextMeshProUGUI>();
        Register("card01").onEnter = (GameObject go, PointerEventData e) =>
        {
            Dictionary<string, string> dict = GameConfigManager.Instance.GetById(ConfigType.Card, card1.ToString());
            string des = dict["Des"]; // 模板文本
            string[] args = dict["Arg"].Split('/'); // 切割参数
            string finalText = string.Format(des, args); // 替换模板中的 {0}、{1}
            txt.text = finalText;
            txt.gameObject.SetActive(true);
        };
        Register("card01").onExit = (GameObject go, PointerEventData e) =>
        {
            txt.gameObject.SetActive(false);
        };
        Register("card02").onEnter = (GameObject go, PointerEventData e) =>
        {
            Dictionary<string, string> dict = GameConfigManager.Instance.GetById(ConfigType.Card, card2.ToString());
            string des = dict["Des"]; // 模板文本
            string[] args = dict["Arg"].Split('/'); // 切割参数
            string finalText = string.Format(des, args); // 替换模板中的 {0}、{1}
            txt.text = finalText;
            txt.gameObject.SetActive(true);
        };
        Register("card02").onExit = (GameObject go, PointerEventData e) =>
        {
            txt.gameObject.SetActive(false);
        };
    }

    private void VictoryEvent(int val = 30)
    {
        FightManager.Instance.Coin += val;
        card1 = Random.Range(1003, 1020);
        transform.Find("card01").GetComponent<Image>().sprite = Resources.Load<Sprite>(GameConfigManager.Instance.GetById(ConfigType.Card, card1.ToString())["Icon"]);
        card2 = Random.Range(1003, 1020);
        transform.Find("card02").GetComponent<Image>().sprite = Resources.Load<Sprite>(GameConfigManager.Instance.GetById(ConfigType.Card, card2.ToString())["Icon"]);
    }


    private void OnContinueGame(GameObject obj, PointerEventData pData)
    {
        Close();

        FightManager.Instance.ChangeType(FightType.Init);
    }

    public void GetCard1(GameObject obj, PointerEventData pData)
    {
        FightCardManager.Instance.cardList.Add(card1.ToString());
        Close();

        FightManager.Instance.ChangeType(FightType.Init);

    }
    public void GetCard2(GameObject obj, PointerEventData pData)
    {
        FightCardManager.Instance.cardList.Add(card2.ToString());
        Close();

        FightManager.Instance.ChangeType(FightType.Init);

    }
}
