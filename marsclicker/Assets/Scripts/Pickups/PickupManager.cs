using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {

    public Texture2D PickupTexture;
    public List<Pickup> ActivePickups = new List<Pickup>();
    public float SpawnTimeInterval = 100.0f;

    public float Timer { get; private set; }
    public float LastSpawnTime { get; private set; }


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Timer += Time.deltaTime;
        HandlePickupSpawns();
        foreach(Pickup pickup in ActivePickups.ToArray())
        {
            pickup.UpdateOpacity();
            if (pickup.IsActive)
            {
                if (pickup.IsCollected())
                {
                    pickup.OnCollect();
                    ActivePickups.Remove(pickup);
                    Destroy(pickup.PickupObject);
                }
            }
            else
            {
                ActivePickups.Remove(pickup);
                Destroy(pickup.PickupObject);
            }

        }
	}

    public void HandlePickupSpawns()
    {
        if (Timer > LastSpawnTime + SpawnTimeInterval)
        {
            float xPos = Random.Range(0, Screen.width - PickupTexture.width);
            float yPos = Random.Range(0, Screen.height);
            LastSpawnTime = Timer;
            MoneyBonusPickup pickup = new MoneyBonusPickup(xPos, yPos, PickupTexture);
            ActivePickups.Add(pickup);

            SpawnTimeInterval = Random.Range(180.0f, 240.0f);
        }
    }
}
