using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class BombTapGameManager : StageManager
{
    public static BombTapGameManager game;
    public bool playable = false;
    public bool isCleared = false;

    public event Action<int> initiate;

    Color target = Color.white;
    int remain = 0;

    int bomb = 6;
    // float limit = 3.0f;

    // int score = 0;
    // int life = 3;
    int currentStage = 0; // stage
    int maxStage = 10;

    public GameObject scoreboard;
    public GameObject timer;
    public GameObject stageboard;

    // core
    SpriteRenderer sprite;
    const int TOTAL = 7;
    const float R = 1.8f;
    public Color[] dex = new Color[TOTAL] { Color.red, Color.green, Color.blue, Color.magenta, Color.yellow, Color.cyan, Color.white };
    public GameObject bombPrefab;

    public GameObject wrongPopup;

    #if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void Vibrate(long _n);
    # endif

    protected override void Awake()
    {
        base.Awake();

        if(game == null)
        {
            game = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentStage = 0;
        maxStage = 10;
        stageTimeLimits = new float[10] { 3f, 3f, 3f, 3f, 3f, 3f, 3f, 3f, 3f, 3f };
        stageScores = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        isStageActive = false;
        isCleared = false;
    }
    protected override void Start()
    {
        InitializeStageTimer();
        InitializeScoreManager();

        this.sprite = GetComponent<SpriteRenderer>();
        this.initiate += make_problem;

        StartStage(0);
        this.scoreboard.GetComponent<TextMeshProUGUI>().text = "0";
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isStageActive)
        {
            this.timer.GetComponent<TextMeshProUGUI>().text = (Mathf.Ceil(stageTimer.GetTimerValue()*10)/10).ToString("F1");
        }
    }

    protected override void StartStage(int stageIndex)
    {
        currentStage = stageIndex;

        float stageTimeLimit = stageTimeLimits[currentStage];
        stageTimer.SetupTimer(stageTimeLimit, EndStage);

        isStageActive = true;

        stageTimer.StartTimer();
        stageboard.GetComponent<TextMeshProUGUI>().text = "STAGE" + (currentStage + 1).ToString();

        new_problem();
    }

    protected override void EndStage()
    {
        isStageActive = false;
        stageTimer.PauseTimer();
        stageTimer.PauseTimer();

        if (isCleared)
        {
            CalculateFinalScore();
            isCleared = false;
        }
        scoreboard.GetComponent<TextMeshProUGUI>().text = scoreManager.GetScoreAsString();

        if (currentStage < maxStage - 1)
        {
            StartStage(currentStage + 1);
        }
        else
        {
            EndGame();
        }
    }

    protected override void CalculateFinalScore()
    {
        float remainingTime = stageTimer.GetTimerValue();
        OnStageClear(remainingTime);
    }

    public void OnStageClear(float remainingTime)
    {
        int scoreToAdd = Mathf.FloorToInt(remainingTime) * 3;
        scoreManager.AddScore(scoreToAdd);
    }

    private void new_problem()
    {
        Debug.Log(1);
        this.playable = false;
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in obj)
        {
            Destroy(x);
        }
        this.initiate?.Invoke(this.bomb);
    }

    public void set_goal(Color color,int num)
    {
        this.target = color;
        this.remain = num;
    }
    public void judge(Color color)
    {
        if(this.target==color)
        {
            # if UNITY_ANDROID && !UNITY_EDITOR
                Vibration.Vibrate(30);
            # elif UNITY_IOS && !UNITY_EDITOR
                Vibrate(1519);
            # endif

            CalculateFinalScore();
            this.remain -= 1;
            this.scoreboard.GetComponent<TextMeshProUGUI>().text = scoreManager.GetScoreAsString();
            if (this.remain<=0)
            {
                isCleared = true;
                EndStage();
            }
            else
            {
                this.playable = true;
            }
        }
        else
        {
            # if UNITY_ANDROID && !UNITY_EDITOR
                Vibration.Vibrate(60);
            # elif UNITY_IOS && !UNITY_EDITOR
                Vibrate(1521);
            # endif

            wrongPopup.SetActive(true);
            stageTimer.PauseTimer();
            Invoke("HideMessage", 0.5f);
            StartCoroutine(delayedEndStage(0.6f));
        }
    }

    // core
    void make_problem(int num)
    {
        int target = UnityEngine.Random.Range(0, TOTAL);
        Debug.Log(target);
        float d = 360.0f / (float)num;
        int cnt = 0;
        Color key = this.dex[target];
        sprite.color = key;
        for(int i=0; i<num; i++)
        {
            float deg = (d * (float)i);
            Color color;
            Vector3 pos = new Vector3(R * Mathf.Cos(Mathf.Deg2Rad * deg), R * Mathf.Sin(Mathf.Deg2Rad * deg), 0);
            GameObject bomb = Instantiate(this.bombPrefab, pos, Quaternion.identity);
            if(i==(num-1) && cnt==0)
            {
                color = key;
            }
            else
            {
                int x = UnityEngine.Random.Range(0, TOTAL);
                color = this.dex[x];
            }
            cnt += (color == key ? 1 : 0);
            bomb.GetComponent<Bomb>().assign_color(color);
        }
        set_goal(key, cnt);
        this.playable = true;
    }

    protected void HideMessage()
    {
        wrongPopup.SetActive(false);
    }

    IEnumerator delayedEndStage(float time)
    {
        yield return new WaitForSeconds(time);
        EndStage();
    }
}
