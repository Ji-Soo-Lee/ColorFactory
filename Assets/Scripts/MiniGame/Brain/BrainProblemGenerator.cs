using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainProblemGenerator : MonoBehaviour
{
    BrainGameManager game;
    public GameObject blockPrefab;
    public GameObject[] palette;
    int row = 2;//���� ��
    int column = 2;//���� ��
    int kind = 3;//���� ��
    Color[] colors = new Color[3] { Color.red, Color.green, Color.blue };

    void Start()
    {
        this.game = BrainGameManager.game;
        game.new_problem += InitiateProblem;
        game.stop_problem += stop_problem;
        for (int i=0; i<this.kind; i++)
        {
            Button button = this.palette[i].GetComponent<Button>();
            Color color = this.colors[i];
            ColorBlock cb = button.colors;
            cb.normalColor = color; cb.highlightedColor = color; cb.selectedColor = color;
            button.colors = cb;
            button.onClick.AddListener(() => change_color(color));
        }
    }
    void change_color(Color x)
    {//��ư�� �ִ� �Լ�. ���� �� �ٲٱ�.
        this.game.now_color = x;
    }
    public void InitiateProblem()
    {//�� ������ ���� �غ��ϱ�
        StartCoroutine(ProblemCycle());
    }
    IEnumerator ProblemCycle()
    {//�� ������ ����� ����Ŭ
        yield return new WaitForSeconds(2.0f);
        prepare_problem();
        yield return new WaitForSeconds(4.0f);
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
                int x = Random.Range(0, this.kind);
                StartCoroutine(block.GetComponent<Block>().assign_color(this.colors[x]));
            }
        }
    }
    void difficulty_increase()
    {//���̵� �ø���
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
        if(game.playable)
        {
            game.playable = false;
            int cnt = 0;
            bool wrong = false;
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {
                Block block = x.GetComponent<Block>();
                bool correct = block.verdict();
                if (correct)
                {//���� �� �ϳ��� 1��
                    cnt += 1;
                }
                else
                {//Ʋ�� ���ڰ� ������ ǥ��
                    wrong = true;
                }
                Destroy(x);
            }
            game.apply_result(cnt);
            if(!wrong)
            {
                difficulty_increase();
            }
        }
    }
    void stop_problem()
    {//�� ���� ����. ���� ������ ������ �ٽ� ������ �� �� ����Ѵ�.
        StopAllCoroutines();
    }
}
