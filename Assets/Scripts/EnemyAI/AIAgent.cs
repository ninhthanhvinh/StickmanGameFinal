using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateID initialState;
    public NavMeshAgent nav;
    public Animator anim;
    public float chaseRange;
    public float patrolRange;
    public float attackRange;
    public float cooldownChangeDestination;
    public CombatSystem combatSystem;
    public GameObject attackDest;
    public float health;
    public bool isMoving;

    Vector3 lasPos;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        combatSystem = GetComponent<CombatSystem>();
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new PatrolState());
        stateMachine.RegisterState(new ChaseState());
        stateMachine.RegisterState(new AttackState());
        stateMachine.RegisterState(new EscapeState());
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        health = combatSystem.health;
        Debug.Log(stateMachine.currentState);
        if (!combatSystem.canMove)
        {
            nav.speed = 0f;
        }
        else
            nav.speed = 5f;
        if (Vector3.Distance(transform.position, lasPos) > .01f)
            isMoving = true;
        else
            isMoving = false;
        Debug.Log("is Moving " + isMoving);
        stateMachine.Update();
        lasPos = transform.position;
    }

    public bool DetectInRange(AIAgent agent, float range)
    {
        Collider[] targets = Physics.OverlapSphere(agent.transform.position, range);
        foreach (var target in targets)
        {
            if (target.GetComponent<CombatSystem>() && target.gameObject != agent.gameObject)
            {
                return true;
            }
        }

        return false;
    }

}
