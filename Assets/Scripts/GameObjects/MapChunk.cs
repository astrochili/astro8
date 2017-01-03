using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class MapChunk : MonoBehaviour {

	[SerializeField] Transform _transform;
	[SerializeField] MeshFilter _filter;
	[SerializeField] MeshCollider _collider;
	[SerializeField] MeshRenderer _renderer;
			
	Mesh mesh;
	Vector3[] vertices;
	int[] triangles;
	Vector2[] uv;
	GDTile[] textures;
	Dictionary<int, int> animations = new Dictionary<int, int>();

	public Vector2 position {
		get {
			return _transform.position;
		}
		set {
			_transform.position = value;
		}
	}

	public void BuildMesh(int size) {
		int tilesCount = size * size;
		int trianglesCount = tilesCount * 6;
		int verticesCount = tilesCount * 4;
		
		// Generate the mesh data
		vertices = new Vector3[verticesCount];
		triangles = new int[trianglesCount];
		Vector3[] normals = new Vector3[verticesCount];
		uv = new Vector2[verticesCount];
				
		// Loop for tiles
		for (int i = 0, t = 0, x = 0; x < size; x++) {
			for (int y = 0; y < size; y++, i+=4, t+=6) {
				vertices[i+0] = new Vector3(x+0, y+0);
				vertices[i+1] = new Vector3(x+1, y+0);
				vertices[i+2] = new Vector3(x+0, y+1);
				vertices[i+3] = new Vector3(x+1, y+1);
				triangles[t+0] = i+0;
				triangles[t+2] = triangles[t+3] = i+1;
				triangles[t+1] = triangles[t+4] = i+2;
				triangles[t+5] = i+3;
				normals[i+0] = normals[i+1] = normals[i+2] = normals[i+3] = Vector3.forward;
			}
		}
		
		// Create a new mesh and populate with the data
		mesh = new Mesh();
		mesh.name = "Chunk Grid";		
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		
		// Assign our mesh to our filter
		_filter.mesh = mesh;
		_collider.sharedMesh = mesh; 
		_renderer.sharedMaterials[0].mainTexture = ResManager.shared.tilesTexture;
	}

	public void SetTextures(GDTile[] textures) {
		this.textures = textures;
		animations.Clear();
		int frame;
		for (int i = 0; i < textures.Length; i++) {
			GDTile tile = textures[i];
			Vector2[] uv = tile.uvs[0];
			frame = 0;
			if (tile.isAnimated) {				
				if (!tile.sync) {
					frame = Random.Range(0, tile.uvs.Length);
					uv = tile.uvs[frame];
				}
				animations[i] = frame;			
			}
			SetUV(i, uv);
		}
		mesh.uv = this.uv;
	}
	
	public void Animate() {
		int[] keys = new int[animations.Keys.Count];
		animations.Keys.CopyTo(keys, 0);
		for (int k = 0; k < keys.Length; k++) {
			int tile = keys[k]; 
			int frame = animations[tile];
			frame++;
			if (frame == textures[tile].uvs.Length) {
				frame = 0;
			}
			animations[tile] = frame;
			Vector2[] uv = textures[tile].uvs[frame];
			SetUV(tile, uv);			
		}
		mesh.uv = this.uv;
	}
	
	void SetUV(int i, Vector2[] uv) {
		this.uv[i*4+0] = uv[0];
		this.uv[i*4+1] = uv[1];
		this.uv[i*4+2] = uv[2];
		this.uv[i*4+3] = uv[3];		
	}

	public void SwitchOff() {
		this.name = "Chunk_in_pool";
		gameObject.SetActive(false);
	}	
}