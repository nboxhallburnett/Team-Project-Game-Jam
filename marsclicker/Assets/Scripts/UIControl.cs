using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

    public Canvas landscapeCanvas;
    public Canvas portraitCanvas;
    public GameObject planet;
	
	// Update is called once per frame
	void Update () {
        if (Screen.width >= Screen.height) {
            landscapeCanvas.enabled = true;
            portraitCanvas.enabled = false;

            if (planet != null) {
                planet.transform.position = new Vector3(-3.333f, 0, 0);
                planet.transform.localScale = new Vector3(5, 5, 5);
            }
        } else {
            landscapeCanvas.enabled = false;
            portraitCanvas.enabled = true;

            if (planet != null) {
                planet.transform.position = new Vector3(0, 2, 0);
                planet.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            }
        }
    }
}
