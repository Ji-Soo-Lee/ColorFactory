using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    SpriteRenderer sprite;
    public GameObject bombPrefab;
    const int TOTAL = 7;//폭탄 색 가짓수
    const float R = 1.8f;//코어를 중심으로 한 원의 반지름. 이 원의 원주에 폭탄을 놓는다.
    Color[] dex = new Color[TOTAL] { Color.red, Color.green, Color.blue, Color.magenta, Color.yellow, Color.cyan, Color.white };
    void Start()
    {
        this.sprite = GetComponent<SpriteRenderer>();
        make_problem(15);
    }
    void make_problem(int num)
    {//목표 색 정하기. 이 색과 같은 색의 폭탄들을 모두 빠르게 클릭해야 한다.
        int target = Random.Range(0, TOTAL);
        int d = 360 / num;
        Color color = this.dex[target];
        sprite.color = color;
        for(int i=0; i<num; i++)
        {
            int deg = (d * i);//배치 각도.
            Vector3 pos = new Vector3(R * Mathf.Cos(Mathf.Deg2Rad * deg), R * Mathf.Sin(Mathf.Deg2Rad * deg), 0);//실제 원주 상 배치 위치
            GameObject bomb = Instantiate(this.bombPrefab, pos, Quaternion.identity);
        }
    }
}
