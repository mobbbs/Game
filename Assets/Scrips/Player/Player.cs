    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce;
    public float swordReturnImpact = 3.25f;
    public float defautMoveSpeed;
    public float defautJumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float defautDashSpeed;
    public float dashDir { get; private set; }
    public SkillManger skill { get; private set; }
    public GameObject sword { get; private set; }
    public CharacterStats stats { get; private set; }
    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerWallSlideState WallSlide { get; private set; }
    public PlayerDashState DashState {get; private set; }
    public PlayerWallJumpState WallJump { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttack { get; private set; }
    public PlayerCounterAttackState CounterAttack { get; private set; }
    public PlayerAimSwordState AimSword { get; private set; }
    public PlayerCatchSwordState CatchSword { get; private set; }
    public PlayerBlackholeState Blackhole { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        stats = GetComponent<CharacterStats>();

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState  = new PlayerAirState(this, StateMachine, "Jump");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        WallSlide = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        WallJump = new PlayerWallJumpState(this, StateMachine, "Jump");
        PrimaryAttack = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
        CounterAttack = new PlayerCounterAttackState(this, StateMachine, "CounterAttack");
        AimSword = new PlayerAimSwordState(this, StateMachine, "AimSword");
        CatchSword = new PlayerCatchSwordState(this, StateMachine, "CatchSword");
        Blackhole = new PlayerBlackholeState(this, StateMachine, "Jump");
        DeadState = new PlayerDeadState(this, StateMachine, "Dead");
    }
    protected override void Start()
    {
        base.Start();
        skill = SkillManger.instance;
        StateMachine.Initialize(IdleState);
        defautMoveSpeed = moveSpeed;
        defautJumpForce = jumpForce;
        defautDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F))
        {
            skill.crystal.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.Instance.useFlask();
        }
    }

    public override void slowEntityBy(float slowPertage, float slowDuration)
    {
        base.slowEntityBy(slowPertage, slowDuration);
        dashSpeed *= (1 - slowPertage);
        jumpForce *= (1 - slowPertage);
        moveSpeed *= (1 - slowPertage);
        anim.speed *= (1 - slowPertage);
        Invoke("ReturnDefautSpeed", slowDuration);
    }
    protected override void ReturnDefautSpeed()
    {
        base.ReturnDefautSpeed();
        dashSpeed = defautDashSpeed;
        moveSpeed = defautMoveSpeed;
        jumpForce = defautJumpForce;
    }
    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }
    public void CatchTheSword()
    {
        StateMachine.ChangeState(CatchSword);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _second)
    {
        isBusy = true;
         
        yield return new WaitForSeconds(_second);

        isBusy = false;
    }

    public void AnimationTrigger() => StateMachine.currentState.AnimationFinishTrigger();
    private void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManger.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            StateMachine.ChangeState(DashState);
            dashUsageTimer = dashCooldown;
        }
    }

    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(DeadState);
    }
}
