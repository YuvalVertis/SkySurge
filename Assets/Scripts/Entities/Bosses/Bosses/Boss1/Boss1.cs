using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using PrimeTween;

public class Boss1 : BossRoot
{
    [Header("Movement")]
    [SerializeField] Transform target;
    [SerializeField] Transform ground;
    [SerializeField] float moveSpeed;
    [SerializeField] float attackCooldown;
    [SerializeField] Vector2 walkRangeX;
    
    [Header("Visuals")]
    [SerializeField] Volume volume;
    [SerializeField] Camera renderCamera;
    [SerializeField] float shakeStrength;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeFrequency;
    [SerializeField] GameObject particleObj;

    [Header("Features")]
    [SerializeField] RandomSpawn maceSpawner;
    [SerializeField] float rageEnteryHp;

    bool inRage, inAttack, hasDied;
    List<SpriteRenderer> sprites = new();
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponentsInChildren<SpriteRenderer>(sprites);
        health = GetComponent<Health>();
    }

    void Update()
    {
        stateTimer += Time.deltaTime;
        StateLogic();
    }

    void FixedUpdate()
    {
        if (target == null || rb == null) return;

        if (currentState == BossStates.Chase && !hasDied)
        {
            Chase();
        }
    }

    public override void StateLogic()
    {
        switch (currentState)
        {
            case BossStates.Attack:
                Attack();
                break;
            case BossStates.Rage:
                RageIntro();
                break;
            case BossStates.Die:
                Die();
                break;
        }
    }

    void Chase()
    {
        float distance = target.position.x - transform.position.x;
        if (stateTimer < attackCooldown)
        {
            if (Mathf.Abs(distance) > 0.05f)
            {
                float direction = Mathf.Sign(distance);
                float newPos = rb.position.x + direction * moveSpeed * Time.deltaTime;

                newPos = Mathf.Clamp(newPos, walkRangeX.x, walkRangeX.y);
                rb.MovePosition(new Vector2(newPos, rb.position.y));
            }
        }
        else
        {
            SetState(BossStates.Attack);
        }
    }

    void Attack()
    {
        if (inAttack) return;
        inAttack = true;

        float startY = transform.position.y;
        float targetY;
        float attackDuration;

        if (!inRage)
        {
            attackDuration = 0.7f;
            targetY = -1f;
        }
        else
        {
            attackDuration = 0.6f;
            targetY = -8f;
        }

        Sequence.Create()
        .Chain(Tween.PositionY(transform, targetY, attackDuration * 0.9f, Ease.InSine))
        .ChainCallback(() => 
        {
            health.TakeDamage(1);
            EffectsManager.Instance.CameraShake(renderCamera, shakeStrength, shakeDuration, shakeFrequency);
            ParticlesHandler.Play(particleObj);
        }, warnIfTargetDestroyed: false)
        .ChainDelay(1f)
        .Chain(Tween.PositionY(transform, startY, attackDuration * 1.1f, Ease.OutSine))
        .ChainDelay(1f)
        .OnComplete(() =>
        {
            if (health.currentHealth <= rageEnteryHp && !inRage)
            {
                SetState(BossStates.Rage);
            }
            else
            {
                SetState(BossStates.Chase);
            }
            inAttack = false;
        }, warnIfTargetDestroyed: false);
    }

    void FadeEye(ref Sequence sequence, int index)
    {
        sequence
        .Chain(EffectsManager.Instance.Fade(sprites[index], 1f, 0.25f))
        .Chain(EffectsManager.Instance.ChangeColor(sprites[index], new Color(0f, 0f, 0f, 0.15f), 0.6f));
    }
    
    void RageIntro()
    {
        if (inRage) return;

        inRage = true;
        float duration = 0.5f;
        Color targetColor;
        ColorUtility.TryParseHtmlString("#8d5b5f", out targetColor);

        var sequence = Sequence.Create()
        .Chain(Tween.PositionY(ground.transform, -10, duration))
        .ChainDelay(1f)
        .Chain(Tween.PositionY(transform, -3.31f, duration * 1.6f, Ease.OutSine))
        .ChainDelay(1.5f)
        .Chain(EffectsManager.Instance.ChangeColor(sprites[0], targetColor, duration + 0.6f));

        if (volume.profile.TryGet(out SplitToning splitToning))
        {
            sequence.Group(
                Tween.Custom(100f, 50f, duration + 0.6f, value =>
                {
                    splitToning.balance.value = value;
                }, Ease.InOutSine)
            );
        }

        sequence.ChainDelay(0.5f);
        FadeEye(ref sequence, 1);

        sequence.OnComplete(() =>
        {
            moveSpeed *= 1.25f;
            shakeStrength *= 1.15f;
            shakeFrequency *= 1.1f;
            attackCooldown *= 0.8f;
            maceSpawner.spawn = true;
            SetState(BossStates.Chase);
        }, warnIfTargetDestroyed: false);
    }


    void Die()
    {
        if (hasDied) return;
        hasDied = true;
        maceSpawner.spawn = false;

        //Unlock next level
        ScenesHandler.NextLevel();
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        Sequence sequence = Sequence.Create();
        FadeEye(ref sequence, 2);
        
        if(sprites.Count > 0)
        {
            for (int i = sprites.Count - 1; i >= 0; i--)
            {
                 sequence.Chain(EffectsManager.Instance.FadeOut(sprites[i], 0.225f));
            }
        }

        sequence.OnComplete(() =>
        {
            ScenesHandler.LoadSceneByIndex(Levels.Levels);
        }, warnIfTargetDestroyed: false);
    }
}