using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {
	
    private static MapManager _shared;
    public static MapManager shared {
        get {
            if (_shared == null)
                _shared = GameObject.FindObjectOfType<MapManager>();
            return _shared;
        }
    }
	
	public Transform selectionQuad;
	
	public MDMap map;
	[SerializeField] int xSizeChunks, ySizeChunks;
	[SerializeField] int chunkSize = 8;
	[SerializeField] GameObject chunkPrefab, objectPrefab, unitPrefab;
	Vector2 bottomLeft, topRight;
	
	List<MapChunk> chunksPool = new List<MapChunk>();
	List<MapObject> objectsPool = new List<MapObject>();
	List<MapUnit> unitsPool = new List<MapUnit>();

	List<List<MapChunk>> chunks = new List<List<MapChunk>>();
	List<MapObject> objects = new List<MapObject>();
	List<MapUnit> units = new List<MapUnit>();
	
	int xChunks { // Количество чанков на экране по X
		get {
			return chunks.Count;
		}
	}
	int yChunks { // Количество чанков на экране по Y
		get {
			if (xChunks > 0) {
				return chunks[xChunks-1].Count;
			}
			return 0;
		}
	}
	
	
	void Start() {
		map = new MDMap(xSizeChunks * chunkSize, ySizeChunks * chunkSize);
		UpdateChunks();
		float speed = ResManager.shared.animationSpeed;
		InvokeRepeating("Animate", speed, speed);  
			
	}
	
	void Animate() {
		for (int x = 0; x < xChunks; x++) {
			for (int y = 0; y < yChunks; y++) {
				chunks[x][y].Animate();
			}
		}
	}
	
	// Получить теоретическую позицию чанка по общей позиции
	Vector2 ChunkPositionForWorldPosition(Vector2 worldPosition) {
		float x = Mathf.FloorToInt(worldPosition.x / (float)chunkSize) *chunkSize;
		float y = Mathf.FloorToInt(worldPosition.y / (float)chunkSize) *chunkSize;
		return new Vector2 (x, y);
	}		
		
	// Обновить чанки (добавить новых? удалить не нужные?)
	public void UpdateChunks() {
		// Определим правильную позицию для первого чанка
		bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
		bottomLeft = ChunkPositionForWorldPosition(bottomLeft);
		bottomLeft = new Vector2(Mathf.Clamp(bottomLeft.x, 0, (xSizeChunks-1)*chunkSize), Mathf.Clamp(bottomLeft.y, 0, (ySizeChunks-1)*chunkSize));
				
		// Определим правильную позицию для последнего чанка
		topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
		topRight = ChunkPositionForWorldPosition(topRight);
		topRight = new Vector2(Mathf.Clamp(topRight.x, 0, (xSizeChunks-1)*chunkSize), Mathf.Clamp(topRight.y, 0, (ySizeChunks-1)*chunkSize));
			
		// Если нет чанков, то пора бы их создать	
		if (xChunks == 0) {
			int width = 1+(int)((topRight.x - bottomLeft.x) / (float)chunkSize);
			int height = 1+(int)((topRight.y - bottomLeft.y) / (float)chunkSize);			
			for (int x = 0; x < width; x++) {
				chunks.Add(new List<MapChunk>());
				for (int y = 0; y < height; y++) {
					Vector2 position = new Vector2(bottomLeft.x + x*chunkSize, bottomLeft.y + y*chunkSize);
					MapChunk chunk = BuildChunk(position);
					chunks[x].Add(chunk);
				}
			}
			return;	
		}
		
		// Фактические первый и последний чанки
		MapChunk firstChunk = chunks[0][0];
		MapChunk lastChunk = chunks[xChunks-1][yChunks-1];
		
		if (firstChunk.position.x > bottomLeft.x) {
			AddChunks(Border.Left);
		} else if (firstChunk.position.x < bottomLeft.x) {
			RemoveChunks(Border.Left);
		}

		if (firstChunk.position.y > bottomLeft.y) {
			AddChunks(Border.Bottom);
		} else if (firstChunk.position.y < bottomLeft.y) {
			RemoveChunks(Border.Bottom);
		}

		if (lastChunk.position.x < topRight.x) {
			AddChunks(Border.Right);	
		} else if (lastChunk.position.x > topRight.x) {
			RemoveChunks(Border.Right);
		}

		if (lastChunk.position.y < topRight.y) {
			AddChunks(Border.Top);
		} else if (lastChunk.position.y > topRight.y) {
			RemoveChunks(Border.Top);
		}
	}
	
	// Добавить чанки с нужной стороны
	void AddChunks(Border side) {
		List<MapChunk> newChunks;
		switch (side) {
			case Border.Left: {
				newChunks = new List<MapChunk>();
				for (int y = 0; y < yChunks; y++) {
					Vector2 position = new Vector2(bottomLeft.x, bottomLeft.y + y*chunkSize);
					MapChunk chunk = BuildChunk(position);
					newChunks.Add(chunk);
				}
				chunks.Insert(0, newChunks);
				break;
			}
			case Border.Bottom: {
				for (int x = 0; x < xChunks; x++) {
					Vector2 position = new Vector2(bottomLeft.x + x*chunkSize, bottomLeft.y);
					MapChunk chunk = BuildChunk(position);
					chunks[x].Insert(0, chunk);
				}
				break;
			}
			case Border.Right: {
				newChunks = new List<MapChunk>();
				for (int y = 0; y < yChunks; y++) {
					Vector2 position = new Vector2(topRight.x, bottomLeft.y + y*chunkSize);
					MapChunk chunk = BuildChunk(position);
					newChunks.Add(chunk);
				}
				chunks.Add(newChunks);
				break;
			}
			case Border.Top: {
				for (int x = 0; x < xChunks; x++) {
					Vector2 position = new Vector2(bottomLeft.x + x*chunkSize, topRight.y);
					MapChunk chunk = BuildChunk(position);
					chunks[x].Add(chunk);
				}			
				break;
			}
		}		
	}
	
	// Удалить чанки с нужной стороны
	void RemoveChunks(Border side) {
		switch (side) {
			case Border.Left: {
				for (int y = 0; y < yChunks; y++) {
					MapChunk chunk = chunks[0][y];
					chunk.SwitchOff();
					chunksPool.Add(chunk);
				}
				chunks.RemoveAt(0);
				break;
			}
			case Border.Bottom: {
				for (int x = 0; x < xChunks; x++) {
					MapChunk chunk = chunks[x][0];
					chunk.SwitchOff();
					chunksPool.Add(chunk);
					chunks[x].Remove(chunk);
				}							
				break;
			}
			case Border.Right: {
				for (int y = 0; y < yChunks; y++) {
					MapChunk chunk = chunks[xChunks-1][y];
					chunk.SwitchOff();
					chunksPool.Add(chunk);
				}										
				chunks.RemoveAt(xChunks-1);
				break;
			}
			case Border.Top: {
				for (int x = 0; x < xChunks; x++) {
					MapChunk chunk = chunks[x][yChunks-1];
					chunk.SwitchOff();
					chunksPool.Add(chunk);
					chunks[x].Remove(chunk);
				}										
				break;				
			}
		}
	}
	
	// Разместить и раскрасить чанк
	MapChunk BuildChunk(Vector2 position) {
		MapChunk chunk = null;
		if (chunksPool.Count > 0) {
			chunk = chunksPool[0];
			chunksPool.Remove(chunk);
			chunk.gameObject.SetActive(true);
		} else {
			GameObject chunkObject = Instantiate(chunkPrefab);
			chunkObject.transform.SetParent(transform);
			chunk = chunkObject.GetComponent<MapChunk>();
			chunk.BuildMesh(chunkSize);		
		}
		chunk.name = "Chunk_"+position.x+"_"+position.y;
		chunk.position = position;
		MDTile[] tiles = map.GetTilesAt((int)position.x, (int)position.y, chunkSize, chunkSize);
		GDTile[] textures = new GDTile[tiles.Length];
		for (int i = 0; i < tiles.Length; i++) {
			textures[i] = ResManager.shared.terrainTextures[tiles[i].texture];
		}
		chunk.SetTextures(textures);
		return chunk;
	}
}