using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStats
{
    Player player;
    PlayerItemDrop itemDrop;
    protected override void Start()
    {
        base.Start();
        player = PlayerManager.instance.player;
        itemDrop = GetComponent<PlayerItemDrop>();
    }

    public override void DoPhysicalDamage(CharacterStats targetStat, int AttackDir = -250)
    {
        base.DoPhysicalDamage(targetStat, AttackDir);
    }
    public override void TakeDamage(int _damage, int damageDir)
    {
        base.TakeDamage(_damage, damageDir);
        player.Damage(damageDir);
    }
    protected override void DecreaseHealthBy(int damage)
    {
        base.DecreaseHealthBy(damage);

        ItemData_Equipment currenArmor = Inventory.Instance.GetEquipement(EquipmentType.Armor);
        if (currenArmor != null)
        {
            currenArmor.ExecuteEffect(player.transform);
        }
    }
    protected override void Die()
    {
        base.Die();
        player.Die();
        itemDrop.GenerateDrop();
    }
}
