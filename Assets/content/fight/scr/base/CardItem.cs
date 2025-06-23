using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Dictionary<string, string> data;
    public string[] vals;
    private int index;
    Vector2 initPos;

    private void Start()
    {
        vals = data["Arg"].Split("/");
        transform.Find("bg").GetComponent<Image>().sprite = Resources.Load<Sprite>(data["BgIcon"]);
        transform.Find("bg/icon").GetComponent<Image>().sprite = Resources.Load<Sprite>(data["Icon"]);
        transform.Find("bg/msgTxt").GetComponent<TextMeshProUGUI>().text = string.Format(data["Des"], vals);
        transform.Find("bg/nameTxt").GetComponent<TextMeshProUGUI>().text = data["Name"];
        transform.Find("bg/useTxt").GetComponent<TextMeshProUGUI>().text = data["Expend"];
        transform.Find("bg/Text").GetComponent<TextMeshProUGUI>().text = GameConfigManager.Instance.GetById(ConfigType.CardType, data["Type"])["Name"];

        transform.Find("bg").GetComponent<Image>().material = Instantiate(Resources.Load<Material>("Mats/outline"));
    }

    public void Init(Dictionary<string, string> data)
    {
        this.data = data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.3f, 0.25f);
        //transform.position  = new Vector3(transform.position.x, transform.position.y+45, transform.position.z);
        index = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
        transform.Find("bg").GetComponent<Image>().material.SetColor("_lineColor", Color.yellow);
        transform.Find("bg").GetComponent<Image>().material.SetFloat("_lineWidth", 10);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1, 0.25f);
        //transform.position = new Vector3(transform.position.x, transform.position.y - 45, transform.position.z);

        transform.SetSiblingIndex(index);
        transform.Find("bg").GetComponent<Image>().material.SetColor("_lineColor", Color.black);
        transform.Find("bg").GetComponent<Image>().material.SetFloat("_lineWidth", 1);
    }



    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        initPos = transform.GetComponent<RectTransform>().anchoredPosition;
        AudioManager.Instance.PlayEffect("Cards/draw");
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out pos))
        {
            transform.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.GetComponent<RectTransform>().anchoredPosition = initPos;
        transform.SetSiblingIndex(index);
    }


    public virtual bool TryUse()
    {
        int cost = int.Parse(data["Expend"]);
        if (cost > FightManager.Instance.CurPowerCount)
        {
            AudioManager.Instance.PlayEffect("Effect/lose");
            UIManager.Instance.ShowTip("消耗点数不足", Color.red);
            return false;
        }
        else
        {
            FightManager.Instance.CurPowerCount -= cost;
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdataPower();
            UIManager.Instance.GetUI<FightUI>("FightUI").RemoveCard(this);
            return true;

        }
    }

    public void PlayEffect(Vector3 pos)
    {
        var effect = Resources.Load(data["Effects"]);
        if (effect != null)
        {
            GameObject effectObj = Instantiate(effect) as GameObject;
            effectObj.transform.position = pos;
            Destroy(effectObj, 2);
        }
        else
        {
            Debug.LogWarning("找不到播放特效" + data["Effects"].ToString());
        }



    }


}
