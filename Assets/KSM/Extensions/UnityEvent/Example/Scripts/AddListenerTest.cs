namespace KSM.Utility.Example
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class AddListenerTest : MonoBehaviour
    {
        public Button button;

        // Start is called before the first frame update
        void Start()
        {
            UnityEventLambda.AddListener(button.onClick, () => Test());
        }

        private void Test()
        {
            Debug.Log("hi");
            UnityEventLambda.RemoveListener(button.onClick, () => Test());
        }
    }
}