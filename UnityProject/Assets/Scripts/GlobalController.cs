using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController
{
    private static GlobalController _instance;

    public static GlobalController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalController();
            }
            return _instance;
        }
    }
    GlobalController()
    {

    }

    public delegate void UserProfileUpdatedDelegate();
    public event UserProfileUpdatedDelegate UserProfileUpdatedEvent;

    public string USER_WALLET_ACCOUNT;
    public OpenSeaInventory UserInventory;
    public bool IsGuestMode;
    public User LoginUser;

    public Dictionary<string, string> ItemDataDic = new Dictionary<string, string>();
    public Dictionary<string, Texture> ItemTextureDic = new Dictionary<string, Texture>();

    public List<ItemDescription> UserItemDatas = new List<ItemDescription>();

    public void UpdateProfile()
    {
        UserProfileUpdatedEvent?.Invoke();
    }
}
