using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ColoringProblemGenerator: MonoBehaviour
{
    ColoringGameManager game;
    string DexFile = "./Assets/ColoringDex.json";//��������. ���⼭ �������� �� ������ �����Ѵ�.

    public GameObject frame;//��ĥ���� �������� �� ��(����)
    public GameObject partPrefab;//���� ��ĥ�ϴ� �κ�
    public GameObject panel;
    public GameObject buttonPrefab;
    ColoringPage[] problemset;

    string sprite_base = "Assets/MyResources/Sprites/";//��������Ʈ ������ ���.
    void Start()
    {//�������� ��������
        this.game = ColoringGameManager.game;
        game.new_problem += InitiateProblem;
        string dex = File.ReadAllText(DexFile);//�������� ���� �б�
        ColoringDex book = JsonUtility.FromJson<ColoringDex>(dex);
        this.problemset = book.problemset;
        InitiateProblem();
    }
    void InitiateProblem()
    {//�������� �� ������ �����Ѵ�.
        int d = Random.Range(0, this.problemset.Length);
        generate_problem(this.problemset[d]);
    }
    void generate_problem(ColoringPage picture)
    {//���� ����
        Sprite file = AssetDatabase.LoadAssetAtPath<Sprite>(sprite_base + picture.file);//������ �� ����
        if (file == null)
        {//������ ���ų� ��ΰ� �߸��� ���
            Debug.Log("������ ã�� ���߽��ϴ�.");
            return;
        }
        StartCoroutine(problem_cycle(picture));
    }
    IEnumerator problem_cycle(ColoringPage picture)
    {//���� ����Ŭ
        show_problem(picture);//���� �����ֱ�
        yield return new WaitForSeconds(5.0f);
        ColorEntry[] palette = picture.palette;
        make_palette(palette);//�ȷ�Ʈ �����ϱ�
        this.game.playable = true;
    }
    void show_problem(ColoringPage picture)
    {//���� �����ֱ�. �� ������ ������(������) �ϳ��� ���� ������� �̷���� �ִ�.
        Object[] problem = AssetDatabase.LoadAllAssetRepresentationsAtPath(sprite_base + picture.file);//������ �����ϴ� ��������Ʈ. �ϳ��� �������� ���� ��ĥ�ϴ� �κе�� ������.
        float scale = picture.scale;//�׸��� ũ��
        ColorEntry[] answer = picture.answer;
        for (int i = 0; i < (problem.Length - 1); i++)
        {//�׸� �������� ��������Ʈȭ �ϰ� ȭ�鿡 �����ش�.
            Sprite sprite = problem[i] as Sprite;
            if (sprite == null)
            {//�ε� �� ���� �߻�
                Debug.LogError("������ �ҷ����� �� ������ �߻��߽��ϴ�.");
                return;
            }
            if (i == 0)
            {//������. ��ĥ�ϴ� �κ��� �ƴϸ� �׸� �������� �����ϴ� ������ �Ѵ�.
                this.frame.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {//�׸� ����. �÷��̾ Ŭ���� ��ĥ�ϴ� �κ��̴�. �̵��� frame�� �ڼտ� �ִ´�.
                GameObject part = Instantiate(this.partPrefab, this.frame.transform);
                part.GetComponent<SpriteRenderer>().sprite = sprite;//��ĥ�ϴ� ������ ��������Ʈ�� �ο��Ѵ�.
                part.AddComponent<PolygonCollider2D>();//�� ��������Ʈ�� �´� �ݶ��̴��� �����Ѵ�.
                StartCoroutine(part.GetComponent<ColorPart>().SetAnswer(answer[i - 1].get_color()));
            }
        }//��� �׸� ������ ������ �κ��� ����ε�, �̰��� ��쿡 ���� �� ���� �ְ� �� �� ���� �ִ�.(���� ����)
        this.frame.transform.localScale = new Vector3(scale, scale, 1);//�׸� ũ�� �����ϱ�.
    }
    void make_palette(ColorEntry[] palette)
    {//�������� �� �ȷ�Ʈ �����ϱ�.
        for (int i = 0; i < palette.Length; i++)
        {
            Color color = palette[i].get_color();
            GameObject btn = Instantiate(this.buttonPrefab, this.panel.transform);
            btn.transform.localPosition = new Vector3(18 + 36 * (i % 7), -120 - 36 * (i / 7), 0);
            Button button = btn.GetComponent<Button>();
            ColorBlock cb = button.colors;
            cb.normalColor = color; cb.highlightedColor = color; cb.selectedColor = color;
            button.colors = cb;
            button.onClick.AddListener(() => change_color(color));
        }
    }
    void change_color(Color x)
    {//��ư�� �ִ� �Լ�. ���� �� �ٲٱ�.
        this.game.now_color = x;
    }
}
