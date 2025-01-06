using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventoryItem> startingItem;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;


    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] InventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlots;
    private UI_StatSlot[] statSlots;

    [Header("Items cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUseArmor;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        InventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlots = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
        AddStartingItem();

        lastTimeUseArmor = -Mathf.Infinity;
        lastTimeUsedFlask = -Mathf.Infinity;
    }
    public bool CanAddItem()
    {
        if (inventory.Count >= InventoryItemSlot.Length)
        {
            return false;
        }
        return true;
    }
    private void AddStartingItem()
    {
        for (int i = 0; i < startingItem.Count; i++)
        {
            AddItem(startingItem[i].data, startingItem[i].stackSize);
        }
    }
    public void EquipItem(ItemData item)
    {
        ItemData_Equipment newEquipment = item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(item, 1);
        ItemData_Equipment oldEquipment = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> value in equipmentDictionary)
        {
            if (value.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = value.Key;
            }
        }
        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment, 1);
        }
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
        RemoveItem(item);
    }
    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem val))
        {
            equipment.Remove(val);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
        ClearTheSlotUI();
        UpdateSlotUI();
    }
    public void UpdateSlotUI()
    {
        ClearTheSlotUI();

        for (int i = 0; i < inventory.Count; i++)
        {
            InventoryItemSlot[i].UpdataSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdataSlot(stash[i]);
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> value in equipmentDictionary)
            {
                if (value.Key.equipmentType == equipmentSlots[i].equipmentType)
                {
                    equipmentSlots[i].UpdataSlot(value.Value);
                }
            }
        }

        if (statSlots == null)
        {
            Debug.Log("FUCK");

        }
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdataStatValueUI();
        }
    }
    private void ClearTheSlotUI()
    {
        for (int i = 0; i < InventoryItemSlot.Length; i++)
        {
            InventoryItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].CleanUpSlot();
        }
    }
    public void AddItem(ItemData item, int size = 1)
    {
        if (item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(item, size);
        }else if (item.itemType == ItemType.Material)
        {
            AddToStash(item, size);
        }
        UpdateSlotUI();
    }
    private void AddToStash(ItemData item, int size)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.addStack(size);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item, size);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }
    private void AddToInventory(ItemData item, int size)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.addStack(size);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item, size);
            inventory.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }
    public void RemoveItem(ItemData item)
    {
        ClearTheSlotUI();
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(item);
            }
            else
            {
                value.removeStack();
            }
        }
        if (stashDictionary.TryGetValue(item, out InventoryItem stashvalue))
        {
            if (stashvalue.stackSize <= 1)
            { 
                stash.Remove(stashvalue);
                stashDictionary.Remove(item);
            }
            else
            {
                stashvalue.removeStack();
            }
        }
        UpdateSlotUI();
    }
    public bool CanCraft(ItemData_Equipment itemToCraft, List<InventoryItem> requiredMaterial)
    {
        List<InventoryItem> materialToRemove = new List<InventoryItem>();
        for (int i = 0; i < requiredMaterial.Count; i++)
        {
            if (stashDictionary.TryGetValue(requiredMaterial[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < requiredMaterial[i].stackSize)
                {
                    Debug.Log("Not enough materials");
                    return false;
                }
                else
                {
                    materialToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("Not enough materials");
                return false;
            }
        }
        for (int i = 0; i < materialToRemove.Count; i++)
        {
            RemoveItem(materialToRemove[i].data);
        }
        AddItem(itemToCraft);

        Debug.Log("Item craft succese" + itemToCraft.name);
        return true;
    }
    public List<InventoryItem> GetEquipmentList() => new List<InventoryItem>(equipment);
    public ItemData_Equipment GetEquipement(EquipmentType _type)
    {
        ItemData_Equipment equipmedItem = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> value in equipmentDictionary)
        {
            if (value.Key.equipmentType == _type)
            {
                equipmedItem = value.Key;
            }
        }
        return equipmedItem;
    }
    public void useFlask()
    {
        ItemData_Equipment currentFlask = GetEquipement(EquipmentType.Flask);
        if (currentFlask == null)
        {
            return;
        }
        bool canUseFlask = Time.time > lastTimeUsedFlask + currentFlask.itemCooldown;

        if (canUseFlask)
        {
            currentFlask.ExecuteEffect(null);
            lastTimeUsedFlask = Time.time;
        }
    }
    public bool CanUseArmor()
    {
        ItemData_Equipment armor = GetEquipement(EquipmentType.Armor);
        if (armor)
        {
            return false;
        } 
        if (Time.time > lastTimeUseArmor + armor.itemCooldown)
        {
            lastTimeUseArmor = Time.time;
            return true;
        }
        return false;
    }
}
