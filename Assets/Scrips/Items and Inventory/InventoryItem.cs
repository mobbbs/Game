using System;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;
    public InventoryItem(ItemData newdata, int _size = 1)
    {
        data = newdata;
        addStack(_size);
    }

    public void addStack(int _size = 1) => stackSize += _size;
    public void removeStack(int _size = 1) => stackSize -= _size;
}
