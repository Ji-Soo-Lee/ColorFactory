using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemGenerator : MonoBehaviour
{
    GameManager game;
    public GameObject blockPrefab;
    public GameObject palettePrefab;
    bool generated = false;
    float elapsed = 0.0f, limit = 3.0f;
    int row = 2;//행의 수
    int column = 2;//열의 수
    int kind = 3;//색의 수
    void Start()
    {
        this.game = GameManager.game;
    }
    void Update()
    {//타이머가 돌아간다.
        elapsed += Time.deltaTime;
        if(this.elapsed>this.limit && this.generated==false)
        {//문제를 보여준 후 일정 시간이 지나면 모든 블록의 색을 제거한다.
            play_problem();
        }
    }
    public void make_problem()
    {//문제 만들기
        this.elapsed = 0; this.generated = false;//문제를 제공한다.
        for(int i=0; i<this.row; i++)
        {//우선 블록들을 설치하고, 무작위 색을 배정한다.
            for(int j=0; j<this.column; j++)
            {
                Vector3 position = new Vector3((-0.5f * (this.column - 1)) + j, (0.5f * (this.row - 1)) - i, 0);
                GameObject block= Instantiate(this.blockPrefab, position, Quaternion.identity);
                int x = Random.Range(1, kind + 1);
                block.GetComponent<Block>().assign_color(x, game.color[x]);
            }
        }
    }
    void play_problem()
    {//문제로 낸 색들을 숨기고 플레이어가 응답을 할 수 있게 한다.
        this.generated = true;
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in allBlocks)
        {//블록들의 색을 제거한다.
            Renderer renderer = block.GetComponent<Renderer>();
            renderer.material = game.color[0];
        }
        for(int i=1; i<=this.kind; i++)
        {//아래에 팔레트를 설치한다.
            Vector3 position = new Vector3(-4 + 2 * i, -4, 0);
            GameObject palette = Instantiate(this.palettePrefab, position, Quaternion.identity);
            palette.GetComponent<Palette>().assign_color(i, game.color[i]);
        }
        game.playable = true;
    }
}
