using System;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int count;

    public InventoryItem(ItemData data, int count)
    {
        this.data = data;
        this.count = count;
    }
}
