namespace KSM.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI.Extensions;
    using MyBox;
    using System.Linq;
    using KSM.Utility;
    using UnityEngine.UI;

    public class LineGraph : AbstractGraph
    {
        UILineRenderer lineRenderer;

        public bool hasMarkImg;
        [ConditionalField(nameof(hasMarkImg))] public Image markImgPrefab;

        public bool hasLabelText;
        public bool hasValueText;

        public float lineThickness = 6;

        protected override IEnumerator Initialize()
        {
            yield return base.Initialize();

            var obj = new GameObject(nameof(UILineRenderer), typeof(UILineRenderer));
            obj.transform.SetParent(transform, false);
            obj.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
            obj.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            lineRenderer = obj.GetComponent<UILineRenderer>();
            lineRenderer.color = colorSetting.graphColor;
            SetLineThickness(lineThickness);
        }

        public override void AddValueWithLabel((string label, float value) p)
        {
            AddValuesWithLabel(new List<(string, float)>() { (p.label, p.value) });
        }

        public override void AddValuesWithLabel(IEnumerable<(string label, float value)> ps)
        {
            base.AddValuesWithLabel(ps);

            int cnt = ps.Count();
            int totalCnt = values.Count;
            int prevCnt = totalCnt - cnt;

            Debug.Log(cnt + ", " + totalCnt);

            if(cnt == 0)
                return;
            rectTf.pivot = new Vector2(0, 0.5f);
            float rectWidth = padding.spacing * (totalCnt- 1) + (padding.offsetLeft + padding.offsetRight);
            rectTf.sizeDelta = new Vector2(rectWidth, rect.height);
            //rectTf.anchoredPosition = new Vector2(rectWidth * 0.5f, 0);

            (float min, float max) = MathUtil.MinMax(ps.Select(pair => pair.value));

            float startX = padding.offsetLeft;

            List<Vector2> points = new List<Vector2>();
           
            for (int i = prevCnt; i < totalCnt; ++i)
            {
                Debug.Log(i);
                float posX = startX + padding.spacing * i;
                float valuePosY = (values[i]) * runtimeSetting.valueToRectRatio + runtimeSetting.valueToRectOffset;

                points.Add(new Vector2(posX, valuePosY));
            }

            if (lineRenderer.Points == null)
            {
                lineRenderer.Points = points.ToArray();
            }
            else
            {
                lineRenderer.Points = lineRenderer.Points.Concat(points).ToArray();
            }
        }

        public void SetLineThickness(float thickness)
        {
            lineRenderer.lineThickness = thickness;
        }

        [ButtonMethod]
        public void Test()
        {
            AddValues(new List<float>() { 100, 200, 150});
        }
    }
}