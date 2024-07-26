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
    string sprite_base = "Assets/Sprites/";
    void Start()
    {//��������
        this.game = ColoringGameManager.game;
        string dex = File.ReadAllText(DexFile);//�������� ���� �б�
        ColoringDex book = JsonUtility.FromJson<ColoringDex>(dex);
        this.problemset = book.problemset;
        Debug.Log(this.problemset[0].file);
        pick_problem(this.problemset[0]);
    }
    void pick_problem(ColoringPage picture)
    {//���� ����
        Sprite file = AssetDatabase.LoadAssetAtPath<Sprite>(sprite_base + picture.file);//������ �� ����
        if (file == null)
        {
            Debug.Log("������ ã�� ���߽��ϴ�.");
            return;
        }
        float scale = picture.scale;
        Object[] problem = AssetDatabase.LoadAllAssetRepresentationsAtPath(sprite_base + picture.file);//������ �����ϴ� ��������Ʈ. �ϳ��� �������� ���� ��ĥ�ϴ� �κе�� ������. 
        for (int i = 0; i < problem.Length; i++)
        {//�̵� ������ ��������Ʈȭ �ϰ� ȭ�鿡 ����.
            Sprite sprite = problem[i] as Sprite;
            if (sprite == null)
            {
                Debug.LogError("������ �ҷ����� �� ������ �߻��߽��ϴ�.");
                return;
            }
            if (i == 0)
            {//������ �κ�
                this.frame.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {//��ĥ�ϴ� �κ�
                GameObject part = Instantiate(this.partPrefab, this.frame.transform);
                part.GetComponent<SpriteRenderer>().sprite = sprite;//��ĥ�ϴ� ������ ��������Ʈ�� �ο��Ѵ�.
                part.AddComponent<PolygonCollider2D>();//�� ��������Ʈ�� �´� �ݶ��̴��� �ش�.
            }//�� �κе��� frame�� �ڼ����� �ִ´�.
        }
        this.frame.transform.localScale = new Vector3(scale, scale, 1);
        ColorEntry[] palette = picture.palette; 
        generate_buttons(palette);
    }
    void generate_buttons(ColorEntry[] palette)
    {
        for (int i = 0; i < palette.Length; i++)
        {
            Color color = palette[i].get_color();
            Button button = Instantiate(this.buttonPrefab, this.panel.transform).GetComponent<Button>();
            ColorBlock cb = button.colors;
            cb.normalColor = color; cb.selectedColor = color;
            button.colors = cb;
            button.onClick.AddListener(() => change_color(color));
        }
    }
    void change_color(Color x)
    {//��ư�� �ִ� �Լ�. ���� �� �ٲٱ�.
        this.game.now_color = x;
    }
}
