namespace KSM.Utility
{
    using System.Collections;
    using UnityEngine;

    public static class MonoBehaviourExtension
    {
        public static Coroutine StopAndStartCoroutine(this MonoBehaviour behaviour, ref IEnumerator refer, IEnumerator target)
        {
            if(refer != null)
            {
                behaviour.StopCoroutine(refer);
            }
            return behaviour.StartCoroutine(refer = target);
        }
    }
}