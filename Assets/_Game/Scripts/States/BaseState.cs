using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Inactive,
    Active,
    Background,
}

/// <summary>
/// 
/// </summary>
public abstract class BaseState
{
    public State CurrentState { get; protected set; }
    public abstract void Transition(State toState, BaseState prevStateClass = null, Dictionary<string, object> options = null);
}
