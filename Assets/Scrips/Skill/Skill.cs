using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {
    }

    protected virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        Transform ClosestEnemy = null;

        Collider2D[] Colliders = Physics2D.OverlapCircleAll(checkTransform.position, 25);
        float closestDis = Mathf.Infinity;
        foreach (var hit in Colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float disToEnemy = Vector2.Distance(hit.transform.position, checkTransform.position);
                if (disToEnemy < closestDis)                                                
                {
                    closestDis = disToEnemy;
                    ClosestEnemy = hit.transform;
                }
            }
        }

        return ClosestEnemy;
    }
}
