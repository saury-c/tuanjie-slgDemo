using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;
using DG.Tweening;
using TMPro;

public class FightUI : UIBase
{
    private TextMeshProUGUI cardCountTxt;
    private TextMeshProUGUI usedCardCountTxt;
    private TextMeshProUGUI powerTxt;
    private TextMeshProUGUI hpTxt;
    private Image hpImg;
    private TextMeshProUGUI dfTxt;
    private List<CardItem> cardItemList;


    private void Awake()
    {
        cardItemList = new List<CardItem>();
        usedCardCountTxt = transform.Find("node_right/txt_card_no").GetComponent<TextMeshProUGUI>();
        cardCountTxt = transform.Find("node_left/txt_card_has").GetComponent<TextMeshProUGUI>();
        powerTxt = transform.Find("node_left/txt_point").GetComponent<TextMeshProUGUI>();
        hpTxt = transform.Find("node_left/HpItem/txt_hp").GetComponent<TextMeshProUGUI>();
        hpImg = transform.Find("node_left/HpItem/sp_hp").GetComponent<Image>();
        dfTxt = transform.Find("node_left/HpItem/txt_defend").GetComponent<TextMeshProUGUI>();

        transform.Find("node_right/btn_turn_end").GetComponent<Button>().onClick.AddListener(onChangeTurnBtn);
    }

    private void onChangeTurnBtn()
    {
        if (FightManager.Instance.fightUnit is Fight_PlayerTurn)
        {
            FightManager.Instance.ChangeType(FightType.Enemy);
        }
    }

    private void Start()
    {
        UpdataCardCount();
        UpdataDefense();
        UpdataPower();
        UpdataUsedCardCount();
        UpdateHP();
    }

    public void UpdateHP()
    {
        hpTxt.text = FightManager.Instance.CurHp + "/" + FightManager.Instance.MaxHp;
        hpImg.fillAmount = (float)FightManager.Instance.CurHp / (float)FightManager.Instance.MaxHp;
    }
    public void UpdataPower()
    {
        powerTxt.text = FightManager.Instance.CurPowerCount.ToString();
    }

    public void UpdataDefense()
    {
        dfTxt.text = FightManager.Instance.DefenseCount.ToString();
    }

    public void UpdataCardCount()
    {
        cardCountTxt.text = FightCardManager.Instance.cardList.Count.ToString();
    }
    public void UpdataUsedCardCount()
    {
        usedCardCountTxt.text = FightCardManager.Instance.usedCardList.Count.ToString();
    }


    public void CreateCardItem(int count)
    {
        if (count > FightCardManager.Instance.cardList.Count)
        {
            count = FightCardManager.Instance.cardList.Count;
        }
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = Instantiate(Resources.Load("UI/CardItem"), transform) as GameObject;
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, -450);
            string cardId = FightCardManager.Instance.DrawCard();
            Dictionary<string, string> data = GameConfigManager.Instance.GetById(ConfigType.Card, cardId);

            CardItem item = obj.AddComponent(System.Type.GetType(data["Script"])) as CardItem;

            item.Init(data);
            cardItemList.Add(item);
        }
    }

    public void UpdateCardItemPos()
    {
        float offset = 800f / cardItemList.Count;
        Vector2 startPos = new Vector2(-cardItemList.Count / 2f * offset + offset * 0.5f, -450);
        for (int i = 0; i < cardItemList.Count; ++i)
        {
            cardItemList[i].GetComponent<RectTransform>().DOAnchorPos(startPos, 0.5f);
            startPos.x = startPos.x + offset;
        }
    }

    public void RemoveCard(CardItem item)
    {
        AudioManager.Instance.PlayEffect("Cards/cardShove");
        item.enabled = false;
        FightCardManager.Instance.usedCardList.Add(item.data["Id"]);

        usedCardCountTxt.text = FightCardManager.Instance.usedCardList.Count.ToString();
        cardItemList.Remove(item);
        UpdateCardItemPos();

        item.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1000, -450), 0.25f);
        item.transform.DOScale(0, 0.25f);
        Destroy(item.gameObject, 1);
    }

    public void RemoveAllCard()
    {
        for (int i = cardItemList.Count - 1; i >= 0; --i)
        {
            RemoveCard(cardItemList[i]);
        }
    }


}
