using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGenerator
{
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



		int gridX = 2 * sideLength + 1; // TODO: redundant
		int gridY = 2 * sideLength + 1;


		// Creating vertices.
		float[,] heightGrid = new float[gridX, gridY];
		HexHelper.FillHexGrid(heightGrid, sideLength);
		
		for (int ix = 0; ix < gridX; ix++) {
			for (int iy = 0; iy < gridY; iy++) {
				if (heightGrid[ix, iy] < 0)
					continue;

				int height = (iy < gridY/2) ? 2 : 0;
				heightGrid[ix, iy] = height;
			}
		}


		// Compiling triangles
		int[,] iGrid = new int[gridX, gridY];
		mesh.vertices = MeshHelper.HexGridToVerts(worldSize, heightGrid, iGrid);
		mesh.triangles = MeshHelper.HexGridToTris(iGrid);

		// TODO: Consider vertex doubling.
		mesh.RecalculateNormals();

		RenderTexture worldTexture = HexHelper.HexTexture(heightGrid);

		world.Load(worldSize, heightGrid, mesh, worldTexture);
	}

	/*
	private static bool ShouldCutStraight(Vector3[,] verts, int x, int z) {
		return ((verts[x, z].y + verts[x + 1, z + 1].y) >= (verts[x, z + 1].y + verts[x + 1, z + 1].y));
	}
	*/

}
