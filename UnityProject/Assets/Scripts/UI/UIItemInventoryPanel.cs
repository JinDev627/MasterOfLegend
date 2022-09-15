using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemInventoryPanel : MonoBehaviour
{
    public GameObject InvenItemPrefab;
    public Transform InvenItemListTrans;

    private void OnEnable()
    {
        LoadInvenList();
    }
    public void LoadInvenList()
    {
        foreach (Transform childTrans in InvenItemListTrans)
        {
            GameObject.Destroy(childTrans.gameObject);
        }

        Timer.Schedule(this, 0.01f, () => {
            foreach (ItemDescription item in GlobalController.Instance.UserItemDatas)
            {
                if (item.isMasterOfLegends == true)
                {
                    GameObject invenItem = GameObject.Instantiate(InvenItemPrefab, InvenItemListTrans);
                    invenItem.GetComponent<RectTransform>().sizeDelta = new Vector2(570, 85);
                    invenItem.transform.localScale = Vector3.one;
                    invenItem.GetComponent<UIInvenItem>().SetInvenItemData(item);
                }
            }
            InvenItemListTrans.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
        });

        
    }
}
