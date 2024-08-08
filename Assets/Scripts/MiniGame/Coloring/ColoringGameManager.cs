using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColoringGameManager : StageManager
{
    // Start is called before the first frame update
    public static ColoringGameManager game;
    public Color now_color;
    public bool playable = false;
    const int TOTAL = 3;//�� ���� ��
    int current = 1;//���� ����
    int hint_used = 0;//�� ����� ��Ʈ ��
    float elapsed = 0.0f;
    public event Action new_problem;//ProblemGenerator

    AudioSource[] sound;
    public GameObject questionboard;

    protected override void Awake()
    {
        //���� �Ŵ����� ���� �̱������� �����ϱ�.
        base.Awake();
    
        if (game == null)
        {
            game = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected override void Start()
    {
        this.sound = GetComponents<AudioSource>();
    }
    public void StartGame()
    {//problem generator�� �غ� ������ ���� �����ϱ�
        new_problem();
    }
    public void toggle_player(bool toggle)
    {
        this.playable = toggle;
    }
    public void hint()
    {//��Ʈ ����ϱ�
        if (!this.playable)
        {
            this.hint_used += 1;
            StartCoroutine(hint_procedure());
        }
    }
    IEnumerator hint_procedure()
    {//��� ��ü ���� 3�ʰ� �ôٰ� �������.
        toggle_player(false);
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in obj)
        {
            ColorPart part = x.GetComponent<ColorPart>();
            if (part != null)
            {
                StartCoroutine(part.hint());
            }
        }
        yield return new WaitForSeconds(3.0f);
        toggle_player(true);
    }
    public void judge_answer()
    {//��� ä���ϱ�
        if (this.playable == true)
        {
            bool wrong = false;
            StopAllCoroutines();
            toggle_player(false);
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {//��� �׸� ������, �ȷ�Ʈ�� player �±װ� �پ� �ִ�.
                ColorPart part = x.GetComponent<ColorPart>();
                if(part!=null)
                {//�׸� ���� Ȯ���ϱ�
                    bool result = part.verdict();
                    if(!result)
                    {
                        StartCoroutine(part.blink());
                        wrong = true;
                    }
                }
            }
            if(!wrong)
            {//��� �´� ���
                this.current += 1;
                this.sound[0].Play();
                foreach (GameObject x in obj)
                {//������ ������ ġ���.
                    Destroy(x);
                }
                GameObject.Find("Frame").GetComponent<SpriteRenderer>().sprite = null;
                if (this.current < TOTAL)
                {//�� ���� ����
                    this.questionboard.GetComponent<TextMeshProUGUI>().text = "Q " + this.current + "/" + TOTAL;
                    new_problem();
                }
                else
                {//���� �����ϱ�
                    EndGame();
                }
            }
            else
            {//Ʋ�� ���� �ִ� ���
                this.sound[1].Play();
                toggle_player(true);
            }
        }//ä���� ���� ���Ŀ��� �������� ġ���.
    }
    protected override void Update()
    {//�ð� ����. ������ Ǫ�� ���϶��� ����.
        if(this.playable)
        {
            this.elapsed += Time.deltaTime;
        }
    }
}
