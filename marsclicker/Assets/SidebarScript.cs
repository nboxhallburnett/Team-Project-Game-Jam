using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SidebarScript : MonoBehaviour {

    public GameObject otherHamburger;
    public GameObject sidebar;
    public bool bottom;

	public void ClickEvent () {
        if (bottom) {
            sidebar.GetComponent<RectTransform>().localScale = Vector3.zero;
        } else {
            sidebar.GetComponent<RectTransform>().localScale = Vector3.one;
        }
        otherHamburger.GetComponent<Image>().enabled = true;
        GetComponent<Image>().enabled = false;
    }
}
