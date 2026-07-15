using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Boss1 : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform ground;
    [SerializeField] float moveSpeed, cooldown;
    [SerializeField] Volume volume;
    Health health;
    Rigidbody2D rb;
    SpriteRenderer sr;
    SpriteRenderer[] spriteRenderers;
    float startTime, duration, targetY;
    bool tracker, raged, doneAttacking, stage, end, died;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        startTime = cooldown;
    }
    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime; 
        }
        if (cooldown <= 0 && !tracker)
        {
            cooldown = 0;
            if(health.currentHealth > 4)
            {
                StartCoroutine(Attack());  
                tracker = true;
            }
            if(health.currentHealth <= 4 && end)
            {
                StartCoroutine(Attack());
                tracker = true;
            }
        }
        if(health.currentHealth <= 4 && !raged && doneAttacking)
        {
            StartCoroutine(Stage2()); 
            raged = true;
        }
        if(health.currentHealth <=0 && !died)
        {
            StartCoroutine(Die());
            int unlockedLevel = 5;
            PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
            PlayerPrefs.Save();
            died = true;
        }
    }
    private void FixedUpdate()
    {
        if(!stage && !died)
        {
            Follow(); 
        }
    }
    void Follow()
    {
        Vector2 direction = new Vector2(player.position.x - transform.position.x, 0f).normalized;
        if (Mathf.Abs(player.position.x - transform.position.x) > 0.1f && cooldown > 0)
        {
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            newPosition.x = Mathf.Clamp(newPosition.x, -9f, 9f);
            rb.MovePosition(newPosition);
        } 
    }
    IEnumerator Attack()
    {
        doneAttacking = false;
        float time = 0;
        float startY = transform.position.y;
        if(!raged)
        {
            duration = 0.7f;
            targetY = -1f;
        }
        else
        {
            duration = 0.5f;
            targetY = -8f;
        }
        while (time < (duration * 0.75))
        {
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(startY, targetY, time / (duration * 0.75f)));
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector2(transform.position.x, targetY); 
        health.TakeDamage(1);
        time = 0;
        yield return new WaitForSeconds(0.75f);
        while (time < duration)
        {
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(targetY, startY, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector2(transform.position.x, startY); 
        yield return new WaitForSeconds(0.75f);
        doneAttacking = true;
        tracker = false;
        cooldown = startTime;
    }
    IEnumerator Stage2()
    {
        stage = true;
        float t = 0f;
        float duration = 0.5f;
        float startY = ground.position.y;
        float startY2 = transform.position.y;
        while (t < duration)
        {
            ground.position = new Vector2(ground.position.x, Mathf.Lerp(startY, -10, t / duration));
            t += Time.deltaTime;
            yield return null;
        }
        ground.position = new Vector2(ground.position.x, -10);
        t = 0;
        yield return new WaitForSeconds(1);
        while (t < duration)
        {
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(startY2, -3.310661f, t / duration));
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector2(transform.position.x, -3.310661f); 
        t = 0;
        yield return new WaitForSeconds(0.75f);
        moveSpeed *= 1.3f;
        startTime = 2.25f;
        Color startColor = sr.color; 
        Color targetColor = new Color(0.57f, 0.30f, 0.33f, 1f);
        while (t < (duration + 0.25f))
        {
            if (volume.profile.TryGet(out SplitToning splitToning))
            {
                splitToning.balance.value = Mathf.Lerp(100f, 16f, t / (duration + 0.25f));
            }
            sr.color = Color.Lerp(startColor, targetColor, t / (duration+0.25f));
            t += Time.deltaTime;
            yield return null;
        }
        sr.color = targetColor;
        end = true;
        stage = false;
    }
    IEnumerator Die()
    {
        float duration = 0.25f;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        for (int i = spriteRenderers.Length - 1; i >= 0; i--)
        {
            SpriteRenderer renderer = spriteRenderers[i];
            Color startColor = renderer.color;
            float time = 0f;
            while (time < duration)
            {
                renderer.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0f), time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            renderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        }
        SceneManager.LoadScene("Levels");
    }
}