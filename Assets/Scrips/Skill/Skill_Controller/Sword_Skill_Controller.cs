using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool canRotate = true;
    private bool isReturning;


    private float returnSpeed;
    private float freezeTimeDuration;

    [Header("Bounce info")]
    private bool isBouncing;
    private float bounceSpeed;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Pierce info")]
    private bool isPierce;
    private int pierceAmount;


    [Header("Spin info")]
    private bool isSpinning;
    private float maxTravelDis;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private float hitCooldown;
    private float hitTimer;
    private float spinDirection;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }
    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player player, float freezeTimeDuration, float returnSpeed)
    {
        this.player = player;
        rb.velocity = dir;
        rb.gravityScale = gravityScale;
        this.freezeTimeDuration = freezeTimeDuration;
        this.returnSpeed = returnSpeed;
        if (isBouncing || isSpinning)
        {
            anim.SetBool("Rotation", true);
        }
        spinDirection = Mathf.Clamp(rb.velocity.x, 0, 1);
    }
    public void SetUpBounce(bool isBouncing, int bounceAmount, float bounceSpeed)
    {
        this.isBouncing = isBouncing;
        this.bounceAmount = bounceAmount;
        this.bounceSpeed = bounceSpeed;
        enemyTarget = new List<Transform>();
    }
    public void SetUpPierce(bool isPierce, int pierceAmount)
    {
        this.isPierce = isPierce;
        this.pierceAmount = pierceAmount;
    }
    public void SetUpSpin(bool isSpinning, float maxTravelDis, float spinDuration, float hitCooldown)
    {
        this.isSpinning = isSpinning;
        this.maxTravelDis = maxTravelDis;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
        if (isBouncing || isSpinning)
        {
            anim.SetBool("Rotation", true);
        }
    }
    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
            return;
        }

        SpinLogic();

        BounceLogic();
    }
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) >= maxTravelDis && !wasStopped)
            {
                StopWhenSpinning();
            }
            if (wasStopped)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 0.15f * Time.deltaTime);
                spinTimer -= Time.deltaTime;
                if (spinTimer < 0)
                {
                    isReturning = true;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            player.stats.DoPhysicalDamage(hit.GetComponent<EnemyStat>(), -hit.GetComponent<Enemy>().facingDir);
                        }
                    }

                }
            }
        }
    }
    private void StopWhenSpinning()
    {
        wasStopped = true;
        spinTimer = spinDuration;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].transform.position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isReturning)
        {
            return;
        }

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetUpTargetsForBounce(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoPhysicalDamage(enemy.GetComponent<EnemyStat>(), player.facingDir);
        enemy.FreezeTimeFor(freezeTimeDuration);

        ItemData_Equipment equipedAmulet = Inventory.Instance.GetEquipement(EquipmentType.Amulet);

        if (equipedAmulet != null)
        {
            equipedAmulet.ExecuteEffect(enemy.transform);
        }
    }

    private void SetUpTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (isSpinning)
        {
            return;
        }
        if (isPierce && pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 1)
        {
            return;
        }

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}