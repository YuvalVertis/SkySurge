using UnityEngine;

public sealed class SawHandler : MonoBehaviour
{
    [SerializeField] GameObject[] saws;

    public void HandleSaws(int index)
    {
        if (saws[index] == null || saws.Length == 0 || EffectsManager.Instance == null) return;

        EffectsManager.Instance.FadeOut(saws[index].GetComponentInChildren<SpriteRenderer>(), 0.75f, false, ChangeActiveState.ChangeInParent);

        int nextIndex = index + 2;
        if(nextIndex < saws.Length)
        {
            saws[nextIndex].SetActive(true);
        }
    }

}