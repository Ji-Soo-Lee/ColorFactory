using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProblemGenerator : MonoBehaviour
{
    GameManager game;
    public GameObject blockPrefab;
    public GameObject palettePrefab;
    public bool playing = false;
    float elapsed = 0.0f;
    int row = 2;//행의 수
    int column = 2;//열의 수
    int kind = 3;//색의 수

    Block[] blocks = new Block[40];

    void Start()
    {
        this.game = GameManager.game;
    }
    void Update()
    {//타이머가 돌아간다.
        elapsed += Time.deltaTime;
        if (this.elapsed > 1.0f && this.playing == false)
        {//문제를 보여주기 전 대기 시간
            this.playing = true;
            StartCoroutine(ProblemCycle());
        }
    }
    public void InitiateProblem()
    {//새 문제를 만들 준비하기
        this.elapsed = 0; this.playing = false;
    }
    IEnumerator ProblemCycle()
    {//한 문제를 만드는 사이클
        prepare_problem();
        yield return new WaitForSeconds(2.0f);
        show_problem();
        yield return new WaitForSeconds(3.0f);
        game.playable = true;
    }
    void prepare_problem()
    {//블록 준비하기
        for (int i = 0; i < this.row; i++)
        {//블록들을 설치한다.
            for (int j = 0; j < this.column; j++)
            {
                Vector3 position = new Vector3((-0.25f * (this.column - 1)) + j * 0.5f, (0.25f * (this.row - 1)) - i * 0.5f, 0);
                GameObject block = Instantiate(this.blockPrefab, position, Quaternion.identity);
                this.blocks[this.column * i + j + 1] = block.GetComponent<Block>();
            }
        }
    }
    void show_problem()
    {//문제 보이기(문제는 자동으로 숨겨진다)
        int total = this.row * this.column;
        for (int i = 1; i <= total; i++)
        {//블록에 색 지정하기
            int x = Random.Range(1, this.kind + 1);
            StartCoroutine(blocks[i].assign_color(x, game.color[x]));
        }
    }
    void difficulty_increase()
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
    public void judge_answer()
    {//응답 채점하기
        if(game.playable==true)
        {
            game.playable = false;
            int total = this.row * this.column;
            int cnt = 0;
            bool wrong = false;
            for (int i = 1; i <= total; i++)
            {//블록에 색 지정하기
                bool result = blocks[i].verdict();
                if (result == true)
                {//맞은 것 하나당 1점
                    cnt += 1;
                }
                if (result == false)
                {//틀린 상자가 있으면 표시
                    wrong = true;
                }
            }
            game.apply_result(cnt);
            reset_problem(!wrong);
        }
    }
    void reset_problem(bool levelup)
    {//문제 정리하고 다음 문제 준비하기
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in obj)
        {
            Destroy(x);
        }
        if(levelup)
        {//레벨을 올린다면 올리기
            difficulty_increase();
        }
    }
}
