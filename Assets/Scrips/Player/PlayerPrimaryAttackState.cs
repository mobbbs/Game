using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int ComboCounter { get; private set; }
    private float lastTimeAttack;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;
        if (ComboCounter > 2 || Time.time >= lastTimeAttack + comboWindow)
        {
            ComboCounter = 0;
        }
        player.anim.SetInteger("ComboCounter", ComboCounter);

        float attackDir = player.facingDir;

        if (xInput != 0)
        {
            attackDir = xInput;
        }

        player.SetVelocity(player.attackMovement[ComboCounter].x * attackDir, player.attackMovement[ComboCounter].y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.15f);
        ComboCounter++;
        lastTimeAttack = Time.time;
        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
