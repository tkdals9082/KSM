namespace KSM.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using MyBox;
    using System.Linq;
    using KSM.Utility;

    public abstract class AbstractGraph : MonoBehaviour
    {
        #region define struct

        [System.Serializable]
        public struct Padding
        {
            public float offsetTop;
            public float offsetBottom;
            public float offsetLeft;
            public float offsetRight;

            [Tooltip("Space between each values")]
            public float spacing;
        }

        [System.Serializable]
        public struct ValueSetting
        {
            public float minValue;
            public float maxValue;

            public string unit;
        }

        [System.Serializable]
        public struct ColorSetting
        {
            public Color graphColor;
            public Color valueColor;
        }

        [System.Serializable]
        public struct RuntimeSetting
        {
            [Header("Graph Size")]
            [ReadOnly] public float graphWidth;
            [ReadOnly] public float graphHeight;

            [Header("Value to Rect Transition")]
            [ReadOnly] public float valueToRectRatio;
            [ReadOnly] public float valueToRectOffset;
        }
        #endregion

        public Padding padding;
        public ValueSetting valueSetting;
        public ColorSetting colorSetting;
        public RuntimeSetting runtimeSetting;

        [SerializeField] protected List<float> values;

        public RectTransform rectTf;
        public Rect rect;

#if UNITY_EDITOR
        private void Reset()
        {
            rectTf = GetComponent<RectTransform>();
            if (rectTf == null)
            {
                Debug.LogError("Please add this component to gameObject with RectTransform!!");
                DestroyImmediate(this);
            }
        }
#endif
        private void Awake()
        {
            rect = rectTf.rect;
            StartCoroutine(Initialize());
        }

        /// <summary>
        /// To ensure that graph runtimeSetting is initialized,
        /// override not Awake() but this function to customize your graph.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator Initialize()
        {
            yield return null;

            runtimeSetting.graphHeight = rect.height - padding.offsetTop - padding.offsetBottom;
            runtimeSetting.graphWidth = rect.width - padding.offsetLeft - padding.offsetRight;

            runtimeSetting.valueToRectRatio = (valueSetting.maxValue - valueSetting.minValue) == 0 ? 0 : runtimeSetting.graphHeight / (valueSetting.maxValue - valueSetting.minValue);
            runtimeSetting.valueToRectOffset = padding.offsetTop - valueSetting.maxValue * runtimeSetting.valueToRectRatio;

            yield break;
        }

        public void AddValue(float value)
        {
            AddValueWithLabel(("", value));
        }

        public void AddValues(IList<float> values)
        {
            AddValuesWithLabel(values.Select(v => ("", v)));
        }

        public virtual void AddValueWithLabel((string label, float value) p)
        {
            values.Add(p.value);
        }

        public virtual void AddValuesWithLabel(IEnumerable<(string label, float value)> ps)
        {
            values.AddRange(ps.Select(pair => pair.value));
        }
    }
}