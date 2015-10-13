using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class GameControl : MonoBehaviour {

    // Used to reference the data and functions stored in the class anywhere in the game
    public static GameControl data;
    public static AchievementManager achievementManager;
	public static WeaponManager weaponManager;

    // variables to be used to store stuff
    public float cash { get; set; }
	public float score { get; set; }
	public float multiplier { get; set; }
	public float multiplierTimer { get; set; }

	int[] weaponCount;

    // Runs before Start
    void Awake () {
        // Makes sure that there is only ever one of this script in the game
        if (data == null) {
            DontDestroyOnLoad(gameObject);
            data = this;
        } else if (data != this) {
            Destroy(gameObject);
        }
    }

    // Initialise data for the game
	void Start () {
		// Initialise achievement data
        achievementManager = GetComponent<AchievementManager>();
        achievementManager.Initialise();
		
		// Load game data from file when starting
		Load();

		// Initialise weapon data
		weaponManager = GetComponent<WeaponManager>();
		weaponManager.Initialise(weaponCount);
    }

	void OnDestroy () {
		Save();
	}

    // Saves data to file
    public void Save () {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");
        GameData saveData = new GameData();

        saveData.cash = cash;
        saveData.score = score;
		saveData.multiplier = multiplier;
		saveData.multiplierTimer = multiplierTimer;
		saveData.weaponCountArray = weaponManager.GetWeaponCounts();
        saveData.achievementTriggers = achievementManager.GetSaveAchievementTriggerList();
        saveData.achievements = achievementManager.GetSaveAchievementList();

        bf.Serialize(file, saveData);
        file.Close();
    }

    // Loads data from file
    public void Load () {
        // Only load the data if the file exists
        if (File.Exists(Application.persistentDataPath + "/gameData.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
            GameData savedData = (GameData)bf.Deserialize(file);

            file.Close();

            cash = savedData.cash;
            score = savedData.score;
			multiplier = savedData.multiplier;
			multiplierTimer = savedData.multiplierTimer;

			weaponCount = savedData.weaponCountArray;

            achievementManager.LoadAchievementTriggersFromSave(savedData.achievementTriggers);
            achievementManager.LoadAchievementsFromSave(savedData.achievements);

        } else { 
            // Otherwise use the default data
            cash = 0.0f;
            score = 0.0f;
			multiplier = 1.0f;
			multiplierTimer = 0.0f;

			weaponCount = new int[30];

            achievementManager.InitialiseTriggers();
            achievementManager.InitialiseAchievements();
            achievementManager.InitialiseAchievementOnAccomplish();
        }
    }

}

// This serializable class is used to create an object which can be saved to and loaded from the disk
[Serializable]
class GameData {
    public float cash { get; set; }
    public float score { get; set; }
    public float multiplier { get; set; }
	public float multiplierTimer { get; set; }
	public int[] weaponCountArray { get; set; }
    public List<Achievement> achievements { get; set; }
    public List<AchievementTrigger> achievementTriggers { get; set; }
}
