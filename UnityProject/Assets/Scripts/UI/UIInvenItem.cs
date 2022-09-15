using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIInvenItem : MonoBehaviour
{
    public TextMeshProUGUI itemCountText;
    public ItemDescription ItemData;
    public TextMeshProUGUI itemNameText;
    public RawImage ItemIconImage;
    public void SetInvenItemData(ItemDescription itemData)
    {
        ItemIconImage.gameObject.SetActive(false);
        this.ItemData = itemData;
        itemCountText.text = "x " + itemData.balance;
        itemNameText.text = ItemData.name;
        if (ItemData.LoadedItemTexture != null)
        {
            ItemIconImage.texture = ItemData.LoadedItemTexture;
            ItemIconImage.gameObject.SetActive(true);
        }
        
    }
}
