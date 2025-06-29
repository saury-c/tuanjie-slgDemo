using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PiercedCard : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData)
    {

    }

    public override void OnDrag(PointerEventData eventData)
    {

    }

    public override void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlayEffect("Cards/draw");
        UIManager.Instance.ShowUI<LineUI>("LineUI");
        UIManager.Instance.GetUI<LineUI>("LineUI").SetStartPos(transform.GetComponent<RectTransform>().anchoredPosition);

        Cursor.visible = false;
        StopAllCoroutines();
        StartCoroutine(OnMouseDownRight(eventData));

    }

    IEnumerator OnMouseDownRight(PointerEventData pData)
    {
        while (true)
        {
            if (Input.GetMouseButton(1))
            {
                break;
            }
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), pData.position, pData.pressEventCamera, out pos))
            {
                UIManager.Instance.GetUI<LineUI>("LineUI").SetEndPos(pos);
                CheckRayToEnemy();
            }
            yield return null;
        }
        Cursor.visible = true;
        UIManager.Instance.CloseUI("LineUI");
    }

    Enemy hitEnemy;
    private void CheckRayToEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Enemy")))
        {
            hitEnemy = hit.transform.GetComponent<Enemy>();

            if (Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                Cursor.visible = true;
                UIManager.Instance.CloseUI("LineUI");
                if (TryUse())
                {
                    PlayEffect(hitEnemy.transform.position);
                    AudioManager.Instance.PlayEffect("Effect/sword");
                    int val = int.Parse(vals[0]);
                    if (hitEnemy.Defend + hitEnemy.CurHp <= val)
                    {
                        FightManager.Instance.CurHp += int.Parse(vals[1]);
                        FightManager.Instance.MaxHp += int.Parse(vals[1]);
                        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHP();
                    }
                    //useCard?.OnEventRaised(this);
                    hitEnemy.Hit(val);
                }
                hitEnemy = null;
            }

        }
        else
        {
            if (hitEnemy != null)
            {
                hitEnemy = null;
            }
        }
    }
}
