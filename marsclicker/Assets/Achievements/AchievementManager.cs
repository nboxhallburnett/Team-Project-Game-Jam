using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AchievementManager : MonoBehaviour {

    private static Dictionary<string, AchievementTrigger> triggers;
    private static Dictionary<string, Achievement> achievements;

    private static Queue<Achievement> accomplishedAchievements;

    private static bool achievementTextTriggered;

    // Use this for initialization
    void Start() {
        triggers = new Dictionary<string, AchievementTrigger>();
        CreateTrigger("MOUSECLICKS", 25);
        CreateTrigger("TOTALMONEYEARNT", 50);

        achievements = new Dictionary<string, Achievement>();
        CreateAchievement("It's a start!", "Some random body text", new string[] { "MOUSECLICKS" });

        accomplishedAchievements = new Queue<Achievement>();
    }
	
	// Update is called once per frame
	void Update () {
        CheckAchievements();
	}

    void OnGUI()
    {
        if(accomplishedAchievements.Count > 0)
        {
            GUI.Label(new Rect(100, 100, 200, 150), "test");
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

    public static void CreateAchievement(string achievementName, string bodyText, string[] triggerNamesArray)
    {
        if (String.IsNullOrEmpty(achievementName))
        {
            throw new ArgumentNullException("achievementName", "Argument must be a valid string corresponding the name of a trigger");
        }
        if(triggerNamesArray == null || triggerNamesArray.Length == 0)
        {
            throw new ArgumentNullException("triggerArray", "Array must be initialised and contain at least one element");
        }

        Achievement achievement = new Achievement(achievementName, bodyText, triggerNamesArray);
        achievements.Add(achievement.AchievementName, achievement);
    }

    public static void CreateTrigger(string triggerName, int activationValue)
    {
        if (String.IsNullOrEmpty(triggerName))
        {
            throw new ArgumentNullException("triggerName", "Argument must be a valid string corresponding the name of a trigger");
        }

        AchievementTrigger trigger = new AchievementTrigger(triggerName, activationValue);
        triggers.Add(trigger.TriggerName, trigger);
    }

    public static void UpdateTriggerCurrentValue(string triggerName, int incrementValue)
    {
        if(String.IsNullOrEmpty(triggerName))
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

        triggers[triggerName].CurrentValue = newActivationValue;
    }
}
