using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainProblemGenerator : MonoBehaviour
{
    BrainGameManager game;
    public GameObject canvas;
    public GameObject blockPrefab;
    // public GameObject[] palette;
    int row = 2;
    int column = 2;
    int kind = 3;
    public Color[] colors;
    [SerializeField] private Vector3 center;

    void Start()
    {
        this.game = BrainGameManager.game;
        game.new_problem += InitiateProblem;
        game.stop_problem += stop_problem;
        game.difficulty_increase += difficulty_increase;
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
        game.ActivateButton();
    }
    void prepare_problem()
    {
        for (int i = 0; i < this.row; i++)
        {
            for (int j = 0; j < this.column; j++)
            {
                Vector3 position = new Vector3((-25f * (this.column - 1)) + j * 50f, (25f * (this.row - 1)) - i * 50f, 0);
                GameObject block = Instantiate(this.blockPrefab, center, Quaternion.identity) as GameObject;
                block.transform.SetParent(canvas.transform, false);
                block.transform.SetSiblingIndex(1);
                block.transform.localPosition = position;
                // Debug.Log("BLOCK " + block.transform.position);
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
        this.game.stageTimeLimit += 1.0f;
    }
    void stop_problem()
    {
        StopAllCoroutines();
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in obj)
        {
            Destroy(x);
        }
    }

    // void pause_problem()
    // {
    //     StopAllCoroutines();
    //     GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
    //     foreach (GameObject x in obj)
    //     {
    //         x.GetComponent<Button>().interactable = false;
    //     }
    // }
}
