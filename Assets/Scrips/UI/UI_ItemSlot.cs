using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    public InventoryItem item;

    protected UI ui => GetComponentInParent<UI>();
    public void UpdataSlot(InventoryItem newitem)
    {
        item = newitem;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.Instance.RemoveItem(item.data);
            return;
        }
        if (item == null)
        {
            return;
        }
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.Instance.EquipItem(item.data);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        ui.itemToolTip.HideToolTip();
    }
}
