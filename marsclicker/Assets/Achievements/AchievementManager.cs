using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AchievementManager : MonoBehaviour {

    private static Dictionary<string, AchievementTrigger> triggers;
    private static Dictionary<string, Achievement> achievements;
    private static Queue<Achievement> accomplishedAchievements;

    //timer for keeping track of how much longer the achievement popup should be on screen for
    private static float achievementDisplayTimer;

    //whether or not the achievement popup is on screen
    private static bool achievementDisplaying;

    //how long the achievement popup is displayed for
    public float AchievementDisplayDuration = 7.5f;

    public Texture2D AchievementPopupBackground;

    public Font AchievementTextFont;

    //the popup that's displayed when an achievement is unlocked
    private static AchievementPopup achievementPopup;

    // Use this for initialization
    void Start() {
        triggers = new Dictionary<string, AchievementTrigger>();
        CreateTrigger("MOUSECLICKS", 25);
        CreateTrigger("TOTALMONEYEARNED", 50);
        CreateTrigger("TOTALTIMEPLAYED", 10);

        //some example achievements, can chain creation of them together to create a sequence of related achievements
        achievements = new Dictionary<string, Achievement>();
        CreateAchievement("Witty Achievement Title 1", "Click the mouse 25 times", new string[] { "MOUSECLICKS" }, true, new Action(() => UpdateTriggerActivationValue("MOUSECLICKS", 100)))
            .SetNextAchievement(CreateAchievement("Witty Achievement Title 2", "Click the mouse 100 times", new string[] { "MOUSECLICKS" }, false, new Action(() => UpdateTriggerActivationValue("MOUSECLICKS", 150))))
            .SetNextAchievement(CreateAchievement("Witty Achievement Title 3", "Click the mouse 150 times", new string[] { "MOUSECLICKS" }, false));
        
        CreateAchievement("Money, Money, Money", "Earn $50", new string[] { "TOTALMONEYEARNED" }, true);

        CreateAchievement("Time Achievement", "Play for 10 whole seconds", new string[] { "TOTALTIMEPLAYED" }, true);

        accomplishedAchievements = new Queue<Achievement>();

        achievementPopup = new AchievementPopup(AchievementTextFont, AchievementPopupBackground);
    }
	
	// Update is called once per frame
	void Update () {
        CheckAchievements();
        //not all triggers will be updated in the AchievementManager, money/mouse clicks etc will be done in whichever scripts handle that stuff
        UpdateTriggerCurrentValue("TOTALTIMEPLAYED", Time.deltaTime);
	}

    void OnGUI()
    {
        //if there are accomplished achievements to show or we're already showing one
        if(accomplishedAchievements.Count > 0 || achievementDisplaying)
        {
            //only dequeue another achievement if the current one has finished displaying
            if(!achievementDisplaying && accomplishedAchievements.Count > 0)
            {
                Achievement ach = accomplishedAchievements.Dequeue();
                achievementPopup.content.text = String.Format("<size=30>Achievement Unlocked!</size> \n <size=20>{0}</size> \n {1}", ach.AchievementName, ach.AchievementText);             
                achievementDisplayTimer = Time.time;
                achievementDisplaying = true;
            }            

            if(achievementDisplayTimer + AchievementDisplayDuration < Time.time)
            {
                achievementDisplaying = false;
            }

            achievementPopup.Draw();
        }    
    }

    public void CheckAchievements()
    {
        foreach(Achievement achi in achievements.Values)
        {
            if(achi.Unlockable == true && achi.Accomplished == false)
            {
                int achievedCounter = 0;
                foreach(string trigName in achi.RequiredTriggerNames)
                {
                    if (triggers[trigName].IsTriggered())
                    {
                        achievedCounter++;
                    }
                }

                if(achievedCounter == achi.RequiredTriggerNames.Length)
                {
                    achi.Accomplish();
                    accomplishedAchievements.Enqueue(achi);
                }
            }
        }
    }

    public static Achievement CreateAchievement(string achievementName, string bodyText, string[] triggerNamesArray, bool initiallyUnlockable, Action onAccomplish = null)
    {
        if (String.IsNullOrEmpty(achievementName))
        {
            throw new ArgumentNullException("achievementName", "Argument must be a valid string corresponding the name of a trigger");
        }
        if(triggerNamesArray == null || triggerNamesArray.Length == 0)
        {
            throw new ArgumentNullException("triggerArray", "Array must be initialised and contain at least one element");
        }

        Achievement achievement = new Achievement(achievementName, bodyText, triggerNamesArray, initiallyUnlockable, onAccomplish);
        achievements.Add(achievement.AchievementName, achievement);

        return achievement;
    }

    public static void CreateTrigger(string triggerName, float activationValue)
    {
        if (String.IsNullOrEmpty(triggerName))
        {
            throw new ArgumentNullException("triggerName", "Argument must be a valid string corresponding the name of a trigger");
        }

        AchievementTrigger trigger = new AchievementTrigger(triggerName, activationValue);
        triggers.Add(trigger.TriggerName, trigger);
    }

    public static void UpdateTriggerCurrentValue(string triggerName, float incrementValue)
    {
        if (String.IsNullOrEmpty(triggerName))
        {
            throw new ArgumentNullException("triggerName", "Argument must be a valid string corresponding the name of a trigger");
        }

        triggers[triggerName].CurrentValue += incrementValue;
    }

    public static void UpdateTriggerActivationValue(string triggerName, int newActivationValue)
    {
        if (String.IsNullOrEmpty(triggerName))
        {
            throw new ArgumentNullException("triggerName", "Argument must be a valid string corresponding the name of a trigger");
        }

        triggers[triggerName].TriggerActivationValue = newActivationValue;
    }
}