using UnityEngine;

public abstract class BossRoot : MonoBehaviour
{
    //Abstract class:
    // All classes that inherrit must implement all abstract methods
    // can't create objects 

    [SerializeField] protected BossStates currentState = BossStates.Intro;
    public BossStates CurrentState => currentState;

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

    protected virtual void OnDestroy()
    {
        if (health != null)
        {
            health.OnDeath -= HandleDeath;
        }
    }

    public virtual void SetState(BossStates newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

    public abstract void StateLogic();

    public virtual void Intro() { }
    public virtual void Idle() { }
    public virtual void Chase() { }
    public virtual void Attack() { }
    public virtual void Rage() { }
    public virtual void Flee() { }
    public virtual void Die() { }
}