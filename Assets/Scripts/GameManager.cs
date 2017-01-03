using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SettingsManager.shared.LoadSettings();
		ResManager.shared.LoadResources();
		MapManager.shared.Init();
	}
	
}
