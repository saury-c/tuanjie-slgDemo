using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddCard : CardItem
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (TryUse() == true)
        {
            int val = int.Parse(vals[0]);
            //Debug.LogWarning("��Ϣ__" + this.vals.ToString() + "___" + val);
            if (FightCardManager.Instance.HasCard(val) == true)
            {
                // ��������������Ч
                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2.5f));
                PlayEffect(pos);

                //Debug.LogWarning("���ɿ���");
                UIManager.Instance.GetUI<FightUI>("FightUI").CreateCardItem(val);
            }
            else
            {
                base.OnEndDrag(eventData);
                FightCardManager.Instance.RefreahCard();
            }
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdataUsedCardCount();
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdataCardCount();
        }
        else
        {
            base.OnEndDrag(eventData);
        }

    }
}
