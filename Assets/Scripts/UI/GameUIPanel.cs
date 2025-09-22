using UnityEngine;
using TMPro;

public class GameUIPanel : MonoBehaviour
{
    [Header("UI 元素引用")]
    [SerializeField] private TextMeshProUGUI dayCountText;
    [SerializeField] private TextMeshProUGUI woodCountText;
    // ... 其他资源文本

    // 你可以从其他地方（比如 GameManager）调用这个方法来更新UI
    public void UpdateResourceDisplay(int day, int wood)
    {
        dayCountText.text = $"天数: {day}";
        woodCountText.text = $"木材: {wood}";
    }
}