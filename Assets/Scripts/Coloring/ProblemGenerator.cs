using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ProblemGenerator : MonoBehaviour
{
    public GameObject frame;//��ĥ���� ������(������)
    public GameObject partPrefab;//���� ��ĥ�ϴ� �κ�
    string sample = "Assets/Sprites/problem_zzangu.png";
    void Start()
    {//�׽�Ʈ��
        Sprite file = AssetDatabase.LoadAssetAtPath<Sprite>(sample);//������ �� ����
        if(file==null)
        {
            Debug.Log("������ ã�� ���߽��ϴ�.");
            return;
        }
        Object[] problem = AssetDatabase.LoadAllAssetRepresentationsAtPath(sample);//������ �����ϴ� ��������Ʈ. �ϳ��� �������� ���� ��ĥ�ϴ� �κе�� ������. 
        for (int i = 0; i < problem.Length; i++)
        {//�̵� ������ ��������Ʈȭ �ϰ� ȭ�鿡 ����.
            Sprite sprite = problem[i] as Sprite;
            if (sprite == null)
            {
                Debug.LogError("������ �ҷ����� �� ������ �߻��߽��ϴ�.");
                return;
            }
            if(i==0)
            {//������ �κ�
                this.frame.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {//��ĥ�ϴ� �κ�
                GameObject part = Instantiate(this.partPrefab, this.frame.transform);
                part.GetComponent<SpriteRenderer>().sprite = sprite;//��ĥ�ϴ� ������ ��������Ʈ�� �ο��Ѵ�.
                part.AddComponent<PolygonCollider2D>();//�� ��������Ʈ�� �´� �ݶ��̴��� �ش�.
                part.GetComponent<ColorPart>().x = i;
            }//�� �κе��� frame�� �ڼ����� �ִ´�.
        }
        float d = 0.7f;
        this.frame.transform.localScale = new Vector3(d, d, d);
    }
}
