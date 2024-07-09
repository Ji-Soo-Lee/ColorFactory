using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    public int selected = 0;//���� ���õ� ��
    public bool playable = false;//���� �÷��̾ ���� �� �� �ִ���
    public Material[] color;//���ӿ��� ����ϴ� ��
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

    // Update is called once per frame
    void Update()
    {
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
            {//��ϵ��� ���� �����Ѵ�.
                bool result = block.GetComponent<Block>().verdict();
                if (result == false)
                {//���� Ʋ�� ���
                    wrong = true;
                }
            }
            Debug.Log(!wrong);//���� �¾Ҵ��� Ʋ�ȴ��� ǥ��
            reset_problem();
        }
    }
    void reset_problem()
    {//������ ���� ��� ��ϰ� �ȷ�Ʈ�� ġ���.
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in allBlocks)
        {//��ϵ��� ���� �����Ѵ�.
            Destroy(block);
        }
        GameObject[] allPalettes = GameObject.FindGameObjectsWithTag("Palette");
        foreach (GameObject palette in allPalettes)
        {//�ȷ�Ʈ�� �����Ѵ�.
            Destroy(palette);
        }
        this.start = false; this.elapsed = 0.0f;//�� ������ �� �غ� �Ѵ�.
    }
}
