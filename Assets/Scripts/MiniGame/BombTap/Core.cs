using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    BombTapGameManager game;
    SpriteRenderer sprite;
    public GameObject bombPrefab;
    const int TOTAL = 7;//��ź �� ������
    const float R = 1.8f;//�ھ �߽����� �� ���� ������. �� ���� ���ֿ� ��ź�� ���´�.
    Color[] dex = new Color[TOTAL] { Color.red, Color.green, Color.blue, Color.magenta, Color.yellow, Color.cyan, Color.white };
    void Awake()
    {
        this.game = BombTapGameManager.game;
        this.sprite = GetComponent<SpriteRenderer>();
        this.game.initiate += make_problem;
    }
    void make_problem(int num)
    {//��ǥ �� ���ϰ� ��ź ����. �� ���� ���� ���� ��ź���� ��� ������ Ŭ���ؾ� �Ѵ�.
        int target = Random.Range(0, TOTAL);
        float d = 360.0f / (float)num;//�� ��ź ���� ������ ����(�� ����. ���� ��ġ �ÿ��� ȣ�������� �ٲپ� ����)
        int cnt = 0;//��ǥ ��ź�� ����
        Color key = this.dex[target];//�̹� ��ǥ ��.
        sprite.color = key;
        for(int i=0; i<num; i++)
        {//��ź �������� ��ġ�ϱ�.
            float deg = (d * (float)i);//��ġ ����.
            Color color;
            Vector3 pos = new Vector3(R * Mathf.Cos(Mathf.Deg2Rad * deg), R * Mathf.Sin(Mathf.Deg2Rad * deg), 0);//���� ���� �� ��ġ ��ġ
            GameObject bomb = Instantiate(this.bombPrefab, pos, Quaternion.identity);
            if(i==(num-1) && cnt==0)
            {//��ǥ ���� �ּ��� �� ���� �ְ� �Ѵ�.
                color = key;
            }
            else
            {
                int x = Random.Range(0, TOTAL);
                color = this.dex[x];
            }
            cnt += (color == key ? 1 : 0);
            bomb.GetComponent<Bomb>().assign_color(color);
        }
        game.set_goal(key, cnt);//key ���� ��ź�� cnt�� �ִ�.
        this.game.playable = true;
    }
}
