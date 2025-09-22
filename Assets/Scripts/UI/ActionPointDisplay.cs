using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �����ж�����
public class ActionPointDisplay : MonoBehaviour
{
    [Header("UI Ԫ��")]
    [SerializeField] private List<Image> pointImages;

    [Header("��ɫ����")]
    [SerializeField] private Color filledColor = Color.yellow;
    [SerializeField] private Color emptyColor = Color.black;

    /// <summary>
    /// �����ж���������ʾ
    /// </summary>
    /// <param name="currentPoints">��ǰ���ж�����</param>
    public void UpdatePoints(int currentPoints)
    {
        for (int i = 0; i < pointImages.Count; i++)
        {
            // ����������С�ڵ�ǰ�ĵ���������ʾΪ������䡱��ɫ������Ϊ���ա���ɫ
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