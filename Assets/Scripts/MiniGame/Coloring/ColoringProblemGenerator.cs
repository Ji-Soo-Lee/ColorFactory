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
    string DexFile = "./Assets/ColoringDex.json";//문제은행. 여기서 무작위로 한 문제를 출제한다.

    public GameObject frame;//색칠놀이 윤곽선이 들어갈 곳(액자)
    public GameObject partPrefab;//실제 색칠하는 부분
    public GameObject panel;
    public GameObject buttonPrefab;
    ColoringPage[] problemset;

    string sprite_base = "Assets/MyResources/Sprites/";//스프라이트 파일의 경로.
    void Start()
    {//문제은행 가져오기
        this.game = ColoringGameManager.game;
        game.new_problem += InitiateProblem;
        string dex = File.ReadAllText(DexFile);//문제은행 파일 읽기
        ColoringDex book = JsonUtility.FromJson<ColoringDex>(dex);
        this.problemset = book.problemset;
        this.game.StartGame();
    }
    void InitiateProblem()
    {//무작위로 한 문제를 출제한다.
        int d = Random.Range(0, this.problemset.Length);
        generate_problem(this.problemset[d]);
    }
    void generate_problem(ColoringPage picture)
    {//문제 내기
        Addressables.LoadAssetAsync<Sprite>(sprite_base + picture.file + ".png").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite file = handle.Result;
                StartCoroutine(problem_cycle(picture));
            }
            else
            {
                Debug.LogError("파일을 찾지 못했습니다.");
            }
        };
    }
    IEnumerator problem_cycle(ColoringPage picture)
    {//출제 사이클
        show_problem(picture);//문제 보여주기
        yield return new WaitForSeconds(5.0f);
        ColorEntry[] palette = picture.palette;
        make_palette(palette);//팔레트 생성하기
        this.game.toggle_player(true);
    }
    void show_problem(ColoringPage picture)
    {//문제 보여주기. 한 문제는 윤곽선(프레임) 하나와 여러 조각들로 이루어져 있다.
        float scale = picture.scale;//그림의 크기
        ColorEntry[] answer = picture.answer;
        int total = picture.count;//조각의 개수
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
                        part.GetComponent<SpriteRenderer>().sprite = sprite;//색칠하는 영역의 스프라이트를 부여한다.
                        part.AddComponent<PolygonCollider2D>();//그 스프라이트에 맞는 콜라이더를 배정한다.
                        StartCoroutine(part.GetComponent<ColorPart>().SetAnswer(color));
                    }
                }
                else
                {
                    Debug.LogError("파일을 찾지 못했습니다.");
                }
            };
        }
        this.frame.transform.localScale = new Vector3(scale, scale, 1);//그림 크기 조정하기.
    }
    void make_palette(ColorEntry[] palette)
    {//문제에서 쓸 팔레트 생성하기.
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
    {//버튼에 넣는 함수. 현재 색 바꾸기.
        this.game.now_color = x;
    }
}
