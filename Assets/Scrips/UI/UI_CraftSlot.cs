using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    public void SetUpCraftSlot(ItemData_Equipment data)
    {
        item.data = data;
        itemImage.sprite = data.icon;
        itemText.text = data.itemName.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;
        ui.craftWindow.SetUpCraftWindow(craftData);   
    }
}
