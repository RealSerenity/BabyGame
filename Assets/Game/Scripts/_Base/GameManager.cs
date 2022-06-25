using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    public enum GameState
    {
        BeforeStart,
        Normal,
        Finish,
        Failed,
        Victory,
        Pause
    }

    [Header("Template Settings")] [SerializeField]
    private bool isDebug = true;

    [SerializeField] private bool startLevelMusicOnPlay = false;

    [SerializeField] private bool giveInputOnFirstClick = false;

    [SerializeField] private bool takeInputCount = false;

    [SerializeField] private bool showMenuOnNewSceneLoaded = false;

    [SerializeField] private bool useAppMetrica = false;

    [SerializeField] private bool useAppsFlyer = false;

    [SerializeField] private bool clearAllPoolObjectsOnNewLevelLoad = false;

    [SerializeField] private bool giveInputToUser = false;

    public bool GiveInputToUser
    {
        get { return giveInputToUser; }
        set { giveInputToUser = value; }
    }

    public bool IsDebug
    {
        get { return isDebug; }
    }

    public bool GiveInputOnFirstClick
    {
        get { return giveInputOnFirstClick; }
    }

    public bool TakeInputCount
    {
        get { return takeInputCount; }
    }

    public bool ShowMenuOnNewSceneLoaded
    {
        get { return showMenuOnNewSceneLoaded; }
    }

    public bool UseAppMetrica
    {
        get { return useAppMetrica; }
    }

    public bool UseAppsFlyer
    {
        get { return useAppsFlyer; }
    }

    public bool ClearAllPoolObjectsOnNewLevelLoad
    {
        get { return clearAllPoolObjectsOnNewLevelLoad; }
    }

    public bool StartLevelMusicOnPlay
    {
        get { return startLevelMusicOnPlay; }
    }

    public static Action onWinEvent;
    public static Action onLoseEvent;


    [Header("Game Settings")] public GameState currentState = GameState.BeforeStart;

    public float playerSpeed = 5f;
    public float playerSmooth = 5f;
    public int playerChances = 3;

    public Action onFeedTake;
    public Action onHitObstacle;
    public Action onFinishEvent;
    public Transform finishline;
    public Transform actor;
    public Spoon spoon;
    public GameObject currentCamera;
    private UIManager UImanager;
    private void Start()
    {
        if (IsDebug)
        {
            OnNewLevelLoaded();
        }

        LevelManager.onLevelRendered += OnNewLevelLoaded;
        UIManager manager = FindObjectOfType<UIManager>();
        onHitObstacle += IsGameOver;
    }


    private void OnNewLevelLoaded()
    {
        UImanager = FindObjectOfType<UIManager>();
        UImanager.UpdateLevelText();
        playerChances = 3;
        actor = GameObject.FindGameObjectWithTag("Player").transform;
        finishline = GameObject.FindGameObjectWithTag("FinishLine").transform;
        UImanager.SetProgresBarMaxValue(finishline.transform.position.z);
        spoon = FindObjectOfType<Spoon>();
        spoon.takenMamas = 0;
        UImanager.UpdateMamaText();
        currentCamera = GameObject.FindGameObjectWithTag("CurrentCam");
        for (int i = 0; i < 3; i++)
        {
            UImanager.kalpler[i].sprite = UImanager.doluKalp;
        }
    }

    private void OnDisable()
    {
        LevelManager.onLevelRendered -= OnNewLevelLoaded;
        onHitObstacle -= IsGameOver;
    }

    void Update()
    {
        if (actor == null)
        {
            actor = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (GameManager.Instance.currentState == GameState.Finish)
        {
            currentCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            onFinishEvent?.Invoke();
            
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (isDebug == true)
        {
            Debug.LogWarning("Debug Mode Active");
        }


        Application.targetFrameRate = 60;
    }

    void IsGameOver()
    {
        playerChances--;
        UImanager.UpdateCanText();
        if (playerChances <= 0)
        {
            currentState = GameState.Failed;
            onLoseEvent?.Invoke();
        }
    }

}