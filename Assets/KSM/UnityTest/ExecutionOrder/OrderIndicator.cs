using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderIndicator : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log(name + " " + GetInstanceID());

        Debug.Log(name + " Awake");
    }

    private void OnEnable()
    {
        Debug.Log(name + " OnEnable");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(name + " Start");
    }
}
