using UnityEngine;
using System.Collections;
using System;

public class MoneyBonusPickup : Pickup {

    public MoneyBonusPickup(float spawnX, float spawnY, Texture2D texture)
        :base(spawnX, spawnY, texture)
    {
        
    }

    /// <summary>
    /// Provides a 10% bonus to the player's cash total
    /// </summary>
    public override void OnCollect()
    {
        GameControl.data.cash += GameControl.data.cash * 0.1f;
    }
}
