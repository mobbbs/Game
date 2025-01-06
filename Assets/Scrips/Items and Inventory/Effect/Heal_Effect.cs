using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item effect/Heal")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    public override void ExecuteEffect(Transform enemy)
    {
        PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        int healAmount = Mathf.RoundToInt(playerStat.GetMaxHp() * healPercent);
        playerStat.IncreaseHp(healAmount);
    }
}
