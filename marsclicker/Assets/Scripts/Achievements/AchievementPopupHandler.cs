using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementPopupHandler : MonoBehaviour {

    /// <summary>
    /// Rather than setting the body text directly, consider using the SetBodyText method
    /// This will ensure the formatting of the text remains consistent
    /// </summary>
    public Text AchievementBodyText { get; set; }

    public Text SubBodyText { get; set; }

    public Text AchievementTitleText { get; set; }

    public ParticleSystem emitter { get; set; }

    private bool Initialised;


	// Use this for initialization
	void Start () {

	    if(!Initialised)
        {
            var particleSystems = GetComponentsInChildren<ParticleSystem>(true);
            foreach (ParticleSystem system in particleSystems)
            {
                emitter = system;
            }
            InitialiseTextObjects();
        }
	}

    public void InitialiseTextObjects()
    {
        Text[] textComponents = GetComponentsInChildren<Text>(true);
        foreach (Text comp in textComponents)
        {
            if (comp.name == "Title Text")
            {
                AchievementTitleText = comp;
            }
            else if (comp.name == "Body Text")
            {
                AchievementBodyText = comp;
            }
            else if (comp.name == "Sub Body Text")
            {
                SubBodyText = comp;
            }
        }

        Initialised = true;
    }

    public void SetBodyText(string achTitle, string achBody)
    {
        if(!Initialised)
        {
            var particleSystems = GetComponentsInChildren<ParticleSystem>(true);
            foreach(ParticleSystem system in particleSystems)
            {
                emitter = system;
            }
            InitialiseTextObjects();
        }
        AchievementBodyText.text = achTitle;
        SubBodyText.text = achBody;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }


}
