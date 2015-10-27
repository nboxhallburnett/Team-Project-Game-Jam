using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MoneyBonusPickup : Pickup {

    public Text BonusTextField { get; set; }

    public MoneyBonusPickup(float spawnX, float spawnY, Texture2D texture, AudioSource sound, Text bonusText)
        :base(spawnX, spawnY, texture, sound)
    {
        BonusTextField = bonusText;
    }

    /// <summary>
    /// Provides a 10% bonus to the player's cash total
    /// </summary>
    public override void OnCollect()
    {
        float cashBonus = GameControl.data.cash * 0.1f;
        GameControl.data.cash += cashBonus;
        if(BonusTextField != null)
        {
            if(cashBonus > 0)
            {
                BonusTextField.text = "+$" + cashBonus.ToString("0.00");
            }            
        }
        if (PickupSound != null)
        {
            PickupSound.Play();
        }        
    }
}
