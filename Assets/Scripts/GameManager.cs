using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;
    private BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playerTurn = true;

    private Text levelText;
    public GameObject levelImage;
    public int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    public GameObject TutorialInfo;
    public bool medal1, medal2, medal3, medal4;
    public bool medal5, medal6, medal7, medal8;
    public bool medal9, medal10, medal11, medal12;
    public bool medal13, medal14, medal15, medal16;
    public bool medal17, medal18, medal19, medal20;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
        TutorialInfo = GameObject.Find("Background");
    }

    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;  
        levelImage.SetActive(true);
        Invoke("HideLevelImage",levelStartDelay);
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    public void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
    
    public void GameOver()
    {
        levelText.text = "You survived for " + level + " days.";
        levelImage.SetActive(true);
        enabled = false;
        NGHelper.instance.NGSubmitScore(12384, level);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
        if(level >= 1 && medal1 == false){
            NGHelper.instance.unlockMedal(77941);
            medal1 = true;
        }
        if(level >= 2 && medal2 == false){
            NGHelper.instance.unlockMedal(77942);
            medal2 = true;
        }
        if(level >= 3 && medal3 == false){
            NGHelper.instance.unlockMedal(77943);
            medal3 = true;
        }
        if(level >= 4 && medal4 == false){
            NGHelper.instance.unlockMedal(77944);
            medal4 = true;
        }
        if(level >= 5 && medal5 == false){
            NGHelper.instance.unlockMedal(71993);
            medal5 = true;
        }
        if(level >= 6 && medal6 == false){
            NGHelper.instance.unlockMedal(77945);
            medal6 = true;
        }
        if(level >= 7 && medal7 == false){
            NGHelper.instance.unlockMedal(77946);
            medal7 = true;
        }
        if(level >= 8 && medal8 == false){
            NGHelper.instance.unlockMedal(77947);
            medal8 = true;
        }
        if(level >= 9 && medal9 == false){
            NGHelper.instance.unlockMedal(77948);
            medal9 = true;
        }
        if(level >= 10 && medal10 == false){
            NGHelper.instance.unlockMedal(71994);
            medal10 = true;
        }
        if(level >= 11 && medal11 == false){
            NGHelper.instance.unlockMedal(77949);
            medal11 = true;
        }
        if(level >= 12 && medal12 == false){
            NGHelper.instance.unlockMedal(77950);
            medal12 = true;
        }
        if(level >= 13 && medal13 == false){
            NGHelper.instance.unlockMedal(77951);
            medal13 = true;
        }
        if(level >= 14 && medal14 == false){
            NGHelper.instance.unlockMedal(77952);
            medal14 = true;
        }
        if(level >= 15 && medal15 == false){
            NGHelper.instance.unlockMedal(71995);
            medal15 = true;
        }
        if(level >= 16 && medal16 == false){
            NGHelper.instance.unlockMedal(77953);
            medal16 = true;
        }
        if(level >= 17 && medal17 == false){
            NGHelper.instance.unlockMedal(71996);
            medal17 = true;
        }
        if(level >= 18 && medal18 == false){
            NGHelper.instance.unlockMedal(77954);
            medal18 = true;
        }
        if(level >= 19 && medal19 == false){
            NGHelper.instance.unlockMedal(77955);
            medal19 = true;
        }
        if(level >= 20 && medal20 == false){
            NGHelper.instance.unlockMedal(77956);
            medal20 = true;
        }
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
    
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playerTurn = true;
        enemiesMoving = false;
    }
}