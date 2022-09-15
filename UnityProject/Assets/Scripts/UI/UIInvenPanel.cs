using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInvenPanel : MonoBehaviour
{
    public GameObject ItemInvenPanel;
    public GameObject SkillInvenPanel;
    public GameObject EquipmentInvenPanel;
    public GameObject StatusInvenPanel;
    public GameObject FormationInvenPanel;

    public void ShowInvenPanel(int index)
    {
        ItemInvenPanel.SetActive(false);
        SkillInvenPanel.SetActive(false);
        EquipmentInvenPanel.SetActive(false);
        StatusInvenPanel.SetActive(false);
        FormationInvenPanel.SetActive(false);

        switch (index)
        {
            case 0:
                ItemInvenPanel.SetActive(true);
                break;
            case 1:
                SkillInvenPanel.SetActive(true);
                break;
            case 2:
                EquipmentInvenPanel.SetActive(true);
                break;
            case 3:
                StatusInvenPanel.SetActive(true);
                break;
            case 4:
                FormationInvenPanel.SetActive(true);
                break;
        }
    }


    

    public void OnCloseButton()
    {
        gameObject.SetActive(false);
    }
}
