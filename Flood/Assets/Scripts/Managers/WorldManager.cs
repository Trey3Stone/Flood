using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoSingleton<WorldManager>
{
	public World World { get; private set; }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Create(int size) {
		print("WorldManager Create");
		World = WorldGenerator.Create(size);
		WorldGenerator.SpawnHives(World);
	}

	public void Save(string name) {

	}

	public void Load(string name) {

	}

	public bool CanPlace(int hexSize, Vector2Int hexPos) {

		for (int n = 0; n <= 6; n++) {
			if (!World.TileMap.HasNeighbor(hexPos.x, hexPos.y, n) || World.TileMap.GetNeighbor(hexPos.x, hexPos.y, n)) {
				return false;
			}
		}

		return true;

	}

	public GameObject PlaceEnt(GameObject ent, Vector2Int hexPos, float platformSize = 1) {
		
		if (platformSize == 2) {
			for (int n = 0; n <= 6; n++) {
				World.TileMap.SetNeighbor(hexPos.x, hexPos.y, n, true);
			}
		} else {
			World.TileMap[hexPos] = true;
		}

		Vector3 pos = HexHelper.HexToWorld(hexPos, World);

		GameObject outEnt = GameObject.Instantiate<GameObject>(ent, pos, Quaternion.identity, World.transform);

		if (platformSize > 0) {
			GameObject platform = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Platform"), pos, Quaternion.identity, outEnt.transform);
			platform.transform.localScale = new Vector3(platformSize, 1, platformSize);
			outEnt.transform.localPosition += new Vector3(0, 0.5f, 0);
		}

		return outEnt;
	}

}
