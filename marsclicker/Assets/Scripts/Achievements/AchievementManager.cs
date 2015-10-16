using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class AchievementManager : MonoBehaviour
{

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

    public bool IsGameScene;

    //the popup that's displayed when an achievement is unlocked
    private static AchievementPopup achievementPopup;

    // Update is called once per frame
    void Update()
    {
        if (IsGameScene)
        {
            CheckAchievements();
            //not all triggers will be updated in the AchievementManager, money/mouse clicks etc will be done in whichever scripts handle that stuff
            UpdateTriggerCurrentValue("TOTALTIMEPLAYED", Time.deltaTime);
        }
    }

    void OnGUI()
    {
        //if there are accomplished achievements to show or we're already showing one
        if (accomplishedAchievements.Count > 0 || achievementDisplaying)
        {
            //only dequeue another achievement if the current one has finished displaying
            if (!achievementDisplaying && accomplishedAchievements.Count > 0)
            {
                Achievement ach = accomplishedAchievements.Dequeue();
                achievementPopup.content.text = String.Format("<size=20>{0}</size> \n <size=15>{1}</size> \n <size=20><b>{2} Points</b></size>", ach.AchievementName, ach.AchievementText, ach.PointReward);
                achievementDisplayTimer = Time.time;
                achievementDisplaying = true;
            }

            if (achievementDisplayTimer + AchievementDisplayDuration < Time.time)
            {
                achievementDisplaying = false;
            }

            achievementPopup.Draw();
        }
    }

    public void Initialise()
    {
        triggers = new Dictionary<string, AchievementTrigger>();
        achievements = new Dictionary<string, Achievement>();
        accomplishedAchievements = new Queue<Achievement>();
        achievementPopup = new AchievementPopup(AchievementTextFont, AchievementPopupBackground);
    }

    public void CheckAchievements()
    {
        foreach (Achievement achi in achievements.Values)
        {
            if (achi.Unlockable == true && achi.Accomplished == false)
            {
                int achievedCounter = 0;
                foreach (string trigName in achi.RequiredTriggerNames)
                {
                    if (triggers[trigName].IsTriggered())
                    {
                        achievedCounter++;
                    }
                }

                if (achievedCounter == achi.RequiredTriggerNames.Length)
                {
                    achi.Accomplish();
                    accomplishedAchievements.Enqueue(achi);
                }
            }
        }
    }

    /// <summary>
    /// Gets a list of all achievements that have either been unlocked or accomplished.
    /// For use with saving.
    /// </summary>
    /// <returns></returns>
    public List<Achievement> GetSaveAchievementList()
    {
        List<Achievement> achievementList = new List<Achievement>();
        foreach (Achievement value in achievements.Values)
        {
            if (value.Unlockable == true || value.Accomplished == true)
            {
                achievementList.Add(value);
            }
        }

        return achievementList;
    }

    /// <summary>
    /// Gets a list of all Triggers.
    /// For use with saving.
    /// </summary>
    /// <returns></returns>
    public List<AchievementTrigger> GetSaveAchievementTriggerList()
    {
        List<AchievementTrigger> achievementTriggerList = new List<AchievementTrigger>();
        foreach (AchievementTrigger value in triggers.Values)
        {
            achievementTriggerList.Add(value);
        }

        return achievementTriggerList;
    }

    public void LoadAchievementTriggersFromSave(List<AchievementTrigger> savedTriggers)
    {
        //call here as well in case new triggers have been added
        InitialiseTriggers();

        //update all the triggers that already existed to have their saved value
        foreach (AchievementTrigger trig in savedTriggers)
        {
            triggers[trig.TriggerName] = trig;
        }
    }

    public void InitialiseTriggers()
    {
        //various triggers
        CreateTrigger("MOUSECLICKS", 1);
        CreateTrigger("TOTALMONEYEARNED", 50);
        CreateTrigger("TOTALTIMEPLAYED", 10);
        CreateTrigger("PAUSEMENUOPENED", 1);
        CreateTrigger("CREDITSCREENOPENED", 1);
        CreateTrigger("UPGRADEPURCHASES", 1);

        //physical upgrade triggers
        CreateTrigger("UNLOCKDRONEBOMBERS", 1);
        CreateTrigger("UNLOCKORBITALSTATION", 1);

        //non-physical upgrade triggers
        CreateTrigger("INSPIREPURCHASES", 1);
        CreateTrigger("SOLARENERGYBOOSTPURCHASES", 1);
        CreateTrigger("BEAGREATGUYPURCHASES", 1);
        CreateTrigger("THEFIVESUNSPURCHASES", 1);
        CreateTrigger("WORMHOLETECHPURCHASES", 1);
        CreateTrigger("DOGEPURCHASES", 1);


    }

    public List<Achievement> GetUnlockedAchievements()
    {
        List<Achievement> unlockedAchievements = new List<Achievement>();
        foreach (Achievement ach in achievements.Values)
        {
            if (ach.Accomplished)
            {
                unlockedAchievements.Add(ach);
            }
        }
        return unlockedAchievements;
    }

    public List<Achievement> GetLockedAchievements()
    {
        List<Achievement> lockedAchievements = new List<Achievement>();
        foreach (Achievement ach in achievements.Values)
        {
            if (!ach.Accomplished)
            {
                lockedAchievements.Add(ach);
            }
        }
        return lockedAchievements;
    }

    public List<Achievement> GetAllAchievements()
    {
        List<Achievement> achievementList = new List<Achievement>();
        foreach (Achievement ach in achievements.Values)
        {
            achievementList.Add(ach);
        }
        return achievementList;
    }

    public void LoadAchievementsFromSave(List<Achievement> savedAchievements)
    {
        //create all the triggers just in case new triggers have been added that aren't in the save file
        InitialiseAchievements();

        //update all the triggers that we have saved
        foreach (Achievement ach in savedAchievements)
        {
            achievements[ach.AchievementName] = ach;
        }

        InitialiseAchievementOnAccomplish();
    }

    public void InitialiseAchievements()
    {
        //create general achievements
        CreateAchievement("Learn the entire game", "Click on the planet", new string[] { "MOUSECLICKS" }, true, 10)
            .SetNextAchievement(CreateAchievement("Now you've got it!", "Reach 10 clicks", new string[] { "MOUSECLICKS" }, false, 20))
                .SetNextAchievement(CreateAchievement("You really like this, don't you?", "Reach 100 clicks", new string[] { "MOUSECLICKS" }, false, 50))
                    .SetNextAchievement(CreateAchievement("Look those clicking!", "Reach 1000 clicks", new string[] { "MOUSECLICKS" }, false, 500))
                        .SetNextAchievement(CreateAchievement("You should really get out more", "Reach 100000 clicks", new string[] { "MOUSECLICKS" }, false, 5000))
                            .SetNextAchievement(CreateAchievement("Godlike!", "Reach 1000000000 clicks", new string[] { "MOUSECLICKS" }, false, 1));


        CreateAchievement("Time dilation", "Open the Pause Menu", new string[] { "PAUSEMENUOPENED" }, true, 10);
        CreateAchievement("<3", "Watch the Credits", new string[] { "CREDITSCREENOPENED" }, true, 10);

        CreateAchievement("Let's get clicking", "Buy an upgrade", new string[] { "UPGRADEPURCHASES" }, true, 30);

        //create physical upgrade achievements
        CreateAchievement("Not a good idea", "Unlock Automated Drone Bombers", new string[] { "UNLOCKDRONEBOMBERS" }, true, 5);
        CreateAchievement("Even your fun is now automated", "Unlock Orbital Station", new string[] { "UNLOCKORBITALSTATION" }, true, 20);

        //create non-physical upgrade achievements
        CreateAchievement("Win the hearts of man", "Purchase Inspire", new string[] { "INSPIREPURCHASES" }, true, 10);
        CreateAchievement("The future is bright", "Purchase Solar Energy Boost", new string[] { "SOLARENERGYBOOSTPURCHASES" }, true, 20);
        CreateAchievement("We ran out of names for upgrades", "Purchase Be a Great Guy", new string[] { "BEAGREATGUYPURCHASES" }, true, 30);
        CreateAchievement("Do you get it? No?", "Purchase THE FIVE SUNS", new string[] { "THEFIVESUNSPURCHASES" }, true, 50);
        CreateAchievement("The event horizon", "Purchase Worm Hole Tech", new string[] { "WORMHOLETECHPURCHASES" }, true, 60);
        CreateAchievement("wow much many wow", "Purchase Secret (doge)", new string[] { "DOGEPURCHASES" }, true, 1000);

        CreateAchievement("Money, Money, Money", "Earn $50", new string[] { "TOTALMONEYEARNED" }, true, 100);
        CreateAchievement("Time Achievement", "Play for 10 whole seconds", new string[] { "TOTALTIMEPLAYED" }, true, 100);
    }

    /// <summary>
    /// Method for initialising the OnAccomplish callback for achievements.
    /// Has to be done seperately otherwise the saving serialization won't work
    /// </summary>
    public void InitialiseAchievementOnAccomplish()
    {
        UpdateAchievementOnAccomplish("Learn the entire game", new Action(() => UpdateTriggerActivationValue("MOUSECLICKS", 10)));
        UpdateAchievementOnAccomplish("Now you've got it!", new Action(() => UpdateTriggerActivationValue("MOUSECLICKS", 100)));
        UpdateAchievementOnAccomplish("You really like this, don't you?", new Action(() => UpdateTriggerActivationValue("MOUSECLICKS", 1000)));
        UpdateAchievementOnAccomplish("Look those clicking!", new Action(() => UpdateTriggerActivationValue("MOUSECLICKS", 100000)));
        UpdateAchievementOnAccomplish("Godlike!", new Action(() => UpdateTriggerActivationValue("MOUSECLICKS", 1000000000)));
    }


    /// <summary>
    /// Creates an achievement and adds it to a dictionary containing all achievements
    /// </summary>
    /// <param name="achievementName">Name to be used for accessing the achievement. This name will also be displayed at the top of the Achievement Popup</param>
    /// <param name="bodyText">Text that will be displayed on the Achievement Popup directly beneath the Achievement Name</param>
    /// <param name="triggerNamesArray">An array of the names of the Triggers which must be active for this achievement to be accomplished</param>
    /// <param name="initiallyUnlockable">Whether or not the achievement can be unlocked straight away. For a series of achievements, this should probably be set to false</param>
    /// <param name="points">The amount of points awarded for completing the achievement</param>
    /// <returns>Returns the achievement that has been created.</returns>
    public Achievement CreateAchievement(string achievementName, string bodyText, string[] triggerNamesArray, bool initiallyUnlockable, int points)
    {
        if (String.IsNullOrEmpty(achievementName))
        {
            throw new ArgumentNullException("achievementName", "Argument must be a valid string corresponding the name of an achievement");
        }
        if (triggerNamesArray == null || triggerNamesArray.Length == 0)
        {
            throw new ArgumentNullException("triggerArray", "Array must be initialised and contain at least one element");
        }

        Achievement achievement = new Achievement(achievementName, bodyText, triggerNamesArray, initiallyUnlockable, points);
        achievements.Add(achievement.AchievementName, achievement);

        return achievement;
    }

    /// <summary>
    /// Updates the Action that is called when the specified achievement is accomplished
    /// </summary>
    /// <param name="achievementName"></param>
    /// <param name="onAccomplish">Callback to be called on an achievement is accomplished</param>
    public void UpdateAchievementOnAccomplish(string achievementName, Action onAccomplish)
    {
        if (String.IsNullOrEmpty(achievementName))
        {
            throw new ArgumentNullException("achievementName", "Argument must be a valid string corresponding the name of an achievement");
        }

        achievements[achievementName].OnAchievementAccomplished = onAccomplish;
    }

    /// <summary>
    /// Creates a Trigger and adds it to a dictionary containing all triggers
    /// </summary>
    /// <param name="triggerName"></param>
    /// <param name="activationValue"></param>
    public void CreateTrigger(string triggerName, float activationValue)
    {
        if (String.IsNullOrEmpty(triggerName))
        {
            throw new ArgumentNullException("triggerName", "Argument must be a valid string corresponding the name of a trigger");
        }

        AchievementTrigger trigger = new AchievementTrigger(triggerName, activationValue);
        triggers.Add(trigger.TriggerName, trigger);
    }

    /// <summary>
    /// Increments the current value of the Trigger (when current value > activation value, the Trigger becomes active)
    /// </summary>
    /// <param name="triggerName"></param>
    /// <param name="incrementValue"></param>
    public void UpdateTriggerCurrentValue(string triggerName, float incrementValue)
    {
        if (String.IsNullOrEmpty(triggerName))
        {
            throw new ArgumentNullException("triggerName", "Argument must be a valid string corresponding the name of a trigger");
        }

        triggers[triggerName].CurrentValue += incrementValue;
    }

    /// <summary>
    /// Updates the value at which a Trigger becomes active. Handy for when you have a series of achievements all relying on the same trigger.
    /// </summary>
    /// <param name="triggerName"></param>
    /// <param name="newActivationValue"></param>
    public void UpdateTriggerActivationValue(string triggerName, int newActivationValue)
    {
        if (String.IsNullOrEmpty(triggerName))
        {
            throw new ArgumentNullException("triggerName", "Argument must be a valid string corresponding the name of a trigger");
        }

        triggers[triggerName].TriggerActivationValue = newActivationValue;
    }
}