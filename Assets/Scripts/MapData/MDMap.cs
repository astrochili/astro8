using UnityEngine;
using System.Collections.Generic;

public class MDMap {
		
	MDTile[] tiles;
	public int width;
	public int height;
	
	Dictionary<Vector2, MDUnit> units = new Dictionary<Vector2, MDUnit>();
	Dictionary<Vector2, MDObject> objects = new Dictionary<Vector2, MDObject>();

	public MDMap(int width, int height) {
		this.width = width;
		this.height = height;
		tiles = new MDTile[width * height];
		
		MakeTestMap();
	} 
	
	public MDTile[] GetTilesAt(int x, int y, int width, int height) {
		MDTile[] result = new MDTile[width*height];
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
	
	// Получить объект по координатам
	public MDObject GetObjectAt(Vector2 position) {
		if (objects.ContainsKey(position)) {
			 return objects[position];
		} else {
			return null;
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
	
	
	// Создать тестовую карту
	void MakeTestMap() {
		MakePlace(0, 0, width, height);
		MakeLake();
		MakeRoom(8, 6, 12, 8);
		InitUnits();
	}
	
	void MakePlace(int left, int bottom, int width, int height) {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				SetTileAt(x, y, new MDTile(((Random.Range(0, 10) < 1))? TILES.TREE : TILES.GRASS));
			}
		}		
	}
	
	void MakeLake(int left = 0, int bottom = 0, int width = 0, int height = 0) {
		if (left == 0) { left = (int)((float)this.width/4); }
		if (bottom == 0) { bottom = (int)((float)this.height/4); }
		if (width == 0) { width = this.width - left*2; }
		if (height == 0) { height = this.height - bottom*2; }
		
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width-1 || y == 0 || y == height-1) {
					SetTileAt(left+x, bottom+y, new MDTile(((Random.Range(0, 2) < 1))? TILES.GRASS : TILES.WATER));
				} else {
					SetTileAt(left+x, bottom+y, new MDTile(TILES.WATER));
				}
			}
		}		
	}
	
	void MakeRoom(int left = 0, int bottom = 0, int width = 0, int height = 0) {
		if (left == 0) { left = Random.Range(1, this.width-3); }
		if (bottom == 0) { bottom = Random.Range(1, this.height-3); }
		if (width == 0) { width = Random.Range(3, this.width - left); }
		if (height == 0) { height = Random.Range(3, this.height - bottom); }
				
		int x, y;
		Border doorSide = (Border) Random.Range(0, 4);
		var side = Border.Bottom;
		
		for (x = 0; x < width; x++) {
			for (y = 0; y < height; y++) { 
				if (x == 0 || x == width-1 || y == 0 || y == height-1) {
					if (x == 0) {
						side = Border.Left;
					} else if (x == width-1) {
						side = Border.Right;
					} else if (y == 0) {
						side = Border.Bottom;
					} else if (x == height-1) {
						side = Border.Top;
					}					
					if (side == doorSide && (x == width/2 || y == height/2) ) {
						SetTileAt(left+x, bottom+y, new MDTile(TILES.FLOOR));
						objects.Add(new Vector2(left+x, bottom+y), new MDObject(OBJECTS.DOOR));
					} else {
						SetTileAt(left+x, bottom+y, new MDTile(((Random.Range(0, 5) < 1))? TILES.WINDOW : TILES.WALL));	
					}
				} else {
					SetTileAt(left+x, bottom+y, new MDTile(TILES.FLOOR));
				}
			}
		}

		MDObject[] interier = new MDObject[3];
		interier[0] = new MDObject(OBJECTS.BED);
		interier[1] = new MDObject(OBJECTS.BOX);
		interier[2] = new MDObject(OBJECTS.TERMINAL);
		for (int i = 0; i < interier.Length; i++) {
			Vector2 position = Vector2.zero;
			while (objects.ContainsKey(position) || position == Vector2.zero) {
				position = new Vector2(Random.Range(left+1, left+width-1), Random.Range(bottom+1, bottom+height-1));
			}
			objects.Add(position, interier[0]);
		}
	}
	
	void InitUnits() {
		int count = tiles.Length / 16;
		for (int i = 0; i < count; i++) {
			Vector2 position = Vector2.zero;
			while (units.ContainsKey(position) || position == Vector2.zero) {
				position = new Vector2(Random.Range(0, width), Random.Range(0, height));
			}
			int type = Random.Range(0, UNITS.count);
			units.Add(position, new MDUnit(type));
		}
	}

}