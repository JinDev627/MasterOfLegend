
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ConnectToWallet : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;

    public GameObject ConnectPanelObject;
    public GameObject PlayPanelObject;
    public GameObject LoginPanelObject;
    public GameObject TopBarObject;
    public GameObject UserNamePanel;

    void Start()
    {
        ConnectPanelObject.SetActive(true);
        PlayPanelObject.SetActive(false);
        LoginPanelObject.SetActive(true);
        TopBarObject.SetActive(false);
        UserNamePanel.SetActive(false);
        ProfileController.Instance.InitFirebase(FirebaseInited);
    }

    private void FirebaseInited(CallbackInitFirebaseMessage logMessage)
    {
        if (logMessage.IsSuccess == true)
        {
            ConnectPanelObject.SetActive(false);
        }
    }

    public void JoinWithWallet()
    {
        GlobalController.Instance.IsGuestMode = false;
        ConnectPanelObject.SetActive(true);
#if UNITY_EDITOR
        OnLogin();
#else
        Web3Connect();
        OnWebGLConnected();
#endif
    }
    async private void OnLogin()
    {
        // get current timestamp
        int timestamp = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        // set expiration time
        int expirationTime = timestamp + 60;
        // set message
        string message = expirationTime.ToString();
        // sign message
        string signature = await Web3Wallet.Sign(message);
        // verify account
        string account = await EVM.Verify(message, signature);
        int now = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        // validate
        if (account.Length == 42 && expirationTime >= now)
        {
            // save account
            PlayerPrefs.SetString("Account", account);
            GlobalController.Instance.USER_WALLET_ACCOUNT = account.ToLower();
            FirebaseLogin();
        }
    }

    async private void OnWebGLConnected()
    {
        account = ConnectAccount();
        while (account == "")
        {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        // save account for next scene
        GlobalController.Instance.USER_WALLET_ACCOUNT = account.ToLower();
        // reset login message
        SetConnectAccount("");
        FirebaseLogin();
    }


    private void FirebaseLogin()
    {
        ProfileController.Instance.LoginWithCustomTokenID(FirebaseLoginMessage);
    }

    private void FirebaseLoginMessage(CallbackLoginMessage logMessage)
    {
        if (logMessage.IsSuccess == true)
        {
            ProfileController.Instance.GetUserData(GlobalController.Instance.USER_WALLET_ACCOUNT, GetUserDataCallback);
        }
    }

    private void GetUserDataCallback(CallbackGetUserDataMessage logMessage)
    {
        if (logMessage.IsSuccess == true && logMessage.UserData != null)
        {
            GlobalController.Instance.LoginUser = logMessage.UserData;
            LoadNextScene();
        }
        else
        {
            ConnectPanelObject.SetActive(false);
            UserNamePanel.SetActive(true);
        }
    }

    public void AuthSuccess(string result)
    {
        Debug.Log("AuthSuccess:" + result);
        LoadNextScene();
    }
    public void AuthFailed(string result)
    {
        Debug.Log("AuthFailed:" + result);
    }


    public async void LoadNextScene()
    {
        string response = await RacingRuleWeb3.ListAllErc1155();
        string jsonString = "{ \"items\": " + response + "}";
        GlobalController.Instance.UserInventory = JsonConvert.DeserializeObject<OpenSeaInventory>(jsonString);
        await LoadAllItemData();

        PlayPanelObject.SetActive(true);
        ConnectPanelObject.SetActive(false);
        TopBarObject.SetActive(true);
    }

    async public static Task<bool> LoadAllItemData()
    {
        GlobalController.Instance.UserItemDatas.Clear();
        foreach (OpenSeaERC115Item itemData in GlobalController.Instance.UserInventory.items)
        {
            if (GlobalController.Instance.ItemDataDic.ContainsKey(itemData.uri) == false)
            {
                UnityWebRequest request = UnityWebRequest.Get(itemData.uri);
                await request.SendWebRequest();
                if (request.isDone == true)
                {
                    var text = request.downloadHandler.text;
                    Debug.Log("ItemData Loaded:" + text);
                    GlobalController.Instance.ItemDataDic[itemData.uri] = text;
                }
            }
            if (GlobalController.Instance.ItemDataDic.ContainsKey(itemData.uri) == true)
            {
                var text = GlobalController.Instance.ItemDataDic[itemData.uri];
                JObject itemDescObject = (JObject)(JsonConvert.DeserializeObject(text));
                ItemDescription itemDesc = new ItemDescription(itemDescObject);
                itemDesc.balance = int.Parse(itemData.balance);
                GlobalController.Instance.UserItemDatas.Add(itemDesc);
                if (GlobalController.Instance.ItemTextureDic.ContainsKey(itemDesc.image) == false)
                {
                    UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(itemDesc.image);
                    await imageRequest.SendWebRequest();
                    if (imageRequest.result == UnityWebRequest.Result.Success)
                    {
                        GlobalController.Instance.ItemTextureDic[itemDesc.image] = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;
                    }
                }
                if (GlobalController.Instance.ItemTextureDic.ContainsKey(itemDesc.image) == true)
                {
                    itemDesc.LoadedItemTexture = GlobalController.Instance.ItemTextureDic[itemDesc.image];
                }

            }
        }
        return true;
    }



}
