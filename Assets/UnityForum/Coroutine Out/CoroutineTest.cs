using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    int x = 1;

    private void Awake()
    {
        StartCoroutine(Increase());
    }

    IEnumerator Increase()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

        while(true)
        {
            ++x;
            yield return waitForSeconds;
        }
    }

    private void Update()
    {
        Debug.Log(x);
    }
}
