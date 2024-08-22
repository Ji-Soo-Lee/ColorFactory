using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainProblemGenerator : MonoBehaviour
{
    BrainGameManager game;
    public GameObject blockPrefab;
    public GameObject[] palette;
    int row = 2;
    int column = 2;
    int kind = 3;
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
    {
        this.game.now_color = x;
    }
    public void InitiateProblem()
    {
        StartCoroutine(ProblemCycle());
    }
    IEnumerator ProblemCycle()
    {
        this.game.remain = (this.row * this.column);
        yield return new WaitForSeconds(0.3f);
        prepare_problem();
        yield return new WaitForSeconds(3.0f);
        game.toggle_player(true);
    }
    void prepare_problem()
    {
        for (int i = 0; i < this.row; i++)
        {
            for (int j = 0; j < this.column; j++)
            {
                Vector3 position = new Vector3((-0.5f * (this.column - 1)) + j * 0.75f, (0.5f * (this.row - 1)) - i * 0.75f, 0);
                GameObject block = Instantiate(this.blockPrefab, position, Quaternion.identity);
                int x = Random.Range(0, this.kind);
                StartCoroutine(block.GetComponent<Block>().assign_color(this.colors[x]));
            }
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
        this.game.stageTimeLimits[this.game.currentStage + 1] += 1.0f;
    }
    void stop_problem()
    {
        StopAllCoroutines();
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        // foreach (GameObject x in obj)
        // {
        //     Destroy(x);
        // }
        StartCoroutine(delayedDestroy(obj, 0.4f));
    }

    IEnumerator delayedDestroy(GameObject[] obj, float time)
    {
        yield return new WaitForSeconds(time);
        foreach (GameObject x in obj)
        {
            Destroy(x);
        }
    }
}
