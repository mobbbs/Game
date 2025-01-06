using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header ("Stunned info")]
    public float stunDurtion;
    public Vector2 stunDirection;
    [SerializeField] protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed;
    public float defaultMoveSpeed;
    public float idleTime;
    public float battleTime;
    [HideInInspector] public float lastTimeAttack;

    [Header("Attack info")]
    public float attackDis;
    public float attackCooldown;

    public EnemyStateMachine stateMachine { get; private protected set; }
    public EnemyStat stat { get; private set; }
    public string lastAnimBoolName { get; private set; }
    public virtual void AssignLastAnimName(string AnimBoolName)
    {
        lastAnimBoolName = AnimBoolName;
    }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        stat = GetComponent<EnemyStat>();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
        
    }

    public override void slowEntityBy(float slowPertage, float slowDuration)
    {
        base.slowEntityBy(slowPertage, slowDuration);
        moveSpeed *= (1 - slowPertage);
        anim.speed *= (1 - slowPertage);
        Invoke("ReturnDefautSpeed", slowDuration);
    }
    protected override void ReturnDefautSpeed()
    {
        base.ReturnDefautSpeed();
        moveSpeed = defaultMoveSpeed;
    }

    public virtual void FreezeTime(bool timeFrozen)
    {
        if (timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }
    public virtual void FreezeTimeFor(float seconds) => StartCoroutine(FreezeTimeCorountine(seconds));
    protected virtual IEnumerator FreezeTimeCorountine(float seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);

    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion
    public virtual bool CanBestunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(transform.position, Vector2.right * facingDir, attackDis, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDis * facingDir, transform.position.y));
    }

}
