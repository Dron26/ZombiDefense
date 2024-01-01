using System;
using Animation;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.EnemyAI.States;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStunningState : EnemyState
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _isStunned;
    private EnemyAnimController _enemyAnimController;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemyAnimController = GetComponent<EnemyAnimController>();
    }

    private void BeStunned()
    {
        
        
    }
    protected override void FixedUpdateCustom()
    {
        if (_isStunned==false)
        {
            OnTakeGranadeDamage();
        }
    }
    public void OnWakeUp( )
    {
        _isStunned = false;
        StateMachine.EnterBehavior<EnemySearchTargetState>();
    }

    public override void Disable()
    {
        _isStunned = false;
    }

    public override void OnTakeGranadeDamage()
    {
        if (_isStunned==false)
        {
            if (_agent==null )
            {
                _agent = GetComponent<NavMeshAgent>();
            }
        
            if (_agent.isOnNavMesh)
            {
                _agent.isStopped = true;
            }
            _agent.speed = 0;
            _isStunned = true;
            _animator.SetBool(_enemyAnimController.Walk, false);
            _animator.SetTrigger(_enemyAnimController.GranadeTakeDamage);
        }
    }
}