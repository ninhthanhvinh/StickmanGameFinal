using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateID
{
    Patrol,
    Chase,
    Attack,
    Escape
}

public interface IAIState 
{
    AIStateID GetId();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);

}
