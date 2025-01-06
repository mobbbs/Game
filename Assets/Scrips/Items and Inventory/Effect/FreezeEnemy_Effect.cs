using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies Effect", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStat playerstats = PlayerManager.instance.player.stats.GetComponent<PlayerStat>();

        if (Inventory.Instance.CanUseArmor() && playerstats.currentHp <= Mathf.RoundToInt(playerstats.maxHp.GetValue() * 0.1f))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

            foreach (var hit in colliders)
            {
                hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);    
            }
        }
    }
}
