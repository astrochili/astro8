using UnityEngine;

public class SettingsManager : MonoBehaviour {

	// Singletone
    private static SettingsManager _shared;
    public static SettingsManager shared {
        get {
            if (_shared == null)
                _shared = GameObject.FindObjectOfType<SettingsManager>();
            return _shared;
        }
    }

	public int tileResolution = 8;
	public float animationSpeed = 0.3f;

	public void LoadSettings() {
		
	}

}
