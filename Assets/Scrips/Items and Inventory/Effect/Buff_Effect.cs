using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item effect/Buff")]
public class Buff_Effect : ItemEffect
{
    PlayerStat stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;

    public override void ExecuteEffect(Transform enemy)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStat>();
        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetStat(buffType));
    }
}
