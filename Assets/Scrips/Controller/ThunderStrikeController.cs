using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    private PlayerStat playerStats;

    void Start()    
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStat>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            EnemyStat enemyStat = collision.GetComponent<EnemyStat>();
            playerStats.DoMagicalDamage(enemyStat);
        }
    }

}
