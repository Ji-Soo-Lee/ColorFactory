/*�������࿡ ��� ������.*/
using UnityEngine;

[System.Serializable]
public class ColorEntry
{//�ȷ�Ʈ�� �� ��
    public string color;//HEX �÷��ڵ�
    public Color get_color()
    {//�÷��ڵ带 �� ������Ʈ�� ��ȯ�ϱ�
        if (ColorUtility.TryParseHtmlString(color, out Color result))
        {
            return result;
        }
        else
        {
            Debug.LogError("Invalid color code: " + color);
            return Color.white; // Or a default color
        }
    }
}

[System.Serializable]
public class ColoringPage
{//�� ������ ����
    public string file;//���ϸ�
    public int count;//�� ������ ��
    public float scale;//ũ�� ����
    public ColorEntry[] answer;//������ �� ������ ����. ������ ��������Ʈ�� ��ȣ ���̸� 0�� �ε����� ���� �ʴ´�.
    public ColorEntry[] palette;//�ȷ�Ʈ�� �� ��
}

[System.Serializable]
public class ColoringDex
{//���� ����
    public ColoringPage[] problemset;
}