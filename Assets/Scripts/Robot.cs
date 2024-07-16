using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO : need to fix
public class Robot : MonoBehaviour
{
    public Button robotButton;
    [SerializeField] int clickAmount = 10;
    [SerializeField] ButtonManager buttonManager;

    private void Awake()
    {
        buttonManager.robots.Add(this);
        robotButton.onClick.AddListener(OnClickButton);
    }

    void OnClickButton()
    {
        buttonManager.RobotClick(clickAmount);
    }
}
