using UnityEngine;
using System.Collections.Generic;

public class ResManager : MonoBehaviour {

	// Singletone
    private static ResManager _shared;
    public static ResManager shared {
        get {
            if (_shared == null)
                _shared = GameObject.FindObjectOfType<ResManager>();
            return _shared;
        }
    }
	
	// JSON keys
	static string jTile = "tile";
	static string jWeight = "weight";
	static string jAnimation = "animation";
	static string jX = "x";
	static string jY = "y";
	static string jSync = "sync";

	// Graphic resources
	public TextAsset tilesDatabase;
	public Texture2D tilesTexture;
	float atlasWidth, atlasHeight;
	
	Dictionary<string, GDTile[]> tiles = new Dictionary<string, GDTile[]>();
	Dictionary<string, GDSprite> objects = new Dictionary<string, GDSprite>();
	Dictionary<string, GDSprite> units = new Dictionary<string, GDSprite>();
	
	public void LoadResources() {
		LoadTiles();
	}
	
	void LoadTiles() {
		this.atlasWidth = this.tilesTexture.width / SettingsManager.shared.tileResolution;
		this.atlasHeight = this.tilesTexture.height / SettingsManager.shared.tileResolution;
		JSONObject root = new JSONObject(this.tilesDatabase.text);
		
		// Переберем тайлы
		for (int k = 0; k < root.list.Count; k++) {
			JSONObject jsonTile = root.list[k];
			GDTile[] textures = new GDTile[jsonTile.list.Count];
			string name = root.keys[k];
			tiles[name] = textures;

			// Переберем варианты текстур тайла
			for (int t = 0; t < jsonTile.list.Count; t++) {
				JSONObject jsonTexture = jsonTile.list[t];

				// Если текстура анимированая, переберем координаты кадров и добавим в массив разверток
				if (jsonTexture.HasField(jAnimation)) {
					JSONObject jsonFrames = jsonTexture.GetField(jAnimation);
					Vector2[][] uvs = new Vector2[jsonFrames.list.Count][];
					for (int f = 0; f < jsonFrames.list.Count; f++) {
						JSONObject jsonFrame = jsonFrames.list[t];
						uvs[f] = GetUV(jsonFrame.GetField(jX).f, jsonFrame.GetField(jY).f);
					}
					GDTile texture = new GDTile(uvs); 
					if (jsonTexture.HasField(jSync)) {
						texture.sync = jsonTexture.GetField(jSync).b;
					}
					textures[t] = texture;
				
				// Иначе текстура не анимированая, просто добавим координату одной развертки
				} else {
					Vector2[] uv = GetUV(jsonTile.GetField(jX).f, jsonTile.GetField(jY).f);
					GDTile texture = new GDTile(uv);
					if (jsonTile.HasField(jWeight)) {
						texture.weight = (int)jsonTile.GetField(jWeight).f;	
					}
					textures[t] = texture;
				}
			}
		}
	}

	// Возвращает развертку по привычным коодинатам (старт сверху слева)
	Vector2[] GetUV(float x, float y) {
		Vector2[] uv = new Vector2[4];
		uv[0] = new Vector2(x/atlasWidth, (atlasHeight-1-y)/atlasHeight);
		uv[1] = new Vector2((x+1)/atlasWidth, (atlasHeight-1-y)/atlasHeight);
		uv[2] = new Vector2(x/atlasWidth, (atlasHeight-y)/atlasHeight);
		uv[3] = new Vector2((x+1)/atlasWidth, (atlasHeight-y)/atlasHeight);
		return uv;
	}

	// Возвращает текстуру по имени тайла
	public GDTile TextureForTile(string type) {
		GDTile texture = null;
		float total = 0;
		GDTile[] textures = this.tiles[type];
		for (int t = 0; t < textures.Length; t++) {
			total += textures[t].weight;
		}
		float random = Random.Range(0, total);
		float position = 0;
		for (int p = 0; p < textures.Length; p++) {
			position += textures[p].weight;
			if (position > random) {
				texture = textures[p];
				break;
			}
		}
		return texture;
	}

	// Возвращает спрайт по имени объекта
	public GDSprite TextureForObject(string type) {
		if (objects.ContainsKey(type)) {
			return objects[type];
		} else {
			return null;
		}
	}

	// Возвращает спрайт по имени юнита
	public GDSprite TextureForUnit(string type) {
		if (units.ContainsKey(type)) {
			return units[type];
		} else {
			return null;
		}
	}	

}