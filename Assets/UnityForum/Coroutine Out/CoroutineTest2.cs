using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest2 : MonoBehaviour
{
    class IntContainer
    {
        public int x;
    }

    private IntContainer container1 = new IntContainer();
    private IntContainer container2 = new IntContainer();

    private void Awake()
    {
        container1.x = 0;
        container2.x = 100;

        StartCoroutine(Increase(container1));
        StartCoroutine(Increase(container2));
    }

    private IEnumerator Increase(IntContainer container)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

        while(true)
        {
            ++container.x;
            yield return waitForSeconds;
        }
    }

    private void Update()
    {
        Debug.Log($"Container1: {container1.x}, Container2: {container2.x}");
    }
}
