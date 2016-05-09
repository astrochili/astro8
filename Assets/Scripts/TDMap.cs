using UnityEngine;

public class TDMap {
	
	TDTile[] tiles;
	public int width;
	public int height;
	
	public TDMap(int width, int height) {
		this.width = width;
		this.height = height;
		tiles = new TDTile[width * height];
		
		MakeTestMap();
	} 
	
	public TDTile[] GetTilesAt(int x, int y, int width, int height) {
		TDTile[] result = new TDTile[width*height];
		for (int i = 0, xi = 0; xi < width; xi++) {
			for (int yi = 0; yi < height; yi++, i++) {
				result[i] = GetTileAt(x+xi, y+yi);
			}
		}
		return result;
	}
	
	public TDTile GetTileAt(int x, int y) {
		if (x < 0 || x >= width || y < 0 || y >= height) {
			return null;
		}
		return tiles[y*width + x];
	}
	
	public bool SetTileAt(int x, int y, TDTile tile) {
		if (x < 0|| x >= width || y < 0 || y >= height) {
			return false;	
		}
		tiles[y*width + x] = tile;
		return true;
	}	
	
	
	// Создать случайную карту из поля и помещения
	void MakeTestMap() {
		MakePlace(0, 0, width, height);
		MakeLake();
		MakeRoom(5, 2, 8, 6);
	}
	
	void MakePlace(int left, int bottom, int width, int height) {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				SetTileAt(x, y, new TDTile(((Random.Range(0, 10) < 1))? TILES.TREE : TILES.GRASS));
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
					SetTileAt(left+x, bottom+y, new TDTile(((Random.Range(0, 2) < 1))? TILES.GRASS : TILES.WATER));
				} else {
					SetTileAt(left+x, bottom+y, new TDTile(TILES.WATER));
				}
			}
		}		
	}
	
	void MakeRoom(int left = 0, int bottom = 0, int width = 0, int height = 0) {
		if (left == 0) { left = Random.Range(1, this.width-3); }
		if (bottom == 0) { bottom = Random.Range(1, this.height-3); }
		if (width == 0) { width = Random.Range(3, this.width - left); }
		if (height == 0) { height = Random.Range(3, this.height - bottom); }
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width-1 || y == 0 || y == height-1) {
					SetTileAt(left+x, bottom+y, new TDTile(((Random.Range(0, 10) < 1))? TILES.WINDOW : TILES.WALL));
				} else {
					SetTileAt(left+x, bottom+y, new TDTile(TILES.FLOOR));
				}
			}
		}
	}	
}