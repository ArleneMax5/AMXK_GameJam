using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPointDisplay : MonoBehaviour
{
    [Header("���� (�������е�)")]
    [SerializeField] private Transform container;

    [Header("���Ԥ�� (����� Image)")]
    [SerializeField] private Image pointPrefab;

    [Header("��������")]
    [SerializeField] private Sprite filledSprite; // ��ɫ
    [SerializeField] private Sprite emptySprite;  // ��ɫ

    [Header("��ѡ������ Tint ��ɫ")]
    [SerializeField] private Color filledTint = Color.white;
    [SerializeField] private Color emptyTint = Color.white;

    [Header("��������")]
    [Tooltip("�Ƿ�ת��ʾ˳���������ö���/������ʣ�����λ��")]
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
            // �Ӿ�˳��ɷ�ת
            int logicalIndex = invertVisualOrder ? (_points.Count - 1 - i) : i;

            bool stillHave = logicalIndex < current; // ʣ��㣺��ɫ
            var img = _points[i];

            if (filledSprite != null && emptySprite != null)
            {
                img.sprite = stillHave ? filledSprite : emptySprite;
                img.color = stillHave ? filledTint : emptyTint;
            }
            else
            {
                // ���ף����δ���þ��飬���ô�ɫռλ
                img.color = stillHave ? Color.yellow : Color.gray;
            }
        }
    }

    private void Rebuild(int max)
    {
        // ɾ���ɵ�
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