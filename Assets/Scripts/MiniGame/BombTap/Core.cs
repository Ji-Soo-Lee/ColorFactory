using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    BombTapGameManager game;
    SpriteRenderer sprite;
    public GameObject bombPrefab;
    const int TOTAL = 7;//폭탄 색 가짓수
    const float R = 1.8f;//코어를 중심으로 한 원의 반지름. 이 원의 원주에 폭탄을 놓는다.
    Color[] dex = new Color[TOTAL] { Color.red, Color.green, Color.blue, Color.magenta, Color.yellow, Color.cyan, Color.white };
    void Start()
    {
        this.game = BombTapGameManager.game;
        this.sprite = GetComponent<SpriteRenderer>();
        this.game.initiate += make_problem;
    }
    void make_problem(int num)
    {//목표 색 정하고 폭탄 놓기. 이 색과 같은 색의 폭탄들을 모두 빠르게 클릭해야 한다.
        int target = Random.Range(0, TOTAL);
        float d = 360.0f / (float)num;//두 폭탄 사이 간격의 각도(도 단위. 실제 배치 시에는 호도법으로 바꾸어 쓴다)
        int cnt = 0;//목표 폭탄의 개수
        Color key = this.dex[target];//이번 목표 색.
        sprite.color = key;
        for(int i=0; i<num; i++)
        {//폭탄 무작위로 배치하기.
            float deg = (d * (float)i);//배치 각도.
            Color color;
            Vector3 pos = new Vector3(R * Mathf.Cos(Mathf.Deg2Rad * deg), R * Mathf.Sin(Mathf.Deg2Rad * deg), 0);//실제 원주 상 배치 위치
            GameObject bomb = Instantiate(this.bombPrefab, pos, Quaternion.identity);
            if(i==(num-1) && cnt==0)
            {//목표 색이 최소한 한 개는 있게 한다.
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
        game.set_goal(key, cnt);//key 색인 폭탄은 cnt개 있다.
        this.game.playable = true;
    }
}
