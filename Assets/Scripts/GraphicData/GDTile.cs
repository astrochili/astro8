using UnityEngine;

// GDTile хранит в себе информацию о развертке текстуры тайла, в том числе анимацию

public class GDTile {
	public Vector2[][] uvs; // Массив разверток, если > 1 - это анимация
	public float weight = 1.0f; // Вес тайла при рандомном выборе
	public bool sync = true; // Синхронизация анимации
	public bool isAnimated {
		get {
			return (uvs.Length > 1);	
		}
	}
	
	// Кастомная иницилизация с одним кадром
	public GDTile(Vector2[] uv) {
		this.uvs = new Vector2[1][];
		this.uvs[0] = uv;
	}
	
	// Кастомная иницилизация с анимацией
	public GDTile(Vector2[][] uvs) {
		this.uvs = uvs;
	}	
}