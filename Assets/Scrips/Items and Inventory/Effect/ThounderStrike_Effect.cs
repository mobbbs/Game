using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thounder Strike Effect", menuName = "Data/Item effect/Thunder strike")]
public class ThounderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform enemy)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, enemy.position, Quaternion.identity);
        Destroy(newThunderStrike, 1f);
    }
}
