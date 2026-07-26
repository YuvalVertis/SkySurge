using UnityEngine;

public abstract class BossRoot : MonoBehaviour
{
    // Abstract class:
    // All classes that inherit must implement all abstract methods.
    // can't create objects 

    [SerializeField] protected BossStates currentState = BossStates.Intro;
    public BossStates CurrentState => currentState;

    protected float stateTimer;
    protected Health health;

    protected virtual void Start()
    {
        health = GetComponent<Health>();
        if(health != null)
        {
            health.OnDeath += HandleDeath;
        }
    }

    protected virtual void HandleDeath()
    {
        SetState(BossStates.Die);
    }

    public virtual void SetState(BossStates newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        stateTimer = 0;
    }
    
    /// <summary>
    /// Controls states transitions [Must implement].
    /// </summary>
    public abstract void StateLogic();
    
    protected virtual void OnDestroy()
    {
        if (health != null)
        {
            health.OnDeath -= HandleDeath;
        }
    }
}