using UnityEngine;

public sealed class CordActions : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject disableObject;
    [SerializeField] float actionCordY;

    private void Update()
    {
        if(target.position.y >= actionCordY && disableObject.activeSelf)
        {
            disableObject.SetActive(false);
        }
    }

}
