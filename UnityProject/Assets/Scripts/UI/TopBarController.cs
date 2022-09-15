using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarController : MonoBehaviour
{
    public static TopBarController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI userCoinValueText;
    public UIInvenPanel InvenPanelObject;

    public Toggle ItemToggle;
    public Toggle SkillsToggle;
    public Toggle EquipmentToggle;
    public Toggle StatusToggle;
    public Toggle FormationToggle;

    public GameObject ToggleListPanel;


    void Start()
    {
        InvenPanelObject.gameObject.SetActive(false);
        ToggleListPanel.gameObject.SetActive(true);
        UserProfileUpdated();
    }
    private void OnEnable()
    {
        GlobalController.Instance.UserProfileUpdatedEvent += UserProfileUpdatedEvent;
    }
    private void OnDisable()
    {
        GlobalController.Instance.UserProfileUpdatedEvent -= UserProfileUpdatedEvent;
    }

    private void UserProfileUpdatedEvent()
    {
        UserProfileUpdated();
    }

    public void UserProfileUpdated()
    {
        playerNameText.text = GlobalController.Instance.LoginUser.UserName;
        userCoinValueText.text = GlobalController.Instance.LoginUser.Coins.ToString();
    }

    public void ToggleChanged()
    {
        if (ItemToggle.isOn == true)
        {
            InvenPanelObject.ShowInvenPanel(0);
        }
        else if (SkillsToggle.isOn == true)
        {
            InvenPanelObject.ShowInvenPanel(1);
        }
        else if (EquipmentToggle.isOn == true)
        {
            InvenPanelObject.ShowInvenPanel(2);
        }
        else if (StatusToggle.isOn == true)
        {
            InvenPanelObject.ShowInvenPanel(3);
        }
        else if (FormationToggle.isOn == true)
        {
            InvenPanelObject.ShowInvenPanel(4);
        }
        InvenPanelObject.gameObject.SetActive(true);
    }

    public void CloseInvenPanel()
    {
        ItemToggle.isOn = false;
        SkillsToggle.isOn = false;
        EquipmentToggle.isOn = false;
        StatusToggle.isOn = false;
        FormationToggle.isOn = false;

        InvenPanelObject.gameObject.SetActive(false);
    }

    public void ShowToggleList()
    {
        ToggleListPanel.SetActive(!ToggleListPanel.activeSelf);
    }
}
