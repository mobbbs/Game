using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{

    public EquipmentType equipmentType;
    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Major Stat")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Defensive stat")]
    public int maxHp;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Offensive stat")]
    public int damage;
    public int critPower;
    public int cirtChance;

    [Header("Magic Stat")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;
    private void OnValidate()
    {
        DescriptionLines = 0;
        GenerateTheDescription();
    }
    public void AddModifiers()
    {
        PlayerStat playerstat = PlayerManager.instance.player.GetComponent<PlayerStat>();

        playerstat.strength.addModifies(strength);
        playerstat.agility.addModifies(agility);
        playerstat.intelligence.addModifies(intelligence);
        playerstat.vitality.addModifies(vitality);
        playerstat.maxHp.addModifies(maxHp);
        playerstat.armor.addModifies(armor);
        playerstat.evasion.addModifies(evasion);    
        playerstat.magicResistance.addModifies(magicResistance);
        playerstat.damage.addModifies(damage);
        playerstat.critPower.addModifies(critPower);
        playerstat.fireDamage.addModifies(fireDamage);
        playerstat.iceDamage.addModifies(iceDamage);
        playerstat.lightningDamage.addModifies(lightningDamage);


    }
    public void RemoveModifiers()
    {
        PlayerStat playerstat = PlayerManager.instance.player.GetComponent<PlayerStat>();

        playerstat.strength.removeModifies(strength);
        playerstat.agility.removeModifies(agility);
        playerstat.intelligence.removeModifies(intelligence);
        playerstat.vitality.removeModifies(vitality);
        playerstat.maxHp.removeModifies(maxHp);
        playerstat.armor.removeModifies(armor);
        playerstat.evasion.removeModifies(evasion);
        playerstat.magicResistance.removeModifies(magicResistance);
        playerstat.damage.removeModifies(damage);
        playerstat.critPower.removeModifies(critPower);
        playerstat.fireDamage.removeModifies(fireDamage);
        playerstat.iceDamage.removeModifies(iceDamage);
        playerstat.lightningDamage.removeModifies(lightningDamage);
    }
    public void ExecuteEffect(Transform enemy)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(enemy);
        }
    }
    public override string GetDescription()
    {
        return sb.ToString();
    }
    private void GenerateTheDescription()
    {
        AddItemDescription(strength, "strength");
        AddItemDescription(agility, "agility");
        AddItemDescription(intelligence, "intelligence");
        AddItemDescription(vitality, "vitality");
        AddItemDescription(maxHp, "maxHp");
        AddItemDescription(armor, "armor");
        AddItemDescription(evasion, "evasion");
        AddItemDescription(magicResistance, "magicResistance");
        AddItemDescription(damage, "damage");
        AddItemDescription(critPower, "critPower");
        AddItemDescription(cirtChance, "cirtChance");
        AddItemDescription(fireDamage, "fireDamage");
        AddItemDescription(iceDamage, "iceDamage");
        AddItemDescription(lightningDamage, "lightningDamage");
        for (int i = 0; i < 7 - DescriptionLines; i++)
        {
            sb.AppendLine(" ");
        }
    }
    private void AddItemDescription(int _value, string _name)
    {
        if (_value == 0)
        {
            return;
        }
        DescriptionLines++;
        sb.AppendLine();
        sb.Append(" + " + _value + " " + _name);
    }
}
