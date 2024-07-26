/*문제은행에 담긴 정보들.*/
using UnityEngine;

[System.Serializable]
public class ColorEntry
{//팔레트에 들어갈 색
    public string color;//HEX 컬러코드
    public Color get_color()
    {//컬러코드를 색 오브젝트로 변환하기
        if (ColorUtility.TryParseHtmlString(color, out Color result))
        {
            return result;
        }
        else
        {
            Debug.LogError("Invalid color code: " + color);
            return Color.white; // Or a default color
        }
    }
}

[System.Serializable]
public class ColoringPage
{//각 문제별 정보
    public string file;//파일명
    public float scale;//크기 조정
    public ColorEntry[] palette;//팔레트에 들어갈 색
}

[System.Serializable]
public class ColoringDex
{//문제 모음
    public ColoringPage[] problemset;
}