using UnityEngine;

public class TempTanmak : MonoBehaviour
{
    [HideInInspector]
    public Color color;

    // Destroy bullet if touched Tanmak Border
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TanmakBorder"))
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        var tgmobj = GameObject.Find("Tanmak Game Manager Object");

        if (tgmobj != null)
        {
            // set initial tanmak color
            TanmakGameManager tanmakGM = tgmobj.GetComponent<TanmakGameManager>();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            string prefabName = gameObject.name.Replace("(Clone)", "").Trim();
            Debug.Log($"Prefab Name: {prefabName}");

            // If color is assigned by prefab name
            if (tanmakGM.bulletColorMapping.TryGetValue(prefabName, out color))
            {
                spriteRenderer.color = color;
            }
            else 
            {
                // random color among colors list
                color = tanmakGM.colors[UnityEngine.Random.Range(0, TanmakGameManager.colorSize)];
                spriteRenderer.color = color;
            }
        }
    }
}
