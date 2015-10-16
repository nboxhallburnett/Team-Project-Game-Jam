using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PickupManager : MonoBehaviour {

    public Texture2D PickupTexture;
    public List<Pickup> ActivePickups = new List<Pickup>();
    public bool IsGameScene;
    public float SpawnTimeInterval = 5.0f;

    public float Timer { get; private set; }
    public float LastSpawnTime { get; private set; }    

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (IsGameScene)
        {
            Timer += Time.deltaTime;
            HandlePickupSpawns();
            foreach (Pickup pickup in ActivePickups.ToArray())
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
	}

    public void HandlePickupSpawns()
    {
        if (Timer > LastSpawnTime + SpawnTimeInterval)
        {
            float xPos = Random.Range(0, Screen.width - PickupTexture.width);
            float yPos = Random.Range(0, Screen.height);
            LastSpawnTime = Timer;

            float pickupNumber = Random.Range(0f, 2f);
            if(pickupNumber <= 1f)
            {
                MoneyBonusPickup pickup = new MoneyBonusPickup(xPos, yPos, PickupTexture);
                ActivePickups.Add(pickup);
            }
            else if(pickupNumber <= 2f)
            {
                MultiplierPickup pickup = new MultiplierPickup(xPos, yPos, PickupTexture);
                ActivePickups.Add(pickup);
            }

            SpawnTimeInterval = Random.Range(180.0f, 240.0f);
        }
    }
}
