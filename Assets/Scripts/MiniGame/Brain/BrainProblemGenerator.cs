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
    public Color[] colors;

    void Start()
    {
        this.game = BrainGameManager.game;
        game.new_problem += InitiateProblem;
        game.stop_problem += stop_problem;
        game.difficulty_increase += difficulty_increase;
        for (int i=0; i<this.kind; i++)
        {
            Button button = this.palette[i].GetComponent<Button>();
            Color color = this.colors[i];
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
        //Debug.Log("������ ����� �����մϴ�.");
        this.game.remain = (this.row * this.column);
        yield return new WaitForSeconds(1.2f);
        prepare_problem();
        yield return new WaitForSeconds(4.0f);
        game.toggle_player(true);
    }
    void prepare_problem()
    {//���� �غ��ϱ�
        for (int i = 0; i < this.row; i++)
        {//���ϵ��� ��ġ�Ѵ�.
            for (int j = 0; j < this.column; j++)
            {
                Debug.Log("���� ����");
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
    void stop_problem()
    {//�� ���� ����. ���� ������ ������ �ٽ� ������ �� �� ����Ѵ�.
        StopAllCoroutines();
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in obj)
        {
            Destroy(x);
        }
    }
}
