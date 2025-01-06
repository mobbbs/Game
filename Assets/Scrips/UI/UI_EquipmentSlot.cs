using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + equipmentType;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        Inventory.Instance.AddItem(item.data as ItemData_Equipment);
        Inventory.Instance.UnequipItem(item.data as ItemData_Equipment);
        CleanUpSlot();
    }
}
