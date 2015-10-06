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
    public Achievement NextAchievement { get; private set; }

    //Code to be called once this achievement has been unlocked/accomplished
    private readonly Action OnAchievementAccomplished;

    /// <summary>
    /// Create a new Achievement
    /// </summary>
    /// <param name="name">THe name of the achievement. Will be displayed on the screen and used a dicitonary key for accessing achievements</param>
    /// <param name="bodyText">The text to display on the screen beneath the achievement name once this achievement is unlocked</param>
    /// <param name="triggerNames">The names of the triggers that must be active in order to unlock this achievement</param>
    /// <param name="initiallyUnlockable">Whether or not the achievement is unlockable straight away. If this is not the first achievement in a sequence then this should probably be false.</param>
    /// <param name="onAccomplish">Optional method which is called when the achievement in unlocked/accomplished. Could contain code for a reward or code to change a trigger activation value</param>
    public Achievement(string name, string bodyText, string[] triggerNames, bool initiallyUnlockable, Action onAccomplish = null)
    {
        AchievementName = name;
        AchievementText = bodyText;
        RequiredTriggerNames = triggerNames;
        Unlockable = initiallyUnlockable;
        Accomplished = false;
        OnAchievementAccomplished = onAccomplish;
    }

    /// <summary>
    /// Method to be called once all triggers are active. This is handled by the AchievementManager.
    /// </summary>
    public void Accomplish()
    {
        Accomplished = true;

        if (OnAchievementAccomplished != null)
        {
            OnAchievementAccomplished.Invoke();
        }

        //allow the next achievement in the chain to be unlockable
        if (NextAchievement != null)
        {
            NextAchievement.Unlockable = true;
        }
    }

    /// <summary>
    /// Method for setting the next achievement in a sequence of achievements. Once this achievement is unlocked, the next achievement wil become unlockable.
    /// </summary>
    /// <param name="next">The next achievement in the sequence</param>
    /// <returns>Returns the achievement that was passed in for the purpose of chaining</returns>
    public Achievement SetNextAchievement(Achievement next)
    {
        NextAchievement = next;

        return NextAchievement;
    }
}
