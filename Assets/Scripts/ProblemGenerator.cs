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
    int row = 2;//���� ��
    int column = 2;//���� ��
    int kind = 3;//���� ��

    Block[] blocks = new Block[40];

    void Start()
    {
        this.game = GameManager.game;
    }
    void Update()
    {//Ÿ�̸Ӱ� ���ư���.
        elapsed += Time.deltaTime;
        if (this.elapsed > 1.0f && this.playing == false)
        {//������ �����ֱ� �� ��� �ð�
            this.playing = true;
            StartCoroutine(ProblemCycle());
        }
    }
    public void InitiateProblem()
    {//�� ������ ���� �غ��ϱ�
        this.elapsed = 0; this.playing = false;
    }
    IEnumerator ProblemCycle()
    {//�� ������ ����� ����Ŭ
        prepare_problem();
        yield return new WaitForSeconds(2.0f);
        show_problem();
        yield return new WaitForSeconds(3.0f);
        game.playable = true;
    }
    void prepare_problem()
    {//��� �غ��ϱ�
        for (int i = 0; i < this.row; i++)
        {//��ϵ��� ��ġ�Ѵ�.
            for (int j = 0; j < this.column; j++)
            {
                Vector3 position = new Vector3((-0.25f * (this.column - 1)) + j * 0.5f, (0.25f * (this.row - 1)) - i * 0.5f, 0);
                GameObject block = Instantiate(this.blockPrefab, position, Quaternion.identity);
                this.blocks[this.column * i + j + 1] = block.GetComponent<Block>();
            }
        }
    }
    void show_problem()
    {//���� ���̱�(������ �ڵ����� ��������)
        int total = this.row * this.column;
        for (int i = 1; i <= total; i++)
        {//��Ͽ� �� �����ϱ�
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
    {//���� ä���ϱ�
        if(game.playable==true)
        {
            game.playable = false;
            int total = this.row * this.column;
            int cnt = 0;
            bool wrong = false;
            for (int i = 1; i <= total; i++)
            {//��Ͽ� �� �����ϱ�
                bool result = blocks[i].verdict();
                if (result == true)
                {//���� �� �ϳ��� 1��
                    cnt += 1;
                }
                if (result == false)
                {//Ʋ�� ���ڰ� ������ ǥ��
                    wrong = true;
                }
            }
            game.apply_result(cnt);
            reset_problem(!wrong);
        }
    }
    void reset_problem(bool levelup)
    {//���� �����ϰ� ���� ���� �غ��ϱ�
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in obj)
        {
            Destroy(x);
        }
        if(levelup)
        {//������ �ø��ٸ� �ø���
            difficulty_increase();
        }
    }
}
