using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Clone Instead of crystal")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canMultiStack;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalList = new List<GameObject>();


    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
            {
                return;
            }

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManger.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }

        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller crystalSkillScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        crystalSkillScript.SetUpCrystal(crystalDuration, canMoveToEnemy, moveSpeed, canExplode, FindClosestEnemy(currentCrystal.transform));
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if (canMultiStack)
        {
            if (crystalList.Count > 0)
            {
                if (crystalList.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }
                cooldown = 0;
                GameObject crystalToSpawn = crystalList[crystalList.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                crystalList.Remove(crystalToSpawn);
                newCrystal.GetComponent<Crystal_Skill_Controller>().SetUpCrystal(crystalDuration, canMoveToEnemy, moveSpeed, canExplode, FindClosestEnemy(newCrystal.transform));
                if (crystalList.Count == 0)
                {
                    RefillCrystal();
                    cooldown = multiStackCooldown;
                }
            }
            return true;
        }
        return false;
    }

    private void RefillCrystal()
    {

        while (crystalList.Count < amountOfStacks) 
        {
            crystalList.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
        {
            return;
        }

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
