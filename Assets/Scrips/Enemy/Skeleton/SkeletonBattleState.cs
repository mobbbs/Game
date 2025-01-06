using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Enemy_Skeleton enemy;
    private Transform player;
    private int moveDir;
    public SkeletonBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDis && CanAttack())
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(enemy.transform.position, player.transform.position) > 7)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
        if (enemy.IsPlayerDetected().distance < enemy.attackDis && !CanAttack())
        {
            moveDir = 0;
        }
        else if (player.position.x > enemy.transform.position.x/* && enemy.IsPlayerDetected().distance > enemy.attackDis*/)
        {
            moveDir = 1;
        }else if (player.position.x < enemy.transform.position.x /*&& enemy.IsPlayerDetected().distance > enemy.attackDis*/)
        {
            moveDir = -1;
        }
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        

    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        return (Time.time > enemy.lastTimeAttack + enemy.attackCooldown);
    }
}
