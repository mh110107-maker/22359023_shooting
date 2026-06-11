using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Etc
}

[CreateAssetMenu(fileName = "Item_", menuName = "ShootingGame/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemId;
    public string itemName;
    public ItemType itemType;
    public Sprite icon;
    public int attackBonus;
    public int defenseBonus;
    public bool canStack = true;
    public int maxStack = 99;
}
