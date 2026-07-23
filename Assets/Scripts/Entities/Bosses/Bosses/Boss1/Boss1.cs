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

    [Header("Features")]
    [SerializeField] RandomSpawn maceSpawner;
    [SerializeField] float rageEnteryHp;

    bool inRage, inAttack, hasDied;
    List<SpriteRenderer> sprites = new();
    EffectsManager instance = EffectsManager.Instance;
    Rigidbody2D rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

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

        if (currentState == BossStates.Chase)
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
        .Chain(Tween.PositionY(transform, targetY, attackDuration * 0.75f))
        .ChainCallback(() => health.TakeDamage(1), warnIfTargetDestroyed: false)
        .ChainDelay(0.75f)
        .Chain(Tween.PositionY(transform, startY, attackDuration))
        .ChainDelay(0.75f)
        .OnComplete(() =>
        {
            if (health.currentHealth <= rageEnteryHp && !inRage)
            {
                SetState(BossStates.Rage);
            }
            if (health.currentHealth > rageEnteryHp || inRage)
            {
                SetState(BossStates.Chase);
            }
            inAttack = false;
        }, warnIfTargetDestroyed: false);
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
        .Chain(Tween.PositionY(transform, -3.31f, duration * 1.6f))
        .ChainDelay(1.5f)
        .Chain(instance.ChangeColor(sprites[0], targetColor, duration + 0.6f));

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
        FadeEye(ref sequence);

        sequence.OnComplete(() =>
        {
            moveSpeed *= 1.25f;
            attackCooldown *= 0.8f;
            maceSpawner.spawn = true;
            SetState(BossStates.Chase);
        }, warnIfTargetDestroyed: false);
    }

    void FadeEye(ref Sequence sequence)
    {
        sequence
        .Chain(instance.Fade(sprites[1], 1f, 0.2f))
        .Chain(instance.ChangeColor(sprites[1], new Color(0f, 0f, 0f, 0.2f), 0.2f));
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
        FadeEye(ref sequence);
        
        if(sprites.Count > 0)
        {
            for (int i = sprites.Count - 1; i >= 0; i--)
            {
                 sequence.Chain(EffectsManager.Instance.FadeOut(sprites[i], 0.25f));
            }
        }

        sequence.OnComplete(() =>
        {
            ScenesHandler.LoadSceneByIndex(Levels.Levels);
        }, warnIfTargetDestroyed: false);
    }
}