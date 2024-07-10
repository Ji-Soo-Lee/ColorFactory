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
    int row = 2;//���� ��
    int column = 2;//���� ��
    int kind = 3;//���� ��
    void Start()
    {
        this.game = GameManager.game;
    }
    void Update()
    {//Ÿ�̸Ӱ� ���ư���.
        elapsed += Time.deltaTime;
        if(this.elapsed>this.limit && this.generated==false)
        {//������ ������ �� ���� �ð��� ������ ��� ����� ���� �����Ѵ�.
            play_problem();
        }
    }
    public void make_problem()
    {//���� �����
        this.elapsed = 0; this.generated = false;//������ �����Ѵ�.
        for(int i=0; i<this.row; i++)
        {//�켱 ��ϵ��� ��ġ�ϰ�, ������ ���� �����Ѵ�.
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
    {//������ �� ������ ����� �÷��̾ ������ �� �� �ְ� �Ѵ�.
        this.generated = true;
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in allBlocks)
        {//��ϵ��� ���� �����Ѵ�.
            Renderer renderer = block.GetComponent<Renderer>();
            renderer.material = game.color[0];
        }
        for(int i=1; i<=this.kind; i++)
        {//�Ʒ��� �ȷ�Ʈ�� ��ġ�Ѵ�.
            Vector3 position = new Vector3(-4 + 2 * i, -4, 0);
            GameObject palette = Instantiate(this.palettePrefab, position, Quaternion.identity);
            palette.GetComponent<Palette>().assign_color(i, game.color[i]);
        }
        game.playable = true;
    }
}
