using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshHelper
{


	public static int[] GridTriCoords(int res) {
		// Creating triangles.
		List<int> tris = new List<int>();

		for (int ix = 0; ix < res - 1; ix++) {
			for (int iz = 0; iz < res - 1; iz++) {
				// Adding triangle 1.
				tris.Add(FlattenCoords(res, ix, iz));
				tris.Add(FlattenCoords(res, ix, iz + 1));
				tris.Add(FlattenCoords(res, ix + 1, iz + 1));

				// Adding triangle 2.
				tris.Add(FlattenCoords(res, ix, iz));
				tris.Add(FlattenCoords(res, ix + 1, iz + 1));
				tris.Add(FlattenCoords(res, ix + 1, iz));
			}
		}

		return tris.ToArray();
	}

	public static Vector3[] GridToVerts(int size, float[,] grid) {
		Vector3[] outVerts = new Vector3[grid.GetLength(0) * grid.GetLength(1)];

		for (int ix = 0; ix < grid.GetLength(0); ix++) {
			for (int iz = 0; iz < grid.GetLength(1); iz++) {
				outVerts[FlattenCoords(grid.GetLength(0), ix, iz)] = ArrToWorld(size, ix, iz) + new Vector3(0, grid[ix, iz], 0);
			}
		}

		return outVerts;
	}

	public static Vector3 ArrToWorld(int size, int x, int z) {
		float halfSize = size / 2.0f;
		return new Vector3(x - halfSize, 0, z - halfSize);
	}

	public static int FlattenCoords(int rowSize, int x, int y) {
		return x + y * rowSize;
	}

	public static Vector2Int UnflattenCoords(int rowSize, int i) {
		return new Vector2Int(i % rowSize, i / rowSize);
	}
}
