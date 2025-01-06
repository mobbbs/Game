using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItems;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.Instance;
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();

        foreach(InventoryItem item in currentEquipment)
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.data);
                inventory.UnequipItem(item.data as ItemData_Equipment);
            }
        }
    }
}
