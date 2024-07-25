using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackgroundManager : MonoBehaviour
{
    public Image backgroundSprite;
    [SerializeField] ButtonManager buttonManager;
    // Start is called before the first frame update
    void Start()
    {
        // buttonManager.backgrounds.Add(backgroundSprite);
    }
}
