using UnityEngine;

public class MapGenerator {

	// Контекст текущей генерации
	static MDMap map;

	// Создать тестовую карту
	static public MDMap Make(int width, int height) {
		MDMap cached = new MDMap(width, height);
		map = cached;
		MakePlace(0, 0, width, height);
		MakeLake();
		MakeRoom(8, 6, 12, 8);
		InitUnits();
		map = null;
		return cached;
	}
	
	static void MakePlace(int left, int bottom, int width, int height) {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				map.SetTileAt(x, y, new MDTile(((Random.Range(0, 10) < 1))? "tree" : "grass"));
			}
		}		
	}
	
	static void MakeLake(int left = 0, int bottom = 0, int width = 0, int height = 0) {
		if (left == 0) { left = (int)((float)map.width/4); }
		if (bottom == 0) { bottom = (int)((float)map.height/4); }
		if (width == 0) { width = map.width - left*2; }
		if (height == 0) { height = map.height - bottom*2; }
		
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width-1 || y == 0 || y == height-1) {
					map.SetTileAt(left+x, bottom+y, new MDTile(((Random.Range(0, 2) < 1))? "grass" : "water"));
				} else {
					map.SetTileAt(left+x, bottom+y, new MDTile("water"));
				}
			}
		}		
	}
	
	static void MakeRoom(int left = 0, int bottom = 0, int width = 0, int height = 0) {
		if (left == 0) { left = Random.Range(1, map.width-3); }
		if (bottom == 0) { bottom = Random.Range(1, map.height-3); }
		if (width == 0) { width = Random.Range(3, map.width - left); }
		if (height == 0) { height = Random.Range(3, map.height - bottom); }
				
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
						map.SetTileAt(left+x, bottom+y, new MDTile("floor"));
						map.SetObjectAt(new Vector2(left+x, bottom+y), new MDObject(OBJECTS.DOOR));
					} else {
						map.SetTileAt(left+x, bottom+y, new MDTile(((Random.Range(0, 5) < 1))? "window" : "wall"));	
					}
				} else {
					map.SetTileAt(left+x, bottom+y, new MDTile("floor"));
				}
			}
		}

		MDObject[] interier = new MDObject[3];
		interier[0] = new MDObject(OBJECTS.BED);
		interier[1] = new MDObject(OBJECTS.BOX);
		interier[2] = new MDObject(OBJECTS.TERMINAL);
		for (int i = 0; i < interier.Length; i++) {
			Vector2 position = Vector2.zero;
			while (map.GetObjectAt(position) != null || position == Vector2.zero) {
				position = new Vector2(Random.Range(left+1, left+width-1), Random.Range(bottom+1, bottom+height-1));
			}
			map.SetObjectAt(position, interier[0]);
		}
	}
	
	static void InitUnits() {
		int count = map.size / 16;
		for (int i = 0; i < count; i++) {
			Vector2 position = Vector2.zero;
			while (map.GetUnitAt(position) != null || position == Vector2.zero) {
				position = new Vector2(Random.Range(0, map.width), Random.Range(0, map.height));
			}
			//int type = Random.Range(0, UNITS.count);
			map.SetUnitAt(position, new MDUnit("bunny"));
		}
	}

}
