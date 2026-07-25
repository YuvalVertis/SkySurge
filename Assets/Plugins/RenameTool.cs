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


    [MenuItem("Tools/Setup/SceneGroups")]   
    static void SetupSceneGroups()
    {
        GameObject rendering = new GameObject("---Rendering---");
        GameObject entities = new GameObject("---Entities---");
        GameObject environement = new GameObject("---Environement---");
        GameObject managers = new GameObject("---Managers---");
    }
}