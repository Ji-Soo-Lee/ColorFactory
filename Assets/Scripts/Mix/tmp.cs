using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class tmp : MonoBehaviour
{
    AnswerToken answerToken;
    public int stage;
    private void Start()
    {
        answerToken = GameObject.Find("TargetImage").GetComponent<AnswerToken>();
    }
    public void OnClick()
    {
        answerToken.SetTargetColor(stage);
    }
}
