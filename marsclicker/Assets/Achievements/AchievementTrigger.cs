using UnityEngine;
using System.Collections;

public class AchievementTrigger {

    public string TriggerName { get; private set; }
    public int TriggerActivationValue { get; set; }
    public int CurrentValue { get; set; }


    public AchievementTrigger(string name, int activationValue)
    {
        TriggerName = name;
        TriggerActivationValue = activationValue;
    }

    public bool IsTriggered()
    {
        return CurrentValue >= TriggerActivationValue ? true : false;
    }

}
