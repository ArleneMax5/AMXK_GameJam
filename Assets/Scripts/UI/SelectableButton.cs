using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

// �Ƴ� IPointerEnterHandler �� IPointerClickHandler
public class SelectableButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("UI Ԫ������")]
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private TextMeshProUGUI buttonLabel;

    [Header("��ͷ��������")]
    [SerializeField] private float floatDistance = 20f;
    [SerializeField] private float floatDuration = 0.5f;

    [Header("�ı����⶯������")]
    [SerializeField] private Color glowColor = Color.white;
    [SerializeField] [Range(0, 1)] private float maxGlowPower = 0.5f;
    [SerializeField] private float glowDuration = 1.5f;

    [Header("��ť����¼�")]
    public UnityEvent OnActivated;

    // �Ƴ��˶� UIManager ������

    private Tween leftArrowTween;
    private Tween rightArrowTween;
    private Tween glowTween;

    private Material buttonLabelMaterial;
    private float initialGlowPower;

    private BasePanel parentPanel; // �Ը���������

    void Awake()
    {
        if (buttonLabel != null)
        {
            buttonLabelMaterial = new Material(buttonLabel.fontMaterial);
            buttonLabel.fontMaterial = buttonLabelMaterial;
            initialGlowPower = buttonLabelMaterial.GetFloat("_GlowPower");
        }
        // ��ʼʱ��������Ч��
        OnDeselected();

        parentPanel = GetComponentInParent<BasePanel>();
    }

    // �������ͣʱ���¼�ϵͳ�Զ�����
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parentPanel != null)
        {
            parentPanel.SelectButton(this);
        }
    }

    // �������ʱ���¼�ϵͳ�Զ�����
    public void OnPointerClick(PointerEventData eventData)
    {
        ActivateButton();
    }

    public void OnSelected()
    {
        if (buttonLabel != null)
        {
            glowTween?.Kill();
            buttonLabelMaterial.SetColor("_GlowColor", glowColor);
            glowTween = buttonLabelMaterial.DOFloat(maxGlowPower, "_GlowPower", glowDuration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        leftArrow?.SetActive(true);
        rightArrow?.SetActive(true);

        leftArrowTween?.Kill();
        rightArrowTween?.Kill();

        leftArrowTween = leftArrow?.transform.DOLocalMoveX(-floatDistance, floatDuration)
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetRelative(true);

        rightArrowTween = rightArrow?.transform.DOLocalMoveX(floatDistance, floatDuration)
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
    }

    public void OnDeselected()
    {
        if (buttonLabel != null)
        {
            glowTween?.Kill();
            buttonLabelMaterial.SetFloat("_GlowPower", initialGlowPower);
        }

        leftArrowTween?.Kill(true);
        rightArrowTween?.Kill(true);

        leftArrow?.SetActive(false);
        rightArrow?.SetActive(false);
    }

    public void ActivateButton()
    {
        OnActivated?.Invoke();
    }


    void OnDestroy()
    {
        if (buttonLabelMaterial != null)
        {
            Destroy(buttonLabelMaterial);
        }
    }
}