using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EscapeState : IAIState
{
    Animator anim;
    NavMeshAgent navAgent;
    Vector3 attackDes;
    bool onEdge;
    readonly float cd = 1.5f;
    float timer;
    public void Enter(AIAgent agent)
    {
        navAgent = agent.nav;
        anim = agent.anim;
        agent.combatSystem.canMove = true;
        navAgent.ResetPath();
        attackDes = agent.attackDest.transform.position;
        onEdge = false;
    }

    public void Exit(AIAgent agent)
    {
    }

    public AIStateID GetId()
    {
        return AIStateID.Escape;
    }

    public void Update(AIAgent agent)
    {
        var dest = (agent.transform.position - attackDes).normalized;
        if (NavMesh.SamplePosition(agent.transform.position + dest, out NavMeshHit hit, .1f, NavMesh.AllAreas))
        {
            navAgent.SetDestination(agent.transform.position + dest * 10);
        }

        else
        {
            float patrolRadius = 30f;
            float offsetX = Random.Range(-patrolRadius, patrolRadius);
            float offsetZ = Random.Range(-patrolRadius, patrolRadius);
            Vector3 chaseDes = new(offsetX, 0, offsetZ);
            if (!onEdge)
                navAgent.ResetPath();
            if (!navAgent.hasPath)
            {
                navAgent.SetDestination(agent.transform.position + chaseDes);
            }
            onEdge = true;
        }

        timer -= Time.deltaTime;
        if (timer <= 0 && IsEnemyInRange(agent, agent.attackRange))
        {
            int random = Random.Range(0, 10);
            if (random <= 2)
            {
                agent.stateMachine.ChangeState(AIStateID.Attack);
            }

            timer = cd;
        }


        anim.SetFloat("Speed", 4);
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
