using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField] Sprite[] sprites = new Sprite[4];
	SpriteRenderer _renderer;
	Transform _transform;
	
	bool onBoat;
	
	void Awake () {
		_transform = GetComponent<Transform>();
		_renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Direction direction = Direction.None;
		if (Input.GetKeyUp("up")) {
			direction = Direction.Up;
		} else if (Input.GetKeyUp("down")) {
			direction = Direction.Down;
		} else if (Input.GetKeyUp("left")) {
			direction = Direction.Left;
		} else if (Input.GetKeyUp("right")) {
			direction = Direction.Right;
		}
		
		if (direction > Direction.None) {
			Vector3 position = _transform.position;
			if (direction == Direction.Up) {
				_renderer.sprite = sprites[(int)Direction.Up-1];
				position.y += 1.0f;
			} else if (direction == Direction.Down) {
				_renderer.sprite = sprites[(int)Direction.Down-1];
				position.y -= 1.0f;
			} else if (direction == Direction.Left) {
				_renderer.sprite = sprites[(int)Direction.Left-1];
				position.x -= 1.0f;
			} else if (direction == Direction.Right) {
				_renderer.sprite = sprites[(int)Direction.Right-1];
				position.x += 1.0f;
			}
			AttempToMove(position);			
		}
	}
	
	void AttempToMove(Vector2 position) {
		TDTile tile = MapManager.shared.map.GetTileAt((int)position.x, (int)position.y);
		TDObject obj = MapManager.shared.GetObjectAt(position);
		TDUnit unit = MapManager.shared.GetUnitAt(position);
		
		if (tile == null || tile.solid || (tile.liquid && !onBoat)) {
			Debug.Log("Solid surface!");
		} else if (obj != null && obj.solid) {
			Interact(obj);
		} else if (unit != null) {
			if (unit.friendly) {
				Talk(unit);	
			} else {
				Attack(unit);
			}
		} else {
			_transform.position = position;
			MapManager.shared.UpdateChunks();
		}
	}
	
	void Interact(TDObject obj) {
		Debug.Log("Interact!");
	}
	
	void Talk(TDUnit unit) {
		Debug.Log("Talk!");
	}
	
	void Attack(TDUnit unit) {
		Debug.Log("Attack!");
	}
	
}
