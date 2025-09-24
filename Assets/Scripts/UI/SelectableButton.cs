using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

// 移除了 IPointerEnterHandler 和 IPointerClickHandler
public class SelectableButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("UI 元素引用")]
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private TextMeshProUGUI buttonLabel;

    [Header("箭头浮动动画")]
    [SerializeField] private float floatDistance = 20f;
    [SerializeField] private float floatDuration = 0.5f;

    [Header("文本发光动画效果")]
    [SerializeField] private Color glowColor = Color.white;
    [SerializeField] [Range(0, 1)] private float maxGlowPower = 0.5f;
    [SerializeField] private float glowDuration = 1.5f;

    [Header("音效设置")]
    [SerializeField] private string hoverSoundName = "ButtonHover";
    [SerializeField] private string clickSoundName = "ButtonClick";

    [Header("按钮激活事件")]
    public UnityEvent OnActivated;

    // 移除了对 UIManager 的引用

    private Tween leftArrowTween;
    private Tween rightArrowTween;
    private Tween glowTween;

    private Material buttonLabelMaterial;
    private float initialGlowPower;

    private BasePanel parentPanel; // 父面板引用

    void Awake()
    {
        if (buttonLabel != null)
        {
            // 注意：这里我们不再创建新材质，而是获取实例化的材质
            // Unity 在运行时第一次访问 renderer.material 时会自动创建实例
            buttonLabel.ForceMeshUpdate(); // 确保字体材质已准备好
            buttonLabelMaterial = buttonLabel.fontMaterial;
            initialGlowPower = buttonLabelMaterial.GetFloat("_GlowPower");
        }
        // 初始时关闭视觉效果
        OnDeselected();

        parentPanel = GetComponentInParent<BasePanel>();
    }

    // 鼠标悬停时，事件系统自动调用
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parentPanel != null)
        {
            parentPanel.SelectButton(this);
        }

        // 播放悬停音效
        if (AudioManager.Instance != null && !string.IsNullOrEmpty(hoverSoundName))
        {
            AudioManager.Instance.PlaySFX(hoverSoundName);
        }
    }

    // 鼠标点击时，事件系统自动调用
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
            
            // *** 修改点：使用 DOTween 的 TextMeshPro 专用扩展方法 ***
            glowTween = DOTween.To(
                () => buttonLabelMaterial.GetFloat("_GlowPower"),
                x => buttonLabelMaterial.SetFloat("_GlowPower", x),
                maxGlowPower,
                glowDuration
            )
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true); // 关键！让动画忽略 timeScale
        }

        leftArrow?.SetActive(true);
        rightArrow?.SetActive(true);

        leftArrowTween?.Kill();
        rightArrowTween?.Kill();

        leftArrowTween = leftArrow?.transform.DOLocalMoveX(-floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative(true)
            .SetUpdate(true); // 关键！让动画忽略 timeScale

        rightArrowTween = rightArrow?.transform.DOLocalMoveX(floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative(true)
            .SetUpdate(true); // 关键！让动画忽略 timeScale
    }

    public void OnDeselected()
    {
        if (buttonLabel != null && buttonLabelMaterial != null)
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
        // 播放点击音效
        if (AudioManager.Instance != null && !string.IsNullOrEmpty(clickSoundName))
        {
            AudioManager.Instance.PlaySFX(clickSoundName);
        }

        OnActivated?.Invoke();
    }

    void OnDestroy()
    {
        // 由于我们不再手动 new Material，所以也不需要手动 Destroy
        // if (buttonLabelMaterial != null)
        // {
        //     Destroy(buttonLabelMaterial);
        // }
    }
}