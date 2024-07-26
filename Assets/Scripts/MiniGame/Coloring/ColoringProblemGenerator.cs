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
    string sprite_base = "Assets/Sprites/";
    void Start()
    {//문제은행
        this.game = ColoringGameManager.game;
        string dex = File.ReadAllText(DexFile);//문제은행 파일 읽기
        ColoringDex book = JsonUtility.FromJson<ColoringDex>(dex);
        this.problemset = book.problemset;
        Debug.Log(this.problemset[0].file);
        pick_problem(this.problemset[0]);
    }
    void pick_problem(ColoringPage picture)
    {//문제 내기
        Sprite file = AssetDatabase.LoadAssetAtPath<Sprite>(sprite_base + picture.file);//문제로 쓸 파일
        if (file == null)
        {
            Debug.Log("파일을 찾지 못했습니다.");
            return;
        }
        float scale = picture.scale;
        Object[] problem = AssetDatabase.LoadAllAssetRepresentationsAtPath(sprite_base + picture.file);//문제를 구성하는 스프라이트. 하나의 윤곽선과 여러 색칠하는 부분들로 나뉜다. 
        for (int i = 0; i < problem.Length; i++)
        {//이들 각각을 스프라이트화 하고 화면에 띄운다.
            Sprite sprite = problem[i] as Sprite;
            if (sprite == null)
            {
                Debug.LogError("파일을 불러오는 중 오류가 발생했습니다.");
                return;
            }
            if (i == 0)
            {//윤곽선 부분
                this.frame.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {//색칠하는 부분
                GameObject part = Instantiate(this.partPrefab, this.frame.transform);
                part.GetComponent<SpriteRenderer>().sprite = sprite;//색칠하는 영역의 스프라이트를 부여한다.
                part.AddComponent<PolygonCollider2D>();//그 스프라이트에 맞는 콜라이더를 준다.
            }//각 부분들은 frame의 자손으로 넣는다.
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
    {//버튼에 넣는 함수. 현재 색 바꾸기.
        this.game.now_color = x;
    }
}
