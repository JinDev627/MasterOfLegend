using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRewardPanel : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public Button receiveButton;
    // Start is called before the first frame update
    void Start()
    {
    }

    

    public void ReceiveReward()
    {
        receiveButton.interactable = false;
        GlobalController.Instance.LoginUser.lastReceiveRewardTime = CommonUtils.ConvertToTimestamp(DateTime.Now);
        GlobalController.Instance.LoginUser.Coins += 10;
        ProfileController.Instance.SetUserData(GlobalController.Instance.LoginUser , UserProfileUpdated);
        gameObject.SetActive(false);
    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
    }

    private void UserProfileUpdated(CallbackSetUserDataMessage logMessage)
    {
        if (logMessage.IsSuccess == true)
        {
            GlobalController.Instance.UpdateProfile();
        }
    }


}
