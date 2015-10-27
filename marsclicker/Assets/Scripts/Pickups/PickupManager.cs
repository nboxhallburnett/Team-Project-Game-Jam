using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PickupManager : MonoBehaviour
{

    public Texture2D PickupTexture;
    public List<Pickup> ActivePickups = new List<Pickup>();
    public bool IsGameScene;
    public float SpawnTimeInterval = 5.0f;
    public GameObject landscapeGamePanel;
    public GameObject portraitGamePanel;
    public AudioSource PickupSound;
    public ParticleSystem CashParticles;
    public Text LandscapeMoneyBonusText;
    public Text PortraitMoneyBonusText;
    private Text CurrentMoneyBonusText;

    public float Timer { get; private set; }
    public float LastSpawnTime { get; private set; }
    private float MoneyBonusTextTimer;
    private bool TextTimerStarted;

    private ParticleSystem cashEmitter;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameScene)
        {
            if(LandscapeMoneyBonusText == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag("landscapeBonusText");
                LandscapeMoneyBonusText = obj.GetComponent<Text>();
                obj = GameObject.FindGameObjectWithTag("portraitBonusText");
                PortraitMoneyBonusText = obj.GetComponent<Text>();
            }
            if (Screen.width > Screen.height)
            {
                CurrentMoneyBonusText = LandscapeMoneyBonusText;
            }
            else
            {
                CurrentMoneyBonusText = PortraitMoneyBonusText;
            }
            Timer += Time.deltaTime;
            HandlePickupSpawns();
            foreach (Pickup pickup in ActivePickups.ToArray())
            {
                pickup.UpdateOpacity();
                if (pickup.IsActive)
                {
                    if (pickup.IsCollected())
                    {
                        if (pickup.GetType() == typeof(MoneyBonusPickup))
                        {
                            if (CashParticles != null)
                            {
                                if (cashEmitter == null)
                                {
                                    cashEmitter = Instantiate(CashParticles);
                                }
                                if (cashEmitter != null)
                                {
                                    cashEmitter.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                    cashEmitter.Emit(cashEmitter.maxParticles);
                                }
                            }
                        }
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
            if(CurrentMoneyBonusText != null && CurrentMoneyBonusText.text != "")
            {
                if (!TextTimerStarted)
                {
                    MoneyBonusTextTimer = Timer;
                    TextTimerStarted = true;
                }
                else if (Timer > MoneyBonusTextTimer + 4.0f)
                {
                    //reset them both in case the screen orientation has changed
                    LandscapeMoneyBonusText.text = "";
                    PortraitMoneyBonusText.text = "";
                    TextTimerStarted = false;
                }
                
            }

        }
    }

    public void HandlePickupSpawns()
    {
        if (landscapeGamePanel == null)
        {
            landscapeGamePanel = GameObject.FindGameObjectWithTag("landscapeGamePanel");
        }
        if (portraitGamePanel == null)
        {
            portraitGamePanel = GameObject.FindGameObjectWithTag("portraitGamePanel");
        }

        if (Timer > LastSpawnTime + SpawnTimeInterval)
        {
            float xPos = Random.Range(0, Screen.height / 2);
            float yPos = Random.Range(0, Screen.width / 2);
            LastSpawnTime = Timer;

            float pickupNumber = Random.Range(0f, 2f);
            if (pickupNumber <= 1f)
            {
                MoneyBonusPickup pickup = new MoneyBonusPickup(xPos, yPos, PickupTexture, PickupSound, CurrentMoneyBonusText);
                if (Screen.width > Screen.height)
                {
                    pickup.PickupObject.transform.parent = landscapeGamePanel.transform;
                }
                else
                {
                    pickup.PickupObject.transform.parent = portraitGamePanel.transform;
                }

                ActivePickups.Add(pickup);
            }
            else if (pickupNumber <= 2f)
            {
                MultiplierPickup pickup = new MultiplierPickup(xPos, yPos, PickupTexture, PickupSound);
                ActivePickups.Add(pickup);

                if (Screen.width > Screen.height)
                {
                    pickup.PickupObject.transform.parent = landscapeGamePanel.transform;
                }
                else
                {
                    pickup.PickupObject.transform.parent = portraitGamePanel.transform;
                }
            }

            SpawnTimeInterval = Random.Range(45.0f, 60.0f);
        }
    }
}
