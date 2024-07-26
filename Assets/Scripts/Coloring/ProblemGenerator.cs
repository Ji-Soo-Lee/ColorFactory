using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ProblemGenerator : MonoBehaviour
{
    public GameObject frame;//색칠놀이 윤곽선(검은선)
    public GameObject partPrefab;//실제 색칠하는 부분
    string sample = "Assets/Sprites/problem_zzangu.png";
    void Start()
    {//테스트용
        Sprite file = AssetDatabase.LoadAssetAtPath<Sprite>(sample);//문제로 쓸 파일
        if(file==null)
        {
            Debug.Log("파일을 찾지 못했습니다.");
            return;
        }
        Object[] problem = AssetDatabase.LoadAllAssetRepresentationsAtPath(sample);//문제를 구성하는 스프라이트. 하나의 윤곽선과 여러 색칠하는 부분들로 나뉜다. 
        for (int i = 0; i < problem.Length; i++)
        {//이들 각각을 스프라이트화 하고 화면에 띄운다.
            Sprite sprite = problem[i] as Sprite;
            if (sprite == null)
            {
                Debug.LogError("파일을 불러오는 중 오류가 발생했습니다.");
                return;
            }
            if(i==0)
            {//윤곽선 부분
                this.frame.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {//색칠하는 부분
                GameObject part = Instantiate(this.partPrefab, this.frame.transform);
                part.GetComponent<SpriteRenderer>().sprite = sprite;//색칠하는 영역의 스프라이트를 부여한다.
                part.AddComponent<PolygonCollider2D>();//그 스프라이트에 맞는 콜라이더를 준다.
                part.GetComponent<ColorPart>().x = i;
            }//각 부분들은 frame의 자손으로 넣는다.
        }
        float d = 0.7f;
        this.frame.transform.localScale = new Vector3(d, d, d);
    }
}
