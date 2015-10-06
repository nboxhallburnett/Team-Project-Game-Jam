using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    void FixedUpdate () {
        // Set the screen orientation depending on the device orientation
        switch (Input.deviceOrientation) {
            case DeviceOrientation.Portrait:
                Screen.orientation = ScreenOrientation.Portrait;
                return;
            case DeviceOrientation.PortraitUpsideDown:
                Screen.orientation = ScreenOrientation.PortraitUpsideDown;
                return;
            case DeviceOrientation.LandscapeRight:
                Screen.orientation = ScreenOrientation.LandscapeRight;
                return;
            case DeviceOrientation.LandscapeLeft:
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                return;
        }
    }
}
