using UnityEngine;

public class TempTanmak : MonoBehaviour
{
    [HideInInspector]
    public Color color;

    void Start()
    {
    
        var tgmobj = GameObject.Find("Tanmak Game Manager Object");

        if (tgmobj != null)
        {
            // set initial tanmak color
            TanmakGameManager tanmakGM = tgmobj.GetComponent<TanmakGameManager>();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            color = tanmakGM.colors[UnityEngine.Random.Range(0, TanmakGameManager.colorSize)];
            spriteRenderer.color = color;
        }
    }
}
