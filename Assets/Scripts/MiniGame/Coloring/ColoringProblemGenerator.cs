using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
        this.game.StartGame();
    }
    void InitiateProblem()
    {//�������� �� ������ �����Ѵ�.
        int d = Random.Range(0, this.problemset.Length);
        generate_problem(this.problemset[d]);
    }
    void generate_problem(ColoringPage picture)
    {//���� ����
        Addressables.LoadAssetAsync<Sprite>(sprite_base + picture.file + ".png").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite file = handle.Result;
                StartCoroutine(problem_cycle(picture));
            }
            else
            {
                Debug.LogError("������ ã�� ���߽��ϴ�.");
            }
        };
    }
    IEnumerator problem_cycle(ColoringPage picture)
    {//���� ����Ŭ
        show_problem(picture);//���� �����ֱ�
        yield return new WaitForSeconds(5.0f);
        ColorEntry[] palette = picture.palette;
        make_palette(palette);//�ȷ�Ʈ �����ϱ�
        this.game.toggle_player(true);
    }
    void show_problem(ColoringPage picture)
    {//���� �����ֱ�. �� ������ ������(������) �ϳ��� ���� ������� �̷���� �ִ�.
        float scale = picture.scale;//�׸��� ũ��
        ColorEntry[] answer = picture.answer;
        int total = picture.count;//������ ����
        for (int i = 0; i <= total; i++)
        {
            Color color = (i == 0 ? Color.black : answer[i - 1].get_color());
            bool is_frame = (i == 0);
            Addressables.LoadAssetAsync<Sprite>(sprite_base + picture.file + ".png[" + picture.file + "_" + i + "]").Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Sprite sprite = handle.Result;
                    if (is_frame)
                    {
                        this.frame.GetComponent<SpriteRenderer>().sprite = sprite;
                    }
                    else
                    {
                        GameObject part = Instantiate(this.partPrefab, this.frame.transform);
                        part.GetComponent<SpriteRenderer>().sprite = sprite;//��ĥ�ϴ� ������ ��������Ʈ�� �ο��Ѵ�.
                        part.AddComponent<PolygonCollider2D>();//�� ��������Ʈ�� �´� �ݶ��̴��� �����Ѵ�.
                        StartCoroutine(part.GetComponent<ColorPart>().SetAnswer(color));
                    }
                }
                else
                {
                    Debug.LogError("������ ã�� ���߽��ϴ�.");
                }
            };
        }
        this.frame.transform.localScale = new Vector3(scale, scale, 1);//�׸� ũ�� �����ϱ�.
    }
    void make_palette(ColorEntry[] palette)
    {//�������� �� �ȷ�Ʈ �����ϱ�.
        for (int i = 0; i < palette.Length; i++)
        {
            Color color = palette[i].get_color();
            GameObject btn = Instantiate(this.buttonPrefab, this.panel.transform);
            btn.transform.localScale = new Vector3(3, 3, 1);
            btn.transform.localPosition = new Vector3(30 + 105 * (i % 8), -50 - 36 * (i / 8), 0);
            btn.GetComponent<Image>().color = color;
            Button button = btn.GetComponent<Button>();
            button.onClick.AddListener(() => change_color(color));
        }
    }
    void change_color(Color x)
    {//��ư�� �ִ� �Լ�. ���� �� �ٲٱ�.
        this.game.now_color = x;
    }
}
