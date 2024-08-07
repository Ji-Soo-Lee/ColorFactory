using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pattern
{
    public string Name;
    public float Time;
    public Vector3 InitPos;
    public Vector3 InitDirection;
}

public class TanmakPatternsIntepreter : MonoBehaviour
{
    public GameObject bulletPrefab1; // First Color
    public GameObject bulletPrefab2; // Second Color 
    public GameObject bulletPrefab3; // Third Color

    float timer = 0f;
    Pattern[] patterns;
    int nextPatternIdx = -0;
    bool isTimer = true;


}
