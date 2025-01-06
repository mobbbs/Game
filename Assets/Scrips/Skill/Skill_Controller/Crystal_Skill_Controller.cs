using System.Linq;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;
    private float crystalDuration;
    private bool canMove;
    private float moveSpeed;
    private bool canExplode;
    private bool canGrow;
    private float growSpeed = 5;
    [SerializeField] private LayerMask whatIsEnemy;
    Transform EnemyTarget;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        cd = GetComponent<CircleCollider2D>();
    }
    public void SetUpCrystal(float crystalDuration, bool canMove, float moveSpeed, bool canExplode, Transform CloestTarget)
    {
        this.crystalDuration = crystalDuration;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
        this.canExplode = canExplode;
        this.EnemyTarget = CloestTarget;
    }

    private void Update()
    {
        crystalDuration -= Time.deltaTime;

        if (crystalDuration < 0)
        {
            FinishCrystal();
        }

        if (canMove && EnemyTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, EnemyTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, EnemyTarget.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    public void ChooseRandomEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, SkillManger.instance.blackhole.getBlackholeRadius(), whatIsEnemy);

        if (colliders.Length > 0) 
        { 
            EnemyTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }
    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Player player = PlayerManager.instance.player;
                player.stats.DoMagicalDamage(hit.GetComponent<EnemyStat>(), -hit.GetComponent<Enemy>().facingDir);
                ItemData_Equipment equipedAmulet = Inventory.Instance.GetEquipement(EquipmentType.Amulet);

                if (equipedAmulet != null)
                {
                    equipedAmulet.ExecuteEffect(hit.transform);
                }
                //hit.GetComponent<Enemy>().Damage(-hit.GetComponent<Enemy>().facingDir);
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}
