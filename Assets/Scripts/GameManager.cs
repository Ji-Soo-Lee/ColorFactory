using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public int selected = 0;//���� ���õ� ��
    public bool playable = false;//���� �÷��̾ ���� �� �� �ִ���
    public Material[] color;//���ӿ��� ����ϴ� ��
    public GameObject scoreboard;
    int score = 0;
    float elapsed = 0.0f;
    bool start = false;
    void Awake()
    {//���� �Ŵ����� ���� �̱������� �����ϱ�.
        if(game==null)
        {
            game = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Debug.Log("���� �غ�");
    }
    void Update()
    {//Ÿ�̸�
        this.elapsed += Time.deltaTime;
        if(this.start==false && this.elapsed>=2.0f)
        {
            this.start = true;
            GameObject.Find("ProblemGenerator").GetComponent<ProblemGenerator>().make_problem();
        }
    }
    public void judge_answer()
    {//����ڰ� �Է��� �� ���ϱ�
        if(this.playable==true)
        {
            this.playable = false;
            bool wrong = false;
            GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
            foreach (GameObject block in allBlocks)
            {//�� ��Ϻ��� ���� �ٸ��� ĥ�������� Ȯ���Ѵ�.
                bool result = block.GetComponent<Block>().verdict();
                if (result == false)
                {//���� Ʋ�� ���
                    wrong = true;
                }
            }
            Debug.Log(!wrong);//���� �¾Ҵ��� Ʋ�ȴ��� ǥ��
            this.score += (wrong ? 0 : 1);//������ ��� ���� �ο�
            this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
            reset_problem();
        }
    }
    void reset_problem()
    {//������ ���� ��� ��ϰ� �ȷ�Ʈ�� ġ���.
        GameObject[] block = GameObject.FindGameObjectsWithTag("Block");
        GameObject[] palette = GameObject.FindGameObjectsWithTag("Palette");
        GameObject[] obj = block.Concat(palette).ToArray();
        foreach (GameObject x in obj)
        {
            Destroy(x);
        }
        this.start = false; this.elapsed = 0.0f;//�� ������ �� �غ� �Ѵ�.
    }
}
