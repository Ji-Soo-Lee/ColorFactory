using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProblemGenerator : MonoBehaviour
{
    GameManager game;
    public GameObject blockPrefab;
    public GameObject palettePrefab;
    enum State { Waiting, Preparing, Memorizing, Playing };
    State state = State.Waiting;
    float elapsed = 0.0f;
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
        if (this.elapsed > 1.0f && this.state == State.Waiting)
        {//문제를 보여주기 전 대기 시간
            prepare_problem();
        }
        if (this.elapsed > 2.0f && this.state == State.Preparing)
        {//문제를 보여주기
            show_problem(true);
        }
        if (this.elapsed > 3.0f && this.state == State.Memorizing)
        {//문제 숨기고 사용자가 답하게 하기
            show_problem(false);
        }
    }
    void prepare_problem()
    {
        this.elapsed = 0; this.state = State.Preparing;
        for (int i = 0; i < this.row; i++)
        {//블록들을 설치한다.
            for (int j = 0; j < this.column; j++)
            {
                Vector3 position = new Vector3((-0.5f * (this.column - 1)) + j, (0.5f * (this.row - 1)) - i, 0);
                GameObject block = Instantiate(this.blockPrefab, position, Quaternion.identity);
            }
        }
    }
    void show_problem(bool show)
    {//문제 보이기, 숨기기
        this.elapsed = 0; this.state = (show ? State.Memorizing : State.Playing);
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            int x = (show ? Random.Range(1, this.kind + 1) : 0);
            block.GetComponent<Block>().assign_color(x, game.color[x], show);
        }
        if(this.state==State.Playing)
        {//사용자가 답을 할 때는 팔레트를 아래에 표시한다.
            for (int i = 1; i <= this.kind; i++)
            {//아래에 팔레트를 설치한다.
                Vector3 position = new Vector3(-4 + 2 * i, -4, 0);
                GameObject palette = Instantiate(this.palettePrefab, position, Quaternion.identity);
                palette.GetComponent<Palette>().assign_color(i, game.color[i]);
            }
            game.playable = true;
        }
    }
    public void difficulty_increase()
    {
        if(this.row==this.column)
        {
            this.column += 1;
        }
        else
        {
            this.row += 1;
        }
    }
    public void reset_problem()
    {
        GameObject[] block = GameObject.FindGameObjectsWithTag("Block");
        GameObject[] palette = GameObject.FindGameObjectsWithTag("Palette");
        GameObject[] obj = block.Concat(palette).ToArray();
        foreach (GameObject x in obj)
        {
            Destroy(x);
        }
        this.elapsed = 0; this.state = State.Waiting;
    }
}
