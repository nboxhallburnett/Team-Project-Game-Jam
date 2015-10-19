using UnityEngine;
using System.Collections;
using System;

public class MultiplierPickup : Pickup
{
    public float x2Probability = 100f;
    public float x4Probability = 25f;
    public float x8Probability = 15f;
    public float x16Probability = 6f;
    public float x32Probability = 3.85f;
    public float x100000Probability = 0.15f;

    public MultiplierPickup(float spawnX, float spawnY, Texture2D texture)
        :base(spawnX, spawnY, texture)
    {

    }

    public override void OnCollect()
    {
        float randomFloat = UnityEngine.Random.Range(0f, 100f);
        float multiplier = 0;
        if(randomFloat <= x100000Probability)
        {
            multiplier = 100000f;
        }
        else if (randomFloat <= x32Probability)
        {
            multiplier = 32f;
        }
        else if (randomFloat <= x16Probability)
        {
            multiplier = 16f;
        }
        else if (randomFloat <= x8Probability)
        {
            multiplier = 8f;
        }
        else if (randomFloat <= x4Probability)
        {
            multiplier = 4f;
        }
        else if (randomFloat <= x2Probability)
        {
            multiplier = 2f;
        }

        clickme.addMultiplier(multiplier);
    }
}
