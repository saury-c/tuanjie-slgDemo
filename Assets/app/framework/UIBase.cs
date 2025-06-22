using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{

    public UIEventTrigger Register(string name)
    {
        Transform tf = transform.Find(name);
        if (tf == null)
        {
            Debug.LogError($"找不到路径为 {name} 的物体，请检查层级结构和路径是否正确", this);
        }
        return UIEventTrigger.Get(tf.gameObject);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Close()
    {
        UIManager.Instance.CloseUI(gameObject.name);

    }

}
