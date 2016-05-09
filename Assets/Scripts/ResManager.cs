﻿using UnityEngine;
using System.Collections.Generic;

public class ResManager : MonoBehaviour {

    private static ResManager _shared;
    public static ResManager shared {
        get {
            if (_shared == null)
                _shared = GameObject.FindObjectOfType<ResManager>();
            return _shared;
        }
    }

	static string jType = "type";
	static string jTiles = "tiles";
	static string jAnimation = "animation";
	static string jX = "x";
	static string jY = "y";
	static string jSync = "sync";

	public TextAsset terrainCoords;
	public Texture2D terrainTexture;
	public int tileResolution = 8;
	public float animationSpeed = 0.3f;
	float terrainWidth, terrainHeight;
	
	public TGTile[] terrainTextures;
	Dictionary<int, TGTile[]> terrainTypes = new Dictionary<int, TGTile[]>();
	
	void Awake() {
		InitGraphic();
	}

	void InitGraphic() {
		terrainWidth = terrainTexture.width / tileResolution;
		terrainHeight = terrainTexture.height / tileResolution;
		JSONObject jsonRoot = new JSONObject(terrainCoords.text);
		List<TGTile> textures = new List<TGTile>();
		
		for (int i = 0; i < jsonRoot.list.Count; i++) {
			JSONObject jsonGroup = jsonRoot.list[i];
			int type = (int)jsonGroup.GetField(jType).i;

			JSONObject jsonTiles = jsonGroup.GetField(jTiles);

			if (jsonGroup.HasField(jAnimation) && jsonGroup.GetField(jAnimation).b == true) {
				// Пишем все в один тайл-анимацию
				Vector2[][] uvs = new Vector2[jsonTiles.list.Count][];
				for (int t = 0; t < jsonTiles.list.Count; t++) {
					JSONObject jsonTile = jsonTiles.list[t];
					uvs[t] = GetUV(jsonTile.GetField(jX).f, jsonTile.GetField(jY).f);
				}
				TGTile tile = new TGTile(uvs);
				if (jsonGroup.HasField(jSync)) {
					tile.sync = jsonGroup.GetField(jSync).b;
				}
				terrainTypes[type] = new TGTile[1];
				terrainTypes[type][0] = tile;
				textures.Add(tile);
			} else {
				// Пишем раздельные тайлы с общей группировкой
				terrainTypes[type] = new TGTile[jsonTiles.list.Count];
				for (int t = 0; t < jsonTiles.list.Count; t++) {
					JSONObject jsonTile = jsonTiles.list[t];
					Vector2[] uv = GetUV(jsonTile.GetField("x").f, jsonTile.GetField("y").f);
					TGTile tile = new TGTile(uv);
					if (jsonTile.HasField("weight")) {
						tile.weight = (int)jsonTile.GetField("weight").f;	
					}
					terrainTypes[type][t] = tile;
					textures.Add(tile);
				}
			}
		}
		terrainTextures = textures.ToArray();
	}
	
	Vector2[] GetUV(float x, float y) {
		Vector2[] uv = new Vector2[4];
		uv[0] = new Vector2(x/terrainWidth, (terrainHeight-1-y)/terrainHeight);
		uv[1] = new Vector2((x+1)/terrainWidth, (terrainHeight-1-y)/terrainHeight);
		uv[2] = new Vector2(x/terrainWidth, (terrainHeight-y)/terrainHeight);
		uv[3] = new Vector2((x+1)/terrainWidth, (terrainHeight-y)/terrainHeight);
		return uv;
	}

	public int TextureIndexForTileType(int type) {
		int result = 0;
		float total = 0;
		TGTile[] tiles = terrainTypes[type];
		for (int i = 0; i < tiles.Length; i++) {
			total += tiles[i].weight;
		}
		float random = Random.Range(0, total);
		float position = 0;
		for (int i = 0; i < tiles.Length; i++) {
			position += tiles[i].weight;
			if (position > random) {
				result = System.Array.IndexOf(terrainTextures, tiles[i]);
				break;
			}
		}
		return result;
	}	

}