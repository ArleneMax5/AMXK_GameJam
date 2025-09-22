using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 管理行动点数
public class ActionPointDisplay : MonoBehaviour
{
    [Header("UI 元素")]
    [SerializeField] private List<Image> pointImages;

    [Header("颜色设置")]
    [SerializeField] private Color filledColor = Color.yellow;
    [SerializeField] private Color emptyColor = Color.black;

    /// <summary>
    /// 更新行动点数的显示
    /// </summary>
    /// <param name="currentPoints">当前的行动点数</param>
    public void UpdatePoints(int currentPoints)
    {
        for (int i = 0; i < pointImages.Count; i++)
        {
            // 如果点的索引小于当前的点数，则显示为“已填充”颜色，否则为“空”颜色
            if (i < currentPoints)
            {
                pointImages[i].color = filledColor;
            }
            else
            {
                pointImages[i].color = emptyColor;
            }
        }
    }
}