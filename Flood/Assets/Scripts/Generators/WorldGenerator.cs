using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGenerator
{
	private const float WORLD_FLOOR_DEPTH = 2f;

	private static GameObject worldPrefab = Resources.Load("Prefabs/World") as GameObject;


	public static World Create(int sideLength) {
		Debug.Log("WorldGenerator Create");
		World outWorld = GameObject.Instantiate(worldPrefab).GetComponent<World>();
		CreateWorld(outWorld, sideLength);

		return outWorld;
	}

	private static void CreateWorld(World world, int sideLength) {
		Mesh mesh = new Mesh();
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		Vector2 worldSize = new Vector2(2 * sideLength, 2 * HexHelper.THETA * sideLength);

		int gridSize = 2 * sideLength + 1;

		// Creating vertices.
		HexGrid<float> heightGrid = new HexGrid<float>(gridSize);
		HexHelper.FillHexGrid(heightGrid, sideLength);

		//TestWalls(gridSize, heightGrid);

		world.Load(worldSize, heightGrid);
	}

	private static void TestWalls(int gridSize, HexGrid<float> heightGrid) {
		for (int ix = 0; ix < gridSize; ix++) {
			for (int iy = 0; iy < gridSize; iy++) {
				if (heightGrid[ix, iy] < 0)
					continue;

				int height = 0;
				int closest = (iy) / (gridSize / 6);
				closest *= gridSize / 6;

				if ((closest != 0 && closest <= 0.9f * gridSize) && Mathf.Abs(iy - closest) < 3) {
					int row = gridSize / closest;

					//Debug.Log(gridY + ", " + closest);
					height = 10 + 5 * row;

					if ((closest % 2) == 0) {
						if (ix < gridSize / 3)
							height = 0;
					} else {
						if (ix > 2 * gridSize / 3)
							height = 0;
					}
				}

				//int height = (Mathf.Abs(iy - gridY/2) < 2) ? 40 : 0;
				//if (Mathf.Abs(ix - gridX / 2) < 10) {
				//	height = 0;
				//}
				heightGrid[ix, iy] = height;
			}
		}
	}

	/*
	private static Mesh CreateWorldEdge(World world, int sideLength) { // TODO: Use submesh, Integrate with mask
		Mesh mesh = new Mesh();
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		Vector2 worldSize = new Vector2(2 * sideLength, 2 * HexHelper.THETA * sideLength);



		int gridSize = 2 * sideLength + 1; // TODO: redundant
		//int gridY = 2 * sideLength + 1;

		Vector2Int curPos = new Vector2Int(gridSize / 2, 0);

		while (true) {

		}



		//mesh.SetIndices(null, MeshTopology.Quads, 1);

		return mesh;
	}
	*/
	/*
	private static bool ShouldCutStraight(Vector3[,] verts, int x, int z) {
		return ((verts[x, z].y + verts[x + 1, z + 1].y) >= (verts[x, z + 1].y + verts[x + 1, z + 1].y));
	}
	*/

}
