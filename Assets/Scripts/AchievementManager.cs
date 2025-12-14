using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class AchievementManager : MonoBehaviour
{
    
    public static AchievementManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CheckAchievements()
    {
        if(GameManager.instance.level >= 5)
        {
            PlayGamesPlatform.Instance.ReportProgress(Achievements.achievementLevel5, 100f, (bool success) => { });
        }
        if (GameManager.instance.level >= 10)
        {
            PlayGamesPlatform.Instance.ReportProgress(Achievements.achievementLevel10, 100f, (bool success) => { });
        }
        if (GameManager.instance.level >= 15)
        {
            PlayGamesPlatform.Instance.ReportProgress(Achievements.achievementLevel15, 100f, (bool success) => { });
        }
        if (GameManager.instance.level >= 20)
        {
            PlayGamesPlatform.Instance.ReportProgress(Achievements.achievementLevel20, 100f, (bool success) => { });
        }
        if (GameManager.instance.level >= 25)
        {
            PlayGamesPlatform.Instance.ReportProgress(Achievements.achievementLevel25, 100f, (bool success) => { });
        }
    }
}
