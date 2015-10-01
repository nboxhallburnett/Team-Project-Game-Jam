using UnityEngine;
using System;
using System.Collections;

public class Achievement {

    public string AchievementName { get; private set; }
    public string AchievementText { get; private set; }
    //the name of the triggers that been to be active in order to accomplish this achievement
    public string[] RequiredTriggerNames { get; set; }
    //determines whether or not the achievement can be accomplished yet
    public bool Unlockable { get; set; }
    //whether or not the achievement has already been accomplished
    public bool Accomplished { get; set; }
    //for linking a sequence of related achievements - the next achievement in the sequence
    public Achievement NextAchievement { get; set; }

    public Achievement(string name, string bodyText, string[] triggerNames)
    {
        AchievementName = name;
        AchievementText = bodyText;
        RequiredTriggerNames = triggerNames;
        Accomplished = false;
        Unlockable = true;
    }

    /// <summary>
    /// Method to be called once all triggers are active. This is handled by the AchievementManager.
    /// </summary>
    public void Accomplish()
    {
        Accomplished = true;

        //allow the next achievement in the chain to be unlockable
        if(NextAchievement != null)
        {
            NextAchievement.Unlockable = true;
        }
    }
}
