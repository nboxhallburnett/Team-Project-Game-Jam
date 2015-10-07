using UnityEngine;
using System.Collections;

public class AchievementPopup {

    public GUIStyle style { get; set; }
    public GUIContent content { get; set; }
    public Rect rectangle { get; set; }

    /// <summary>
    /// An achievement popup that will be displayed when the player unlocks a new achievement.
    /// </summary>
    /// <param name="achievementTextFont">Optional font for the text. If null it will use the default Unity font</param>
    /// <param name="achievementBackground">Optional background for the popup. If null, no background will be used</param>
    public AchievementPopup(Font achievementTextFont = null, Texture2D achievementBackground = null)
    {
        content = new GUIContent();
        style = new GUIStyle();
        style.alignment = TextAnchor.UpperCenter;        
        style.normal.textColor = Color.white;
        style.padding = new RectOffset(10, 10, 10, 10);
        style.richText = true;

        if(achievementBackground != null)
        {
            style.normal.background = achievementBackground;
        }
        if(achievementTextFont != null)
        {
            style.font = achievementTextFont;
        }
    }

    /// <summary>
    /// Method to draw the popup to the screen.
    /// NOTE: This must be called inside an OnGUI method!
    /// </summary>
    public void Draw()
    {
        rectangle = GUILayoutUtility.GetRect(content, style);
        GUI.Label(new Rect((Screen.width / 4) - (rectangle.width / 4), 10, rectangle.width, rectangle.height), content, style);
    }
}
