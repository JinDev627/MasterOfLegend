using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIUserNameInput : MonoBehaviour
{
    public TMP_InputField userNameInput;
    public GameObject ConnectPanelObject;
    public ConnectToWallet LoginPanel;

    public void OnOKButton()
    {
        if (string.IsNullOrEmpty(userNameInput.text) == false)
        {
            User newUser = new User();
            newUser.UserName = userNameInput.text;
            newUser.Coins = 0;
            newUser.lastReceiveRewardTime = 0;
            newUser.UserID = GlobalController.Instance.USER_WALLET_ACCOUNT;
            ProfileController.Instance.SetUserData(newUser, SetUserDataCallback);
            ConnectPanelObject.SetActive(true);
        }
    }

    private void SetUserDataCallback(CallbackSetUserDataMessage logMessage)
    {
        if (logMessage.IsSuccess == true)
        {
            GlobalController.Instance.LoginUser = logMessage.UserData;
            gameObject.SetActive(false);
            LoginPanel.LoadNextScene();
        }
        else
        {
            ConnectPanelObject.SetActive(false);
        }
    }
    
}
