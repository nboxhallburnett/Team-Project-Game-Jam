using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Canvas landscapeCanvas;
    public Canvas portraitCanvas;
    public Canvas landscapeAchievementCanvas;
    public Canvas portraitAchievementCanvas;
    public GameObject planet;
    public bool CallUpdate;
    public static bool AchievementsShowing { get; private set; }

    void Start()
    {
        if(portraitAchievementCanvas != null)
        {
            portraitAchievementCanvas.enabled = false;
        }
        if(landscapeAchievementCanvas != null)
        {
            landscapeAchievementCanvas.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!AchievementsShowing && CallUpdate)
        {
            if (Screen.width >= Screen.height)
            {
                landscapeCanvas.enabled = true;
                portraitCanvas.enabled = false;

                if (planet != null)
                {
                    planet.transform.position = new Vector3(-3.333f, 0, 0);
                    planet.transform.localScale = new Vector3(5, 5, 5);
                }
            }
            else
            {
                landscapeCanvas.enabled = false;
                portraitCanvas.enabled = true;

                if (planet != null)
                {
                    planet.transform.position = new Vector3(0, 2, 0);
                    planet.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                }
            }
        }
    }

    public void ShowAchievements()
    {
        AchievementsShowing = true;
        landscapeCanvas.enabled = false;
        portraitCanvas.enabled = false;
        if (Screen.width > Screen.height)
        {
            landscapeAchievementCanvas.enabled = true;
            portraitAchievementCanvas.enabled = false;
        }
        else
        {
            landscapeAchievementCanvas.enabled = false;
            portraitAchievementCanvas.enabled = true;
        }
    }

    public void CloseAchievements()
    {
        AchievementsShowing = false;
        landscapeAchievementCanvas.enabled = false;
        portraitAchievementCanvas.enabled = false;        
        
        if (Screen.width > Screen.height)
        {
            landscapeCanvas.enabled = false;
        }
        else
        {
            portraitCanvas.enabled = true;
        }
    }
}