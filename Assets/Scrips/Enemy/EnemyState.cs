using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{

    protected EnemyStateMachine stateMachine;
    protected Rigidbody2D rb;
    protected Enemy enemyBase;

    protected float stateTimer;

    private string animBoolName;
    protected bool triggerCalled;


    public EnemyState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemyBase = enemyBase;
        this.animBoolName = animBoolName;
    }
    
    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationTrigger()
    {
        triggerCalled = true;
    }
}
