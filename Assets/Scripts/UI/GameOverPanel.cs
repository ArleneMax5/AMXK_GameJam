using UnityEngine;

public class GameOverPanel : BasePanel
{
    [Header("��Ϸ��������")]
    [SerializeField] private GameObject backgroundOverlay; // ��͸����������

    public override void Show()
    {
        base.Show();
        
        // ȷ�����ǲ������
        if (backgroundOverlay != null)
        {
            backgroundOverlay.SetActive(true);
        }
        
        Debug.Log("��ʾ��Ϸ�������渲�ǲ�");
    }

    public override void Hide()
    {
        if (backgroundOverlay != null)
        {
            backgroundOverlay.SetActive(false);
        }
        
        base.Hide();
        Debug.Log("������Ϸ�������渲�ǲ�");
    }
}