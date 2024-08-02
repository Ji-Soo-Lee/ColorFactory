using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    SpriteRenderer sprite;
    public GameObject bombPrefab;
    const int TOTAL = 7;//��ź �� ������
    const float R = 1.8f;//�ھ �߽����� �� ���� ������. �� ���� ���ֿ� ��ź�� ���´�.
    Color[] dex = new Color[TOTAL] { Color.red, Color.green, Color.blue, Color.magenta, Color.yellow, Color.cyan, Color.white };
    void Start()
    {
        this.sprite = GetComponent<SpriteRenderer>();
        make_problem(15);
    }
    void make_problem(int num)
    {//��ǥ �� ���ϱ�. �� ���� ���� ���� ��ź���� ��� ������ Ŭ���ؾ� �Ѵ�.
        int target = Random.Range(0, TOTAL);
        int d = 360 / num;
        Color color = this.dex[target];
        sprite.color = color;
        for(int i=0; i<num; i++)
        {
            int deg = (d * i);//��ġ ����.
            Vector3 pos = new Vector3(R * Mathf.Cos(Mathf.Deg2Rad * deg), R * Mathf.Sin(Mathf.Deg2Rad * deg), 0);//���� ���� �� ��ġ ��ġ
            GameObject bomb = Instantiate(this.bombPrefab, pos, Quaternion.identity);
        }
    }
}
