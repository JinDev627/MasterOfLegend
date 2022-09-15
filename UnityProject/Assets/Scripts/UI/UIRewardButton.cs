using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRewardButton : MonoBehaviour
{
    public TextMeshProUGUI RewardButtonText;
    public Button buttonObject;
    // Start is called before the first frame update
    void Start()
    {
        buttonObject = gameObject.GetComponent<Button>();
        buttonObject.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (GlobalController.Instance.LoginUser != null)
        {
            double timeStamp = GlobalController.Instance.LoginUser.lastReceiveRewardTime;
            DateTime lastReceiveTime = CommonUtils.UnixTimeStampToDateTime(timeStamp);
            if (timeStamp == 0 || (DateTime.Now - lastReceiveTime).TotalDays > 1.0)
            {
                buttonObject.interactable = true;
                RewardButtonText.text = "Get Reward";
            }
            else
            {
                TimeSpan remainTime = new TimeSpan(1, 0, 0, 0) - (DateTime.Now - lastReceiveTime);
                string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", remainTime.Hours, remainTime.Minutes, remainTime.Seconds);
                buttonObject.interactable = false;
                RewardButtonText.text = timeText;
            }
        }
        else
        {
            buttonObject.interactable = false;
            RewardButtonText.text = "23:59:59";
        }
    }
    

    public void OnRewardButton()
    {
        GlobalController.Instance.LoginUser.lastReceiveRewardTime = CommonUtils.ConvertToTimestamp(DateTime.Now);
        GlobalController.Instance.LoginUser.Coins += 10;
        ProfileController.Instance.SetUserData(GlobalController.Instance.LoginUser, UserProfileUpdated);
        buttonObject.interactable = false;
        RewardButtonText.text = "23:59:59";
    }

    private void UserProfileUpdated(CallbackSetUserDataMessage logMessage)
    {
        if (logMessage.IsSuccess == true)
        {
            GlobalController.Instance.UpdateProfile();
        }
    }
}
