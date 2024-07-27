using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoringGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ColoringGameManager game;
    public Color now_color;
    public bool playable = false;
    const int TOTAL = 3;//�� ���� ��
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
    public void judge_answer()
    {//��� ä���ϱ�
        if (this.playable == true)
        {
            this.playable = false;
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in obj)
            {
                ColorPart part = x.GetComponent<ColorPart>();
                Debug.Log(part.verdict());
                Destroy(x);
            }
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
