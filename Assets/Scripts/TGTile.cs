using UnityEngine;

public class TGTile {
	public Vector2[][] uvs; // Массив разверток. Если больше 1 - то это анимация
	public float weight = 1.0f; // Вес тайла при рандомном выборе (напр. какая трава из 16 будет на тайле травы?)
	public bool sync = true; // Синхронизация анимации тайлов на карте
	public bool animation {
		get {
			return (uvs.Length > 1);	
		}
	}
	
	public TGTile(Vector2[] uv) {
		this.uvs = new Vector2[1][];
		this.uvs[0] = uv;
	}
	
	public TGTile(Vector2[][] uvs) {
		this.uvs = uvs;
	}	
}