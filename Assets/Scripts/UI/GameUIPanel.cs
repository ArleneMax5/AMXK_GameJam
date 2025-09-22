using UnityEngine;
using TMPro;

public class GameUIPanel : MonoBehaviour
{
    [Header("UI Ԫ������")]
    [SerializeField] private TextMeshProUGUI dayCountText;
    [SerializeField] private TextMeshProUGUI woodCountText;
    // ... ������Դ�ı�

    // ����Դ������ط������� GameManager�������������������UI
    public void UpdateResourceDisplay(int day, int wood)
    {
        dayCountText.text = $"����: {day}";
        woodCountText.text = $"ľ��: {wood}";
    }
}