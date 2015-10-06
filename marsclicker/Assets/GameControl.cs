using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameControl : MonoBehaviour {

    // Used to reference the data and functions stored in the class anywhere in the game
    public static GameControl data;

    // variables to be used to store
    public float cash;
    public float score;

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
        // Load game data from file when starting
        Load();
    }

    // Saves data to file
    public void Save () {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");
        GameData saveData = new GameData();

        saveData.cash = cash;
        saveData.score = score;

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

            cash = data.cash;
            score = data.score;
        } else { 
            // Otherwise use the default data
            cash = 0;
            score = 0;
        }
    }

}

// This serializable class is used to create an object which can be saved to and loaded from the disk
[Serializable]
class GameData {
    public float cash { get; set; }
    public float score { get; set; }
}
