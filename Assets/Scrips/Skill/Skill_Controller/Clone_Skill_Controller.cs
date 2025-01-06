using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float colorLosingSpeed;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    private float cloneTimer;
    private Transform ClosestEnemy;
    private bool canDuplicateClone;
    private float chanceToDuplicate;

    private int facingDir;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer -= Time.time;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));

            if (sr.color.a <= 0)
            {
               Destroy(gameObject);
            }
        }
    }
   public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 offset, Transform ClosestEnemy, bool canDuplicate, float chanceToDuplicate)
    {
        if (canAttack)
        {
            anim.SetInteger("AttackNumber", UnityEngine.Random.Range(1, 4));
        }
        transform.position = newTransform.position + offset;
        cloneTimer = cloneDuration;
        this.ClosestEnemy = ClosestEnemy;
        this.canDuplicateClone = canDuplicate;
        this.chanceToDuplicate = chanceToDuplicate;
        facingDir = 1;
        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Player player = PlayerManager.instance.player;
                player.stats.DoPhysicalDamage(hit.GetComponent<EnemyStat>(), facingDir);

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManger.instance.clone.CreateClone(hit.transform, new Vector3(1f * facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {

        if (ClosestEnemy != null)
        {
            if (transform.position.x > ClosestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
