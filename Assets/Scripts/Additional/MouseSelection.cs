using UnityEngine;

public class MouseSelection : MonoBehaviour {

	private MeshCollider _collider;
	private Vector2 currentTileXY;
	
	void Awake() {
		_collider = GetComponent<MeshCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		
		if (_collider.Raycast(ray, out hitInfo, Mathf.Infinity)) {
			int x = Mathf.FloorToInt(hitInfo.point.x);
			int y = Mathf.FloorToInt(hitInfo.point.y);
			currentTileXY.x = x;
			currentTileXY.y = y;
			MapManager.shared.selectionQuad.position = new Vector3(x, y, MapManager.shared.selectionQuad.position.z);
		}
	}
}
