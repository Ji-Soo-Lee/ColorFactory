using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Image : MonoBehaviour
{
    const int DES=2000;
    Texture2D texture;
    public void assign_image(string file)
    {
        byte[] image = File.ReadAllBytes(file);
        this.texture = new Texture2D(2, 2);
        this.texture.LoadImage(image);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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
        this.texture.Apply();
    }
    void FloodFill(int x, int y, Color fill)
    {//텍스쳐의 해당 좌표 색칠하기
        Queue<Vector2Int> Q = new Queue<Vector2Int>();
        int[] xmove = new int[4] { -1, 0, 1, 0 };
        int[] ymove = new int[4] { 0, 1, 0, -1 };
        Q.Enqueue(new Vector2Int(x, y));
        Color target = this.texture.GetPixel(x, y);//칠하기 이전 클릭한 곳의 색.
        this.texture.SetPixel(x, y, fill);
        while (Q.Count > 0)
        {
            Vector2Int now = Q.Dequeue();
            for(int i=0; i<=3; i++)
            {
                int tx = now.x + xmove[i];
                int ty = now.y + ymove[i];
                if (tx >= 0 && tx < this.texture.width && ty >= 0 && ty < this.texture.height)
                {//그림의 범위를 벗어나지 않는 경우
                    if (this.texture.GetPixel(tx, ty) == target)
                    {//target과 인접한 색들을 모두 바꾼다.
                        this.texture.SetPixel(tx, ty, fill);
                        Q.Enqueue(new Vector2Int(tx, ty));
                    }
                }
            }
        }
    }
}
