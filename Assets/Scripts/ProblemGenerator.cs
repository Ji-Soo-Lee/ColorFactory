using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProblemGenerator : MonoBehaviour
{
    public GameObject canvas;
    string sample = "./Assets/Images/picture1.jpg";
    void Start()
    {
        canvas.GetComponent<Image>().assign_image(sample);
    }
}
