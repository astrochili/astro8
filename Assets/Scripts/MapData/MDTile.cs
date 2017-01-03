using System.Collections.Generic;

public class MDTile {
	
	public struct Resources {
		public int wood, coal, iron, copper, titan, silver, crudeoil, food;
	};
	
	public string type;
	public GDTile texture;
	public bool solid;
	public bool liquid;
	public float speed = 1.0f;
	public Resources resources = new Resources();
	
	public MDTile(string type, bool solid = false, bool liquid = false, float speed = 1.0f) {
		this.type = type;
		this.texture = ResManager.shared.TextureForTile(type);
		this.solid = solid;
		this.liquid = liquid;
		this.speed = speed;
	}
	
}