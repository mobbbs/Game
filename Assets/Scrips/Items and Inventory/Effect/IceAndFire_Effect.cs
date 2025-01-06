using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice And Fire Effect", menuName = "Data/Item effect/Ice And Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject IceAndFirePrefab;
    [SerializeField] private Vector2 newVelocity;
    public override void ExecuteEffect(Transform respondPosition)
    {

        Transform player = PlayerManager.instance.player.transform;
        bool thirdAttack = player.GetComponent<Player>().PrimaryAttack.ComboCounter == 2;
        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(IceAndFirePrefab, player.position, player.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(newVelocity.x * PlayerManager.instance.player.facingDir, newVelocity.y);
            Destroy(newIceAndFire, 10f);
        }

    }
}
