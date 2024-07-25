using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Image : MonoBehaviour
{
    Texture2D texture;
    Color[] pixels;
    public void assign_image(string file)
    {
        byte[] image = File.ReadAllBytes(file);
        this.texture = new Texture2D(2, 2);
        this.texture.LoadImage(image);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        this.pixels = this.texture.GetPixels();
    }
    void OnMouseDown()
    {//클릭한 곳 기준으로 색칠하기
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//클릭한 곳
        Vector2 spritePosition = gameObject.transform.InverseTransformPoint(mousePosition);//클릭한 곳의 좌표를 그림 기준으로의 좌표로 변환
        float dx = gameObject.GetComponent<SpriteRenderer>().bounds.size.x / gameObject.transform.localScale.x;
        float dy = gameObject.GetComponent<SpriteRenderer>().bounds.size.y / gameObject.transform.localScale.y;
        Vector2 textureUV = new Vector2(spritePosition.x / dx + 0.5f, spritePosition.y / dy + 0.5f);
        int x = Mathf.RoundToInt(textureUV.x * this.texture.width);
        int y = Mathf.RoundToInt(textureUV.y * this.texture.height);
        FloodFill(x, y, Color.red);
    }
    void FloodFill(int x, int y, Color fill)
    {//텍스쳐의 해당 좌표 색칠하기
        Queue<Vector2Int> Q = new Queue<Vector2Int>();
        bool[] check = new bool[this.pixels.Length];
        int[] xmove = new int[4] { -1, 0, 1, 0 };
        int[] ymove = new int[4] { 0, 1, 0, -1 };
        Q.Enqueue(new Vector2Int(x, y));
        Color target = this.pixels[index(x, y)];//칠하기 이전 클릭한 곳의 색.
        this.pixels[index(x, y)] = fill; check[index(x, y)] = true;
        int tmp = 0;
        while (Q.Count > 0)
        {
            Vector2Int now = Q.Dequeue();
            tmp += 1;
            for(int i=0; i<=3; i++)
            {
                int tx = now.x + xmove[i];
                int ty = now.y + ymove[i];
                if (tx >= 0 && tx < this.texture.width && ty >= 0 && ty < this.texture.height)
                {//그림의 범위를 벗어나지 않는 경우
                    int pos = index(tx, ty);
                    if (this.pixels[pos] == target && !check[pos])
                    {//target과 인접한 색들을 모두 바꾼다.
                        Q.Enqueue(new Vector2Int(tx, ty));
                        this.pixels[pos] = fill; check[pos] = true;
                    }
                }
            }
        }
        Debug.Log(tmp + "개의 칸이 변경되었습니다.");
        this.texture.SetPixels(pixels);
        this.texture.Apply();
        Debug.Log("전체 칸"+this.pixels.Length+" 개, 작업 끝");
    }
    int index(int x,int y)
    {//Flood-fill에서 사용하는 픽셀 인덱스
        return (y * this.texture.width + x);
    }
}
