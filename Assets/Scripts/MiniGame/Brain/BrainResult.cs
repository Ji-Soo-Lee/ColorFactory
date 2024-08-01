using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BrainResult : MonoBehaviour
{
    int score;
    int bonus;
    public GameObject scoretext;
    public GameObject bonustext;
    public GameObject DummyEndGamePannel;
    void Start()
    {
        this.score = PlayerPrefs.GetInt("score");
        this.bonus = PlayerPrefs.GetInt("bonus");
        ScoreDataManager.Inst.SaveResult(this.score);
        StartCoroutine(announce_result());
    }
    IEnumerator announce_result()
    {
        yield return new WaitForSeconds(1.0f);
        this.scoretext.SetActive(true);
        this.scoretext.GetComponent<TextMeshProUGUI>().text = this.score.ToString();
        yield return new WaitForSeconds(1.0f);
        this.bonustext.SetActive(true);
        this.bonustext.GetComponent<TextMeshProUGUI>().text = this.bonus.ToString();
        yield return new WaitForSeconds(1.0f);
        DummyEndGamePannel.SetActive(true);
    }
}
