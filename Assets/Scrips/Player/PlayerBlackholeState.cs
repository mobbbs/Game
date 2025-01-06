using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float Flytime = 0.4f;
    private bool skillUsed;
    private float defaultGravity;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = Flytime;
        defaultGravity = rb.gravityScale;
        skillUsed = false;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        rb.gravityScale = defaultGravity;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }
        else
        {
            rb.velocity = new Vector2(0, -0.2f);

            if (!skillUsed)
            {
                skillUsed = true;

                SkillManger.instance.blackhole.CanUseSkill();
            }
        }

        if (player.skill.blackhole.BlackholeFinished())
        {
            stateMachine.ChangeState(player.AirState);
        }
    }
}
