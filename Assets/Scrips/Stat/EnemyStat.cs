using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStats
{
    Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = 0.4f;

    protected override void Start()
    {
        ApplyLevelModifier();
        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifier()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(maxHp);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(damage);
        Modify(critPower);
        Modify(cirtChance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }

    private void Modify(Stat stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = stat.GetValue() * percentageModifier;
            stat.addModifies(Mathf.RoundToInt(modifier));
        }
    }

    public override void DoPhysicalDamage(CharacterStats targetStat, int AttackDir = -250)
    {
        base.DoPhysicalDamage(targetStat, AttackDir);
    }
    public override void TakeDamage(int _damage, int damageDir)
    {
        //Debug.Log("Enemy " + damageDir);
        base.TakeDamage(_damage, damageDir);
        enemy.Damage(damageDir);
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();
        myDropSystem.GenerateDrop();
    }

}
