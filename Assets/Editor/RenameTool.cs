using UnityEngine;
using UnityEditor;

public sealed class RenameTool 
{
    [MenuItem("Tools/Rename/Ordered")]   
    static void RenameOrdered()
    {
        GameObject parent = Selection.activeGameObject;
        if(parent == null) return;

        for(int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).name = $"{parent.name}{i + 1}";
        }
    }

}