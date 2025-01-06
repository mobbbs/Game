using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    Enemy enemy;
    public SkeletonDeadState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy; 
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
