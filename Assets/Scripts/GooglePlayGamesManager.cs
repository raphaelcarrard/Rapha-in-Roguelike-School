using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

public class GooglePlayGamesManager : MonoBehaviour
{

    public static GooglePlayGamesManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
    }

    void Start()
    {
        PlayGamesPlatform.Activate();
        Login();
    }

    public void Login()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Success to login Google Play Games!");
        }
        else
        {
            Debug.LogError("Fail to login Google Play Games!");
        }
    }

    public void AddScoreToLeaderboard()
    {
        PlayGamesPlatform.Instance.ReportScore(GameManager.instance.level, Leaderboard.leaderboardBestPlayer, (bool success) => { });
    }

    public void ShowLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(Leaderboard.leaderboardBestPlayer);
    }

    public void ShowAchievements()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }
}
