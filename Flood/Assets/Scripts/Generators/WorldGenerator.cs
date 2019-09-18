using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGenerator
{
	private static GameObject worldPrefab = Resources.Load("Prefabs/World") as GameObject;


	public static World Create(int size) {
		Debug.Log("WorldGenerator Create");
		World outWorld = GameObject.Instantiate(worldPrefab).GetComponent<World>();
		CreateWorld(outWorld, size);

		return outWorld;
	}

	private static void CreateWorld(World world, int size) {
		Mesh mesh = new Mesh();
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

		int res = size + 1;

		// Creating vertices.
		float[,] grid = new float[res, res];

		for (int ix = 0; ix < res; ix++) {
			for (int iz = 0; iz < res; iz++) {
				int height = 0;
				grid[ix, iz] = height;
			}
		}

		// Compiling triangles
		mesh.vertices = MeshHelper.GridToVerts(size, grid);
		mesh.triangles = MeshHelper.GridTriCoords(res);

		// TODO: Consider vertex doubling.
		mesh.RecalculateNormals();

		world.Load(size, grid, mesh);
	}

	/*
	private static bool ShouldCutStraight(Vector3[,] verts, int x, int z) {
		return ((verts[x, z].y + verts[x + 1, z + 1].y) >= (verts[x, z + 1].y + verts[x + 1, z + 1].y));
	}
	*/

}
