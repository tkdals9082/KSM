using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSM.Utility
{
    using UnityEngine.UI;

    public enum Interpolation
    {
        NearestNeighbor,
        InverseDistanceWeighted,
        Average,
    }

    public static class UIExtensions
    {
        #region Texture2D

        /// <summary>
        /// Texture 2D resize.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destSize"></param>
        /// <param name="interpMethod">Interpolation Method</param>
        /// <returns></returns>
        public static Texture2D ResizeTexture2D(this Texture2D source, Vector2 destSize, Interpolation interpMethod)
        {
            Color[] sourceColors = source.GetPixels(0);
            Vector2 sourceSize = new Vector2(source.width, source.height);

            float destWidth = destSize.x;
            float destHeight = destSize.y;

            Texture2D newTex = new Texture2D((int)destWidth, (int)destHeight, TextureFormat.RGBA32, false);

            int len = (int)destWidth * (int)destHeight;
            Color[] destColors = new Color[len];

            if (interpMethod == Interpolation.Average)
            {
                Vector2 pixelSize = new Vector2(sourceSize.x / destWidth, sourceSize.y / destHeight);

                Vector2 center = new Vector2();

                float x, y;
                int xFrom, xTo, yFrom, yTo;
                float gridCount;
                Color colorTemp;

                for (int i = 0; i < len; i++)
                {
                    x = (float)i % destWidth;
                    y = Mathf.Floor((float)i / destWidth);

                    center.x = (x / destWidth) * sourceSize.x;
                    center.y = (y / destHeight) * sourceSize.y;

                    xFrom = (int)Mathf.Max(Mathf.Floor(center.x - (pixelSize.x * 0.5f)), 0);
                    xTo = (int)Mathf.Min(Mathf.Ceil(center.x + (pixelSize.x * 0.5f)), sourceSize.x);
                    yFrom = (int)Mathf.Max(Mathf.Floor(center.y - (pixelSize.y * 0.5f)), 0);
                    yTo = (int)Mathf.Min(Mathf.Ceil(center.y + (pixelSize.y * 0.5f)), sourceSize.y);

                    colorTemp = new Color();

                    gridCount = 0;
                    for (int yy = yFrom; yy < yTo; yy++)
                    {
                        for (int xx = xFrom; xx < xTo; xx++)
                        {
                            colorTemp += sourceColors[(int)(((float)yy * sourceSize.x) + xx)];

                            gridCount++;
                        }
                    }

                    // Average
                    destColors[i] = colorTemp / (float)gridCount;
                }
            }
            else if (interpMethod == Interpolation.NearestNeighbor)
            {
                int width = (int)destWidth;
                int height = (int)destHeight;

                float coef_widthDestToSource = sourceSize.x / destSize.x;
                float coef_heigthDestToSource = sourceSize.y / destSize.y;

                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        destColors[x + y * width] = sourceColors[(int)(x * coef_widthDestToSource) + (int)(y * coef_heigthDestToSource) * (int)sourceSize.x];
                    }
                }
            }
            // IDW for adjacent 4 pixels
            else if (interpMethod == Interpolation.InverseDistanceWeighted)
            {
                int width = (int)destWidth;
                int height = (int)destHeight;

                float coef_widthDestToSource = sourceSize.x / destSize.x;
                float coef_heigthDestToSource = sourceSize.y / destSize.y;

                float sourceX, sourceY;

                /*
                 * p_1      p_2
                 *      p'
                 * p_3      p_4
                 * 
                 */

                float d1, d2, d3, d4; // distance for 4 adjacent pixels

                float w1, w2, w3, w4; // weight for 4 adjacent pixels

                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        sourceX = x * coef_widthDestToSource;
                        sourceY = y * coef_heigthDestToSource;

                        d1 = Mathf.Sqrt(Mathf.Pow(sourceX - Mathf.Floor(sourceX), 2) + Mathf.Pow(sourceY - Mathf.Floor(sourceY), 2));
                        d2 = Mathf.Sqrt(Mathf.Pow(sourceX - Mathf.Ceil(sourceX), 2) + Mathf.Pow(sourceY - Mathf.Floor(sourceY), 2));
                        d3 = Mathf.Sqrt(Mathf.Pow(sourceX - Mathf.Floor(sourceX), 2) + Mathf.Pow(sourceY - Mathf.Ceil(sourceY), 2));
                        d4 = Mathf.Sqrt(Mathf.Pow(sourceX - Mathf.Ceil(sourceX), 2) + Mathf.Pow(sourceY - Mathf.Ceil(sourceY), 2));

                        // handle Border Exception
                        //if (sourceX > sourceSize.x || sourceY > sourceSize.y)
                        //{
                        //    destColors[x + y * width] = sourceColors[(int)((int)sourceX + (int)sourceY * sourceSize.x)];
                        //}
                        // handle Zero Division Exception
                        // Note that Floor and Ceil contain Equality => Check d1 is enough.
                        if (d1 == 0) destColors[x + y * width] = sourceColors[(int)((int)sourceX + (int)sourceY * sourceSize.x)];
                        else
                        {
                            w1 = 1 / d1;
                            w2 = 1 / d2;
                            w3 = 1 / d3;
                            w4 = 1 / d4;

                            destColors[x + y * width] =
                                (sourceColors[(int)((int)sourceX + (int)sourceY * sourceSize.x)] * w1 +
                                sourceColors[(int)((int)sourceX + 1 + (int)sourceY * sourceSize.x)] * w2 +
                                sourceColors[(int)((int)sourceX + (int)(sourceY + 1) * sourceSize.x)] * w3 +
                                sourceColors[(int)((int)sourceX + 1 + (int)(sourceY + 1) * sourceSize.x)] * w4) / (w1 + w2 + w3 + w4);
                        }
                    }
                }
            }

            newTex.SetPixels(destColors);
            newTex.Apply();

            return newTex;
        }

        #endregion Texture2D

        #region Toggle

        /// <summary>
        /// Safe toggle on for toggle Group
        /// </summary>
        /// <param name="toggle">enable target toggle</param>
        /// <param name="group">toggle group</param>
        /// <returns></returns>
        public static IEnumerator SafeToggleOn(this Toggle toggle)
        {
            if (toggle.group == null)
            {
                toggle.isOn = true;
                yield break;
            }

            yield return new WaitUntil(() => toggle.group.enabled && toggle.enabled);

            toggle.isOn = true;
        }

        #endregion Toggle

        #region RichText

        /// <summary>
        /// Add ordinal decoration to number. i.e. 1 -> 1st
        /// </summary>
        /// <param name="ordinalNumber">number</param>
        /// <param name="fontSize">size of number text</param>
        /// <param name="sizeRatio">size ratio of ordinal decor to number</param>
        /// <returns></returns>
        public static string AddOrdinalDecoration(this string ordinalNumber, int fontSize, float sizeRatio = 0.66f)
        {
            string decoration = "";
            if (ordinalNumber.EndsWith("1") && ordinalNumber != "11")
                decoration = "st";
            else if (ordinalNumber.EndsWith("2") && ordinalNumber != "12")
                decoration = "nd";
            else if (ordinalNumber.EndsWith("3") && ordinalNumber != "13")
                decoration = "rd";
            else
                decoration = "th";

            decoration.SetSize((int)(fontSize * sizeRatio));

            return ordinalNumber + decoration;
        }
        public static string SetSize(this string str, int size)
        {
            if (string.IsNullOrEmpty(str)) return "";

            return $"<size={size}>{str}</size>";
        }

        public static string SetItalic(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            return $"<i>{str}</i>";
        }

        public static string SetBold(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            return $"<b>{str}</b>";
        }

        public static string SetColor(this string str, Color color)
        {
            if (string.IsNullOrEmpty(str)) return "";

            string hexColorStr = ColorUtility.ToHtmlStringRGBA(color);
            return $"<color=#{hexColorStr}>{str}</color>";
        }

        #endregion RichText Tag

        #region Layout Group

        /// <summary>
        /// Set Layout group only once to increase performance.
        /// <para/>You can call this method in OnEnable.
        /// </summary>
        /// <param name="monoBehaviour">Parent component</param>
        /// <param name="isHorizontal"></param>
        /// <param name="isVertical"></param>
        public static void SetChildrenLayoutGroupOnlyOnce(this MonoBehaviour monoBehaviour, bool isHorizontal = true, bool isVertical = false)
        {
            monoBehaviour.StartCoroutine(IE_SetChildrenLayoutOnlyOnce(monoBehaviour.transform, isHorizontal, isVertical));
        }

        private static IEnumerator IE_SetChildrenLayoutOnlyOnce(Transform transform, bool isHorizontal, bool isVertical)
        {
            SetActiveChildrenLayout(transform, true, isHorizontal, isVertical);
            yield return null;
            SetActiveChildrenLayout(transform, false, isHorizontal, isVertical);
        }

        private static void SetActiveChildrenLayout(Transform transform, bool isActive, bool isHorizontal, bool isVertical)
        {
            if (isHorizontal)
            {
                HorizontalLayoutGroup[] groups = transform.GetComponentsInChildren<HorizontalLayoutGroup>();

                int len = groups.Length;

                for (int i = 0; i < len; ++i)
                {
                    groups[i].enabled = isActive;
                }
            }
            if (isVertical)
            {
                VerticalLayoutGroup[] groups = transform.GetComponentsInChildren<VerticalLayoutGroup>();

                int len = groups.Length;

                for (int i = 0; i < len; ++i)
                {
                    groups[i].enabled = isActive;
                }
            }
        }

        #endregion
    }
}