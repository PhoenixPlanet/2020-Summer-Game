using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScale : MonoBehaviour {

	void Awake () {
		Screen.SetResolution(Screen.width, (Screen.width*9)/16, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
}
