using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPointDisplay : MonoBehaviour
{
    [Header("容器 (放置所有点)")]
    [SerializeField] private Transform container;

    [Header("点的预制 (必须带 Image)")]
    [SerializeField] private Image pointPrefab;

    [Header("精灵配置")]
    [SerializeField] private Sprite filledSprite; // 黄色
    [SerializeField] private Sprite emptySprite;  // 灰色

    [Header("可选：调整 Tint 颜色")]
    [SerializeField] private Color filledTint = Color.white;
    [SerializeField] private Color emptyTint = Color.white;

    [Header("方向设置")]
    [Tooltip("是否反转显示顺序（例如想让顶部/左侧代表剩余最高位）")]
    [SerializeField] private bool invertVisualOrder = false;

    private readonly List<Image> _points = new List<Image>();
    private int _cachedMax = -1;

    public void Refresh(int current, int max)
    {
        if (max <= 0) return;
        if (max != _cachedMax)
        {
            Rebuild(max);
            _cachedMax = max;
        }

        current = Mathf.Clamp(current, 0, max);

        for (int i = 0; i < _points.Count; i++)
        {
            // 视觉顺序可反转
            int logicalIndex = invertVisualOrder ? (_points.Count - 1 - i) : i;

            bool stillHave = logicalIndex < current; // 剩余点：黄色
            var img = _points[i];

            if (filledSprite != null && emptySprite != null)
            {
                img.sprite = stillHave ? filledSprite : emptySprite;
                img.color = stillHave ? filledTint : emptyTint;
            }
            else
            {
                // 兜底：如果未配置精灵，可用纯色占位
                img.color = stillHave ? Color.yellow : Color.gray;
            }
        }
    }

    private void Rebuild(int max)
    {
        // 删除旧点
        for (int i = _points.Count - 1; i >= 0; i--)
        {
            if (_points[i] != null)
            {
                if (Application.isPlaying)
                    Destroy(_points[i].gameObject);
                else
                    DestroyImmediate(_points[i].gameObject);
            }
        }
        _points.Clear();

        if (container == null || pointPrefab == null) return;

        for (int i = 0; i < max; i++)
        {
            var img = Instantiate(pointPrefab, container);
            img.name = $"AP_Point_{i + 1}";
            _points.Add(img);
        }
    }
}