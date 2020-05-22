using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
public class HierarchyHelper : MonoBehaviour
{
    private static HierarchyHelper instance;

    public static HierarchyHelper Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<HierarchyHelper>();
                if(instance == null)
                {
                    //Debug.LogError("Cannot find " + nameof(HierarchyHelper) + " in current Scene!");
                }
            }
            return instance;
        }
    }

    public List<GameObject> hiddenObjects = new List<GameObject>();
    
    private void Awake()
    {
        Destroy(this);
    }    
}
#endif
