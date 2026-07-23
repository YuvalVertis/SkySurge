using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }


    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale++;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Time.timeScale = 1;
        }
    }

#endif
}