using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ItemNameText;
    [SerializeField] private TextMeshProUGUI ItemTypeText;
    [SerializeField] private TextMeshProUGUI ItemDescriptionText;

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null)
        {
            return;
        }
        ItemNameText.text = item.itemName;
        ItemTypeText.text = item.itemType.ToString();
        ItemDescriptionText.text = item.GetDescription();
        gameObject.SetActive(true);
    }
    public void HideToolTip() => gameObject.SetActive(false);
}
