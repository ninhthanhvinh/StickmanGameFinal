using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IAIState
{
    NavMeshAgent navAgent;
    CombatSystem cb;
    float timer;
    float attackCD;
    float attackRadius;
    int index;
    public void Enter(AIAgent agent)
    {
        navAgent = agent.nav;
        cb = agent.combatSystem;
        attackCD = 0.35f;
        attackRadius = agent.attackRange;
    }

    public void Exit(AIAgent agent)
    {
    }

    public AIStateID GetId()
    {
        return AIStateID.Attack;
    }

    public void Update(AIAgent agent)
    {
        if(!navAgent.hasPath)
            navAgent.SetDestination(agent.transform.position);
        if (timer <= 0)
        {
            cb.Attack();
            timer = attackCD;
        }
        timer -= Time.deltaTime;

        //UpdateAttackDes(agent);

        bool enemyInRange = false;
        Collider[] canAttack = Physics.OverlapSphere(agent.transform.position, attackRadius);
        for (int i = 0; i < canAttack.Length; i++)
        {
            if (canAttack[i].GetComponent<CombatSystem>() &&
                canAttack[i].transform.position != agent.transform.position)
            {
                enemyInRange = true;
            }
        }

        if (!enemyInRange)
        {
            agent.stateMachine.ChangeState(AIStateID.Chase);
        }

        //Make enemies look at their attack target
        if (agent.attackDest)
        {
            agent.transform.LookAt(agent.attackDest.transform, Vector3.up);
            if (agent.health < agent.attackDest.GetComponent<CombatSystem>().health &&
                agent.health < 0.3 * agent.combatSystem.maxHealth)
            {
                agent.stateMachine.ChangeState(AIStateID.Escape);
            }
        }



    }

    //void UpdateAttackDes(AIAgent agent)
    //{
    //    if (Vector3.Distance(agent.attackDest.transform.position, agent.transform.position) > agent.attackRange)
    //    {
    //        float minDis = 999999;
    //        Collider[] canAttack = Physics.OverlapSphere(agent.transform.position, attackRadius);
    //        for (int i = 0; i < canAttack.Length; i++)
    //        {
    //            if (canAttack[i].GetComponent<CombatSystem>() &&
    //                canAttack[i].transform.position != agent.transform.position)
    //            {
    //                if (minDis > Vector3.Distance(canAttack[i].transform.position, agent.transform.position))
    //                    index = i;
    //            }
    //        }
    //        agent.attackDest = canAttack[index].gameObject;
    //    }
    //}
}
