using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoringGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ColoringGameManager game;
    public Color now_color;
    const int TOTAL = 3;//총 문제 수
    void Awake()
    {//게임 매니저를 전역 싱글톤으로 설정하기.
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
