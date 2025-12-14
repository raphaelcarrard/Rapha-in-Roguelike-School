using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Game Flow")]
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public int playerFoodPoints = 100;

    [HideInInspector] public bool playerTurn = true;

    private BoardManager boardScript;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    [Header("Level UI")]
    public GameObject levelImage;
    private Text levelText;
    public int level = 1;

    [Header("Tutorial / UI")]
    public GameObject TutorialInfo;

    [Header("Achievements")]
    public GameObject googlePlayAchievements;
    public GameObject googlePlayLeaderboard;
    public GameObject exitGame;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText")?.GetComponent<Text>();
        TutorialInfo = GameObject.Find("Background");

        if (levelText != null)
            levelText.text = "Day " + level;

        if (levelImage != null)
            levelImage.SetActive(true);

        Invoke(nameof(HideLevelImage), levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);

        googlePlayAchievements = GameObject.Find("GooglePlayAchievementsButton");
        googlePlayLeaderboard = GameObject.Find("GooglePlayLeaderboardButton");
        exitGame = GameObject.Find("ExitGameAfterLose");

        if (googlePlayAchievements != null) googlePlayAchievements.SetActive(false);
        if (googlePlayLeaderboard != null) googlePlayLeaderboard.SetActive(false);
        if (exitGame != null) exitGame.SetActive(false);

        playerTurn = true;
        enemiesMoving = false;
        AchievementManager.instance.CheckAchievements();
        AdsManager.Instance.interstitialAds.ShowInterstitialAd();
    }

    void HideLevelImage()
    {
        if (levelImage != null)
            levelImage.SetActive(false);

        doingSetup = false;
    }

    void Update()
    {
        if (playerTurn || enemiesMoving || doingSetup)
            return;

        enemiesMoving = true;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            playerTurn = true;
            enemiesMoving = false;
            yield break;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playerTurn = true;
        enemiesMoving = false;
    }

    public void GameOver()
    {
        if (levelText != null)
            levelText.text = "You survived for " + level + " days.";

        if (levelImage != null)
            levelImage.SetActive(true);

        enabled = false;

        if (googlePlayAchievements != null) googlePlayAchievements.SetActive(true);
        if (googlePlayLeaderboard != null) googlePlayLeaderboard.SetActive(true);
        if (exitGame != null) exitGame.SetActive(true);
        GooglePlayGamesManager.instance.AddScoreToLeaderboard();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
