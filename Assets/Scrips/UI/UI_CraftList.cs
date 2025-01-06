using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject CraftPrefab;
    [SerializeField] private Transform CraftSlotParent;
    [SerializeField] private List<ItemData_Equipment> itemList = new List<ItemData_Equipment>();

    public void SetUpCraftList()
    {
        for (int i = 0; i < CraftSlotParent.childCount; i++)
        {
            Destroy(CraftSlotParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject newCraft = Instantiate(CraftPrefab, CraftSlotParent);
            UI_CraftSlot craftSlot = newCraft.GetComponent<UI_CraftSlot>();
            craftSlot.SetUpCraftSlot(itemList[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetUpCraftList();
    }
}
