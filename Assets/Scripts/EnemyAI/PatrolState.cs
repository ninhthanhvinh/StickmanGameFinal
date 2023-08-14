using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IAIState
{
    Animator anim;
    NavMeshAgent navAgent;
    float patrolRadius;
    float chaseRadius;
    float checkTargetCD;
    float timer;


    public void Enter(AIAgent agent)
    {
        navAgent = agent.nav;
        anim = agent.anim;
        patrolRadius = agent.patrolRange;
        chaseRadius = agent.chaseRange;
        checkTargetCD = agent.cooldownChangeDestination;
    }

    public void Exit(AIAgent agent)
    {
    }

    public AIStateID GetId()
    {
        return AIStateID.Patrol;
    }

    public void Update(AIAgent agent)
    {
        float offsetX = Random.Range(-patrolRadius, patrolRadius);
        float offsetZ = Random.Range(-patrolRadius, patrolRadius);
        Vector3 chaseDes = new(offsetX, 0, offsetZ);
        if (!navAgent.hasPath)
        {
            navAgent.SetDestination(agent.transform.position + chaseDes);
        }
        anim.SetFloat("Speed", 1);
        if (timer <= 0f)
        {
            if (IsEnemyInRange(agent, chaseRadius))
            {
                Debug.Log(IsEnemyInRange(agent, chaseRadius));
                agent.stateMachine.ChangeState(AIStateID.Chase);
            }
            timer = checkTargetCD;
        }
        timer -= Time.deltaTime;
    }
    bool IsEnemyInRange(AIAgent agent, float _chaseRadius)
    {
        Collider[] colliders = Physics.OverlapSphere(agent.transform.position, _chaseRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<CombatSystem>() && collider.gameObject != agent.gameObject)
            {
                return true;
            }

        }
        return false;
    }
}
