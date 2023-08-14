using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    public IAIState[] states;
    public AIAgent agent;
    public AIStateID currentState;

    public AIStateMachine(AIAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AIStateID)).Length;
        states = new IAIState[numStates];
    }

    public void RegisterState(IAIState state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }

    public IAIState GetState(AIStateID stateID)
    {
        int index = (int)stateID;
        return states[index];
    }
    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(AIStateID newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
