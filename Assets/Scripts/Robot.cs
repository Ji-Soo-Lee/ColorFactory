using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : need to fix
public class Robot : MonoBehaviour
{
    [SerializeField] float clickPeriod = 1f;
    [SerializeField] int clickAmount = 10;
    WaitForSeconds wait;

    [SerializeField] ButtonManager buttonManager;

    private void Start()
    {
        wait = new WaitForSeconds(clickPeriod);
        StartCoroutine(ClickRoutine());
    }

    IEnumerator ClickRoutine()
    {
        while(false)
        {
            buttonManager.Click(clickAmount);
            yield return wait;
        }
    }
}
