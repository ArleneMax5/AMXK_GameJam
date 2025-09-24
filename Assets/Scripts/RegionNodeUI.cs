using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class RegionNodeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("��ʶ")]
    public string regionId;

    // [�ڽ�����] ��ص��ֶκͷ����ѱ��Ƴ�

    [Header("��Ч����")]
    [SerializeField] private string hoverSoundName = "RegionHover";
    [SerializeField] private string clickSoundName = "RegionClick";
    // [��Ч�����Ч] �ֶ��ѱ��Ƴ�

    // �ɿ�����ע��
    [HideInInspector] public RegionMapUIController controller;

    Image _img;

    void Awake()
    {
        _img = GetComponent<Image>();
    }

    // IsNeighborOf �����ѱ��Ƴ�

    // ͳһ���ñ���ɫ��������ͼƬ�����ɫ��
    public void SetColor(Color c)
    {
        if (_img) _img.color = c;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        controller?.SetHover(this);

        // ������ͣ��Ч
        if (AudioManager.Instance != null && !string.IsNullOrEmpty(hoverSoundName))
        {
            AudioManager.Instance.PlaySFX(hoverSoundName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        controller?.SetHover(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller == null) return;

        // ֻҪ����Ĳ��ǵ�ǰ���򣬾���Ϊ��Ч���
        if (this != controller.currentRegion)
        {
            // ���ųɹ������Ч
            if (AudioManager.Instance != null && !string.IsNullOrEmpty(clickSoundName))
            {
                AudioManager.Instance.PlaySFX(clickSoundName);
            }
            // ���Խ���������
            controller.TryEnter(this);
        }
        // ��Ч������߼�����Ч�ѱ���ȫ�Ƴ�
    }
}
