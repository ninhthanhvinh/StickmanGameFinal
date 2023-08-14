using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IAIState
{
    Animator anim;
    NavMeshAgent navAgent;
    float chaseRadius;
    float attackRadius;
    float dist;
    int index;
    float checkTargetCD;
    float timer;

    public void Enter(AIAgent agent)
    {
        navAgent = agent.nav;
        anim = agent.anim;
        chaseRadius = agent.chaseRange;
        attackRadius = agent.attackRange;
        dist = chaseRadius;
        checkTargetCD = agent.cooldownChangeDestination;
    }

    public void Exit(AIAgent agent)
    {
        anim.SetFloat("Speed", 0f);
        dist = chaseRadius;
    }

    public AIStateID GetId()
    {
        return AIStateID.Chase;
    }

    public void Update(AIAgent agent)
    {
        bool hasEnemies = false;
        Collider[] enemies = Physics.OverlapSphere(agent.transform.position, chaseRadius);
        if (timer >= 0f)
        {
            //Check if any enemies in chase range
            
            for (int i = 0; i <enemies.Length; i++)
            {
                if (enemies[i].GetComponent<CombatSystem>() && 
                    enemies[i].transform.position != agent.transform.position)
                {
                    if (enemies[i].CompareTag("Player"))
                    {
                        index = i;
                        break;
                    }

                    //Make sure chase the nearest enemy
                    if (Vector3.Distance(agent.transform.position, enemies[i].transform.position) < dist)
                    {
                        dist = Vector3.Distance(agent.transform.position, enemies[i].transform.position);
                        index = i;
                    }
                    hasEnemies = true;
                }
            }
            Debug.Log(agent.name + " has enemies in chase range: " + hasEnemies);
            if (!hasEnemies)
            {
                agent.stateMachine.ChangeState(AIStateID.Patrol);
            }
            if (index < enemies.Length)
            {
                navAgent.SetDestination(enemies[index].transform.position);
                anim.SetFloat("Speed", 1);
            }            
            Collider[] canAttack = Physics.OverlapSphere(agent.transform.position, attackRadius);
            for (int i = 0; i < canAttack.Length; i++)
            {
                if (canAttack[i].GetComponent<CombatSystem>() &&
                    canAttack[i].transform.position != agent.transform.position)
                { 
                    agent.attackDest = canAttack[i].gameObject;
                    navAgent.SetDestination(agent.transform.position);
                    agent.stateMachine.ChangeState(AIStateID.Attack);
                }
            }
            timer = checkTargetCD;
        }

        timer -= Time.deltaTime;

        
    }
   

}
