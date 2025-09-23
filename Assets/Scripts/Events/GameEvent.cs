using UnityEngine;

// ����һ�� GameEvent ���ݽṹ������ EventDialogPanel �� ShowEvent ��������������ݶ��������UI��
[CreateAssetMenu(fileName = "New GameEvent", menuName = "Game/Game Event")]
public class GameEvent : ScriptableObject
{
    [Header("�¼���Ϣ")]
    public string title;
    [TextArea(3, 10)]
    public string description;

    // ���������չ�����¼���ص����ԣ����磺
    // public Sprite eventImage;
    // public Choice[] choices;
}