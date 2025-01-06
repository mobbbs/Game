using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStat target = hit.GetComponent<EnemyStat>();

                player.stats.DoPhysicalDamage(target);

                Inventory.Instance.GetEquipement(EquipmentType.Weapon)?.ExecuteEffect(target.transform);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManger.instance.sword.CreaterSword();
    }

    private void CatchSword()
    {
        
    }
}
