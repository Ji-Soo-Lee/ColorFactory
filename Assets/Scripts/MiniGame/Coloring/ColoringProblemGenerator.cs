using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
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
        InitiateProblem();
    }
    void InitiateProblem()
    {//무작위로 한 문제를 출제한다.
        int d = Random.Range(0, this.problemset.Length);
        generate_problem(this.problemset[d]);
    }
    void generate_problem(ColoringPage picture)
    {//문제 내기
        Sprite file = AssetDatabase.LoadAssetAtPath<Sprite>(sprite_base + picture.file);//문제로 쓸 파일
        if (file == null)
        {//파일이 없거나 경로가 잘못된 경우
            Debug.Log("파일을 찾지 못했습니다.");
            return;
        }
        StartCoroutine(problem_cycle(picture));
    }
    IEnumerator problem_cycle(ColoringPage picture)
    {//출제 사이클
        show_problem(picture);//문제 보여주기
        yield return new WaitForSeconds(5.0f);
        ColorEntry[] palette = picture.palette;
        make_palette(palette);//팔레트 생성하기
        this.game.playable = true;
    }
    void show_problem(ColoringPage picture)
    {//문제 보여주기. 한 문제는 윤곽선(프레임) 하나와 여러 조각들로 이루어져 있다.
        Object[] problem = AssetDatabase.LoadAllAssetRepresentationsAtPath(sprite_base + picture.file);//문제를 구성하는 스프라이트. 하나의 윤곽선과 여러 색칠하는 부분들로 나뉜다.
        float scale = picture.scale;//그림의 크기
        ColorEntry[] answer = picture.answer;
        for (int i = 0; i < (problem.Length - 1); i++)
        {//그림 조각들을 스프라이트화 하고 화면에 보여준다.
            Sprite sprite = problem[i] as Sprite;
            if (sprite == null)
            {//로딩 중 오류 발생
                Debug.LogError("파일을 불러오는 중 오류가 발생했습니다.");
                return;
            }
            if (i == 0)
            {//윤곽선. 색칠하는 부분은 아니며 그림 조각들을 구분하는 역할을 한다.
                this.frame.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {//그림 조각. 플레이어가 클릭해 색칠하는 부분이다. 이들은 frame의 자손에 넣는다.
                GameObject part = Instantiate(this.partPrefab, this.frame.transform);
                part.GetComponent<SpriteRenderer>().sprite = sprite;//색칠하는 영역의 스프라이트를 부여한다.
                part.AddComponent<PolygonCollider2D>();//그 스프라이트에 맞는 콜라이더를 배정한다.
                StartCoroutine(part.GetComponent<ColorPart>().SetAnswer(answer[i - 1].get_color()));
            }
        }//모든 그림 파일의 마지막 부분은 배경인데, 이것은 경우에 따라 쓸 수도 있고 안 쓸 수도 있다.(수정 예정)
        this.frame.transform.localScale = new Vector3(scale, scale, 1);//그림 크기 조정하기.
    }
    void make_palette(ColorEntry[] palette)
    {//문제에서 쓸 팔레트 생성하기.
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
    {//버튼에 넣는 함수. 현재 색 바꾸기.
        this.game.now_color = x;
    }
}
