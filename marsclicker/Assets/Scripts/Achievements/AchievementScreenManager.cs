using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AchievementScreenManager : MonoBehaviour {

	public GameObject UnlockedAchievementPrefab;
	public GameObject LockedAchievementPrefab;

	// Use this for initialization
	void Start () {
		float rightX = -32.5f;
		float rightY = -75f;
		float leftX = 32.5f;
		float leftY = -75;
		float achievementPrefabHeight = 90;
		float achievementPadding = 45;

        int achievementCounter = 0;

        //get all the achievements in the game
		List<Achievement> achievements = GameControl.achievementManager.GetAllAchievements();

        //order those achievements so that the unlocked ones appear at the top
        achievements = achievements.OrderByDescending(ach => ach, new AchievementComparer()).ToList();

		RectTransform contentTransform = this.GetComponent<RectTransform> ();
		contentTransform.sizeDelta = new Vector2 (contentTransform.sizeDelta.x, achievements.Count * (achievementPrefabHeight + achievementPadding));
		foreach (Achievement ach in achievements) 
		{
            GameObject obj;
            if (ach.Accomplished)
            {
                 obj = Instantiate(UnlockedAchievementPrefab);
            }
            else
            {
                obj = Instantiate(LockedAchievementPrefab);
            }

            List<Text> textComponents = new List<Text>();
            obj.GetComponentsInChildren<Text>(textComponents);
            
            foreach(Text text in textComponents)
            {
                switch(text.name)
                {
                    case "Heading Text":
                        text.text = ach.AchievementName;
                        break;
                    case "Body Text":
                        text.text = ach.AchievementText;
                        break;
                    case "Points Text":
                        text.text = string.Format("{0} Points", ach.AchievementName != "Godlike!" ? ach.PointReward.ToString() : "???");
                        break;
                }
            }
			//put the object inside the Content area
			obj.transform.SetParent (this.transform);
			RectTransform trans = obj.GetComponent<RectTransform> ();	
			//set the right/left values
			trans.offsetMax = new Vector2(rightX, rightY + (-(achievementPrefabHeight + achievementPadding) * achievementCounter));
			trans.offsetMin = new Vector2(leftX, leftY + (-(achievementPrefabHeight + achievementPadding) * achievementCounter));
			// set the width/height
			trans.sizeDelta = new Vector2(trans.sizeDelta.x, 90);
            achievementCounter++;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
