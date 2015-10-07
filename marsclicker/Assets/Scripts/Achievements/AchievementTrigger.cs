using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AchievementTrigger {

    public string TriggerName { get; private set; }
    public float TriggerActivationValue { get; set; }
    public float CurrentValue { get; set; }

    public AchievementTrigger(string name, float activationValue)
    {
        TriggerName = name;
        TriggerActivationValue = activationValue;
    }

    public bool IsTriggered()
    {
        return CurrentValue >= TriggerActivationValue ? true : false;
    }

}
