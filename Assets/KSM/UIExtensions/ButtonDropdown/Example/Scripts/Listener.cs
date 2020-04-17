namespace KSM.UI.Example
{
    using UnityEngine;
    using UnityEngine.UI;

    public class Listener : MonoBehaviour
    {
        public ButtonDropdown buttonDropdown;

        private void OnEnable()
        {
            buttonDropdown.OnFinishCreateList.AddListener(AddListenerForButtons);
        }
        
        private void AddListenerForButtons()
        {
            int cnt = buttonDropdown.options.Count;

            for(int i = 0; i < cnt; ++i)
            {
                int j = i;
                buttonDropdown.buttons[i].onClick.AddListener(() => Debug.Log("Button " + j + " is clicked!"));
            }
        }

        private void OnDisable()
        {
            buttonDropdown.OnFinishCreateList.RemoveListener(AddListenerForButtons);
        }
    }
}