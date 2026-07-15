using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public sealed class Boss2 : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveDuration;
    [SerializeField] GameObject spikes;
    bool doneIntro, attacking, increaseDiff, escape;
    int randomAttack, lastNumber;
    Vector2 position;
    Health health;
    FadeAdv fadeAdvanced;
    FadeMusic fadeMusic;

    void Start()
    {
        position = transform.position;
        StartCoroutine(Intro());
        health= GetComponent<Health>();
        fadeAdvanced = spikes.GetComponent<FadeAdv>();
        fadeMusic = GameObject.FindObjectOfType<FadeMusic>();
    }

    void Update()
    {
        if(doneIntro)
        {
            StartCoroutine(FirstAttack());
            doneIntro = false;
        }
        if (attacking)
        {
            randomAttack = GetRandom(1, 4);
            if (randomAttack == 1)
            {
                StartCoroutine(Intro());
                if (doneIntro)
                {
                    StartCoroutine(FirstAttack());
                    doneIntro = false;
                }
            }
            else if (randomAttack == 2)
            {
                StartCoroutine(SecondAttack());
            }
            else if (randomAttack == 3)
            {
                StartCoroutine(IncreaseSize());
            }
            attacking = false;
        }
        if(health.currentHealth == 3 && !increaseDiff)
        {
            spikes.SetActive(true);
            increaseDiff = true; 
        }
        if(health.currentHealth == 1 && increaseDiff)
        {
            Invoke("Switch", 2f); 
            increaseDiff = false;
        }
        if(health.currentHealth == 0 && !escape)
        {
            StopAllCoroutines();
            attacking = false;
            StartCoroutine(Die());
            fadeMusic.enabled = true;
            escape = true;
        }
    }

    int GetRandom(int min, int max)
    {
        int rand = Random.Range(min, max);
        while (rand == lastNumber)
            rand = Random.Range(min, max);
        lastNumber = rand;
        return rand;
    }

    IEnumerator Intro() 
    {
        yield return new WaitForSeconds(1f);
        float time = 0;
        while(time < 0.75f)
        {
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(position.y, position.y - 6.25f, time / 0.75f));
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector2(transform.position.x, position.y - 6.25f);
        health.TakeDamage(1);
        time = 0;
        yield return new WaitForSeconds(0.65f);
        if (target.position.x > transform.position.x)
        {
            while (time < moveDuration)
            {
                transform.position = new Vector2(Mathf.Lerp(position.x, position.x + 8f, time / moveDuration), transform.position.y);
                time += Time.deltaTime;
                yield return null;
            }
        }
        else if (target.position.x < transform.position.x)
        {
            while (time < moveDuration)
            {
                transform.position = new Vector2(Mathf.Lerp(position.x, position.x - 7.95f, time / moveDuration), transform.position.y);
                time += Time.deltaTime;
                yield return null;
            }
        }
        doneIntro = true;
    }

    IEnumerator FirstAttack()
    {
        float time = 0;
        float startPosX = transform.position.x;
        moveDuration *= 1.7f;
        yield return new WaitForSeconds(0.85f);
        if (transform.position.x < 1)
        {
            while (time < moveDuration)
            {
                transform.position = new Vector2(Mathf.Lerp(startPosX, startPosX + 15.5f, time / moveDuration), transform.position.y);
                time += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.45f);
        }
        else if (transform.position.x > 4)
        {
            while (time < moveDuration)
            {
                transform.position = new Vector2(Mathf.Lerp(startPosX, startPosX - 15.8f, time / moveDuration), transform.position.y);
                time += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.45f);
        }
        time = 0;
        float currentX = transform.position.x;
        float currentY = transform.position.y;
        while (time < 1)
        {
            yield return null;
            transform.position = new Vector2(Mathf.Lerp(currentX, position.x, time / 1),
                                             Mathf.Lerp(currentY, position.y, time / 1));
            time += Time.deltaTime;
        }
        transform.position = position;
        moveDuration /= 1.7f;
        attacking = true;
    }

    IEnumerator SecondAttack()
    {
        float time = 0;
        float duration = 0.9f;
        float startPosY = transform.position.y;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.identity;
        yield return new WaitForSeconds(1f);
        while(time < (duration - 0.2f))
        {
            yield return null;
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(startPosY, startPosY - 2f, time / (duration -0.2f)));
            time+= Time.deltaTime;
        }
        time = 0;
        if (Mathf.Abs(target.position.x - transform.position.x) > 0.3f)
        {
            if (target.position.x < transform.position.x)
            {
                targetRotation = Quaternion.Euler(0f, 0f, -15f);
            }
            else if (target.position.x > transform.position.x)
            {
                targetRotation = Quaternion.Euler(0f, 0f, 15f);
            }
        }
        float startPosX = transform.position.x;
        Vector2 targetPos = target.position;
        float currentY = transform.position.y;
        yield return new WaitForSeconds(0.5f);
        while (time < (duration + 0.15f))
        {
            yield return null;
            float newX = Mathf.Lerp(startPosX, targetPos.x, time / (duration + 0.15f));
            newX = Mathf.Clamp(newX, -6.025f, 6.8f);
            transform.position = new Vector2(newX, Mathf.Lerp(currentY, startPosY - 6f, time / (duration + 0.15f)));
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, time / (duration + 0.15f));
            time += Time.deltaTime;
        }
        health.TakeDamage(1);
        time = 0;
        float currentX = transform.position.x;
        yield return new WaitForSeconds(1f); 
        while (time < duration)
        {
            yield return null;
            transform.position = new Vector2(Mathf.Lerp(currentX, startPosX, time / duration),
                                             Mathf.Lerp(startPosY - 6f, startPosY, time / duration));
            transform.rotation = Quaternion.Lerp(targetRotation, initialRotation ,time / duration);
            time += Time.deltaTime;
        }
        attacking = true;
    }

    IEnumerator IncreaseSize()
    {
        float time = 0;
        float startY = transform.position.y;
        Vector2 initialScale = transform.localScale;
        yield return new WaitForSeconds(0.6f);
        while(time < 0.6f)
        {
            yield return null;
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(startY, startY - 2.15f, time / 0.6f));
            time+= Time.deltaTime;
        }
        time = 0;
        yield return new WaitForSeconds(0.5f);
        while (time <0.65f)
        {
            yield return null;
            transform.localScale = Vector2.Lerp(initialScale, initialScale * 2.15f, time / 0.65f);
            time+= Time.deltaTime;
        }
        time = 0;
        yield return new WaitForSeconds(0.6f);
        while(time < 0.5f)
        {
            yield return null;
            transform.localScale = Vector2.Lerp(initialScale * 2.15f, initialScale, time / 0.5f);
            time += Time.deltaTime;
        }
        time = 0;
        float currentY = transform.position.y;
        yield return new WaitForSeconds(0.5f);
        while (time < 0.6f)
        {
            yield return null;
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(currentY, position.y, time / 0.6f));
            time += Time.deltaTime;
        }
        transform.localScale = new Vector2(initialScale.x, initialScale.y);
        attacking = true;
    }

    IEnumerator Die()
    {
        float time = 0;
        float currentX = transform.position.x;
        float currentY = transform.position.y;
        transform.eulerAngles = new Vector3(0, 0, 0);
        StartCoroutine(fadeAdvanced.FadeEffect());
        while (time < 1.5f)
        {
            yield return null;
            transform.position = new Vector2(Mathf.Lerp(currentX, position.x, time / 1.5f),Mathf.Lerp(currentY, position.y, time / 1.5f));
            time += Time.deltaTime;
        }
        transform.position = new Vector2(position.x, position.y);
        time = 0;
        while (time < 1.5f)
        {
            yield return null;
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(position.y, position.y + 10, time / 1.5f));
            time += Time.deltaTime;
        }
        SceneManager.LoadScene("Levels");
    }

    void Switch()
    {
        spikes.GetComponent<SpikesAdv>().enabled = true;
    }
}