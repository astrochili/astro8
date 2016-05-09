public class TDTile {
	
	public int type;
	public int texture;
	public bool solid;
	public bool liquid;
	public float speed = 1.0f;
	public TileResourcesStruct resources;
	
	public TDTile(int type) {
		this.type = type;
		this.texture = ResManager.shared.TextureIndexForTileType(type);
		this.solid = (type == TILES.WALL || type == TILES.WINDOW);
		this.liquid = (type == TILES.WATER);
	}
}