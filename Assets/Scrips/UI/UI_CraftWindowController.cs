using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindowController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private List<GameObject> itemMaterialList;
    ItemData_Equipment purposeItem;
    private int requestMaterialsize;
    private void Start()
    {
        ClearUpCraftWindow();
    }

    private void ClearUpCraftWindow()
    {
        itemIcon.color = Color.clear;
        itemName.color = Color.clear;
        itemDescription.color = Color.clear;
    }

    public void SetUpCraftWindow(ItemData_Equipment data)
    {
        purposeItem = data;
        itemIcon.color = Color.white;
        itemName.color = Color.white;
        itemDescription.color = Color.white;
        itemIcon.sprite = data.icon;
        itemName.text = data.itemName;
        itemDescription.text = data.GetDescription();

        for (int i = 0; i < itemMaterialList.Count; i++)
        {
            itemMaterialList[i].SetActive(false);
        }

        requestMaterialsize = 0;
        for (int i = 0; i < itemMaterialList.Count && i < data.craftingMaterials.Count; i++)
        {
            UI_ItemSlot itemSlot = itemMaterialList[i].GetComponentInChildren<UI_ItemSlot>();
            requestMaterialsize++;
            itemMaterialList[i].SetActive(true);
            itemSlot.UpdataSlot(data.craftingMaterials[i]);
        }
    }

    public void checkCraft()
    {
        bool ok = true;
        for (int i = 0; i < requestMaterialsize; i++)
        {
            Inventory inventory = Inventory.Instance;
            UI_ItemSlot itemSlot = itemMaterialList[i].GetComponentInChildren<UI_ItemSlot>();
            if (inventory.stashDictionary.TryGetValue(itemSlot.item.data, out InventoryItem item))
            {
                if (item.stackSize < itemSlot.item.stackSize)
                {
                    ok = false;
                }
            }
            else
            {
                ok = false;
            }
        }
        if (ok)
        {
            Inventory inventory = Inventory.Instance;
            for (int i = 0; i < requestMaterialsize; i++)
            {
                UI_ItemSlot itemSlot = itemMaterialList[i].GetComponentInChildren<UI_ItemSlot>();
                if (inventory.stashDictionary.TryGetValue(itemSlot.item.data, out InventoryItem item))
                {
                    item.stackSize -= itemSlot.item.stackSize;
                    if (item.stackSize == 0)
                    {
                        inventory.RemoveItem(itemSlot.item.data);
                    }
                }
            }
            inventory.AddItem(purposeItem);

        }
        else
        {
            Debug.Log("No enough");
        }
    }

}
