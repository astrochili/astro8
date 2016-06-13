using System.Collections.Generic;

public class MDTile {
	
	public struct Resources {
		public int wood, coal, iron, copper, titan, silver, crudeoil, food;
	};
	
	public int type;
	public int texture;
	public bool solid;
	public bool liquid;
	public float speed = 1.0f;
	public Resources resources;
	
	public MDTile(int type) {
		this.type = type;
		this.texture = ResManager.shared.TextureIndexForTileType(type);
		this.solid = solidTypes.Contains(type);
		this.liquid = liquidTypes.Contains(type);
	}
}