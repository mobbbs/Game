using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool CreateCloneOnDashStart;
    [SerializeField] private bool CreateCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;
    [SerializeField] private float delayOfCounterAttack;

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicate;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;
    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManger.instance.crystal.CreateCrystal();
            SkillManger.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab, clonePosition.position, Quaternion.identity);
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(clonePosition, cloneDuration, canAttack, offset, FindClosestEnemy(newClone.transform), canDuplicate, chanceToDuplicate);
    }

    public void cloneOnDashStart()
    {
        if (CreateCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void cloneOnDashOver()
    {
        if (CreateCloneOnDashOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }
    public void cloneOnCounterAttack(Transform enemy)
    {
        if (canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(enemy.transform, new Vector3(1.8f * player.facingDir, 0), delayOfCounterAttack));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform transform, Vector3 offset, float second)
    {
        yield return new WaitForSeconds(second);
        CreateClone(transform, offset);
    }
}
