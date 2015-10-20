using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AchievementComparer : IComparer<Achievement>
{
    /// <summary>
    /// Orders based on whether or not the achievement has been unlocked and then based on the points reward.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int Compare(Achievement x, Achievement y)
    {
        //always put the Godlike! achievement to the bottom
        if(x.AchievementName == "Godlike!")
        {
            return -1;
        }
        else if(y.AchievementName == "Godlike!")
        {
            return 1;
        }

        if(x.Accomplished && !y.Accomplished)
        {
            return 1;
        }
        else if(!x.Accomplished && y.Accomplished)
        {
            return -1;
        }
        else
        {
            if(x.PointReward > y.PointReward)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
