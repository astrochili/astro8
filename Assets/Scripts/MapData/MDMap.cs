using UnityEngine;
using System.Collections.Generic;

public class MDMap {
	
	public int width;
	public int height;
	public int size {
		get {
			return width * height;
		}
	}	
	MDTile[] tiles;
	
	Dictionary<Vector2, MDUnit> units = new Dictionary<Vector2, MDUnit>();
	Dictionary<Vector2, MDObject> objects = new Dictionary<Vector2, MDObject>();

	public MDMap(int width, int height) {
		this.width = width;
		this.height = height;
		this.tiles = new MDTile[this.size];
	} 
	
	public MDTile[] GetTilesAt(int x, int y, int width, int height) {
		MDTile[] result = new MDTile[this.size];
		for (int i = 0, xi = 0; xi < width; xi++) {
			for (int yi = 0; yi < height; yi++, i++) {
				result[i] = GetTileAt(x+xi, y+yi);
			}
		}
		return result;
	}
	
	public MDTile GetTileAt(int x, int y) {
		if (x < 0 || x >= width || y < 0 || y >= height) {
			return null;
		}
		return tiles[y*width + x];
	}
	
	public bool SetTileAt(int x, int y, MDTile tile) {
		if (x < 0|| x >= width || y < 0 || y >= height) {
			return false;	
		}
		tiles[y*width + x] = tile;
		return true;
	}	
	
	// Установить объект по координатам
	public bool SetObjectAt(Vector2 position, MDObject obj) {
		if (objects.ContainsKey(position)) {
			return false;
		} else {
			objects.Add(position, obj);
			return true;
		}
	}

	// Получить объект по координатам
	public MDObject GetObjectAt(Vector2 position) {
		if (objects.ContainsKey(position)) {
			 return objects[position];
		} else {
			return null;
		}
	}

	// Установить юнит по координатам
	public bool SetUnitAt(Vector2 position, MDUnit unit) {
		if (units.ContainsKey(position)) {
			return false;
		} else {
			units.Add(position, unit);
			return true;
		}
	}

	// Получить юнит по координатам
	public MDUnit GetUnitAt(Vector2 position) {
		if (units.ContainsKey(position)) {
			 return units[position];
		} else {
			return null;
		}
	}

}