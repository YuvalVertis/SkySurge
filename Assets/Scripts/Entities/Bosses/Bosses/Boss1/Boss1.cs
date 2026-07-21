using UnityEngine.Rendering.Universal;
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

    [Header("Visuals")]
    [SerializeField] Volume volume;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] Vector2 xRange;

    SpriteRenderer sr;
    Rigidbody2D rb;
    float attackTimer, attackDuration, targetY;
    bool isAttacking, inRageIntro, end, died;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        attackTimer = attackCooldown;
    }

    void FixedUpdate()
    {
        if(!inRageIntro && !died)
        {
            Follow();
        }
    }

    void Update()
    {
        StateLogic();

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime; 
        }

        if (attackTimer <= 0)
        {
            attackTimer = 0;
            if(health.currentHealth > 4)
            {
                SetState(BossStates.Attack);  
            }
            if(health.currentHealth <= 4 && end)
            {
                SetState(BossStates.Attack);
            }
        }

        if(health.currentHealth <= 4 && !inRageIntro && isAttacking)
        {
            SetState(BossStates.Rage);
        }
    }

    public override void StateLogic()
    {
        switch (currentState)
        {
            case BossStates.Idle:
                break;
            case BossStates.Attack:
                Attack();
                break;
            case BossStates.Rage:
                RageAttack();
                break;
            case BossStates.Die:
                Die();
                break;
        }
    }

    void Follow()
    {
        Vector2 direction = new Vector2(target.position.x - transform.position.x, 0f).normalized;
        if (Mathf.Abs(target.position.x - transform.position.x) > 0.1f && attackCooldown > 0)
        {
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.deltaTime;
            newPosition.x = Mathf.Clamp(newPosition.x, xRange.x, xRange.y);
            rb.MovePosition(newPosition);
        } 
    }

    void Attack()
    {
        isAttacking = true;
        float time = 0;
        float startY = transform.position.y;

        if(currentState != BossStates.Rage)
        {
            attackDuration = 0.7f;
            targetY = -1f;
        }
        else
        {
            attackDuration = 0.5f;
            targetY = -8f;
        }

        Sequence.Create()
            .Chain(Tween.PositionY(transform, targetY, attackDuration * 0.75f))
            .OnComplete(() => health.TakeDamage(1))
            .ChainDelay(0.75f)
            .Chain(Tween.PositionY(transform, startY, attackDuration))
            .ChainDelay(0.75f)
            .OnComplete(() =>
            {
                isAttacking = false;
                attackTimer = attackCooldown;
            });
    }

    void RageAttack()
    {
        inRageIntro = true;
        float duration = 0.5f;

        Sequence.Create()
            .Chain(Tween.PositionY(ground.transform, -10, duration))
            .ChainDelay(1f)
            .Chain(Tween.PositionY(transform, -3.31f, duration))
            .ChainDelay(0.75f);

        moveSpeed *= 1.3f;
        attackTimer = 2.25f;

        Color startColor = sr.color; 
        Color targetColor = new Color(0.57f, 0.30f, 0.33f, 1f);

        Sequence sequence = Sequence.Create()
        .Group(EffectsManager.Instance.InterpolateColor(sr, targetColor, duration + 0.25f));

        if (volume.profile.TryGet(out SplitToning splitToning))
        {
            sequence.Group(
                Tween.Custom(100f, 16f, duration + 0.25f, value =>
                {
                    splitToning.balance.value = value;
                })
            );
        }

        sequence.OnComplete(() => inRageIntro = false);
    }

    void Die()
    {
        if (!died)
        {
            int unlockedLevel = 5;
            PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
            PlayerPrefs.Save();
            died = true;
        }

        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        int count = spriteRenderers.Length;
        int finished = 0;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            EffectsManager.Instance.FadeOut(spriteRenderers[i], 0.25f)
                .OnComplete(() =>
                {
                    finished++;
                    if (finished == spriteRenderers.Length)
                    {
                        ScenesHandler.LoadSceneByIndex(Levels.Levels);
                    }
                });
        }
    }
}