using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimonGameManager : MonoBehaviour
{
    public static SimonGameManager game;
    public bool playable = false;

    public GameObject[] buttons;
    public GameObject scoreboard;
    float elapsed = 0.0f;//Ÿ�̸�
    Bulb bulb;

    Color[] kind = new Color[4] { Color.red, Color.yellow, Color.green, Color.blue };
    Color[] answer = new Color[100];
    int level = 1;//���� ����. ������ �� ��ŭ�� ���� �ܿ��� �Ѵ�.
    int current = 0;

    public GameObject DummyEndGamePannel;
    int score = 5;

    public void EndGame()
    {
        ScoreDataManager.Inst.SaveResult(score);
        DummyEndGamePannel.SetActive(true);
    }

    void Awake()
    {//���� �Ŵ����� ���� �̱������� �����ϱ�.
        if (game == null)
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
        this.bulb = GameObject.Find("Bulb").GetComponent<Bulb>();
        for (int i = 0; i < this.buttons.Length; i++)
        {//�� ��ư�� �Լ� �ֱ�
            Button button = this.buttons[i].GetComponent<Button>();
            Color color = this.kind[i];
            button.onClick.AddListener(() => check_color(color));
        }
        toggle_player(false);
        StartCoroutine(show_problem(1.0f));
    }
    IEnumerator show_problem(float time)
    {//������ �����ش�.
        yield return new WaitForSeconds(time);
        for (int i = 0; i < this.level; i++)
        {
            Color now;
            if (i == (this.level - 1))
            {//���ο� �� ���� �����ϱ�
                now = this.kind[Random.Range(0, 4)];
                this.answer[i] = now;
            }
            else
            {//������ �������� �� �����ֱ�
                now = this.answer[i];
            }
            StartCoroutine(this.bulb.blink(now));
            yield return new WaitForSeconds(0.7f);
        }
        toggle_player(true);
        this.current = 0;
    }
    void toggle_player(bool toggle)
    {//�÷��̾��� �Է� �������� üũ�ϱ�
        this.elapsed = 0.0f;
        this.playable = toggle;
        for(int i=0; i<this.buttons.Length; i++)
        {
            Button button = this.buttons[i].GetComponent<Button>();
            button.interactable = toggle;
        }
    }
    void check_color(Color color)
    {//������ �´� ���� �������� Ȯ���ϱ�
        if(this.playable)
        {
            StopAllCoroutines();
            StartCoroutine(this.bulb.blink(color));
            if (this.answer[this.current] == color)
            {//������ �´� ���� ���� ���
                this.elapsed = 0.0f;
                this.current += 1;
                if (this.current >= this.level)
                {//���� �������� ��� ���� ���
                    toggle_player(false);
                    this.scoreboard.GetComponent<TextMeshProUGUI>().text = this.level.ToString();
                    this.current = 0; this.level += 1;
                    if (this.level <= 100)
                    {//���� ������ �Ѿ��
                        StartCoroutine(show_problem(1.0f));
                    }
                }
            }
            else
            {//������ ���� �ʴ� ���� ���� ��� ��� ������ �����Ѵ�.
                toggle_player(false);
                Debug.Log(this.level + " ���� Ʋ�Ƚ��ϴ�");
            }
        }
    }
    void Update()
    {//Ÿ�̸�
        this.elapsed += Time.deltaTime;
        if(this.elapsed>=5.0f && this.playable==true)
        {//�÷��̾� �����ε� 5�� �̻� ���� ���� ���� ���
            toggle_player(false);
            Debug.Log(this.level + " ���� �ð��ʰ� �Ǿ����ϴ�");
        }
    }
}
