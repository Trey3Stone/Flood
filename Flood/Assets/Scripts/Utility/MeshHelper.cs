using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshHelper
{

	public static Mesh HexGridToMesh(Vector2 worldSize, HexGrid<float> hexGrid) {
		int gridSize = hexGrid.Size;

		Mesh mesh = new Mesh();
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

		int[,] iGrid = new int[gridSize, gridSize];
		mesh.vertices = HexGridToVerts(worldSize, hexGrid, iGrid);
		mesh.triangles = HexGridToTris(iGrid);

		Vector2[] uvs = new Vector2[mesh.vertexCount];
		for (int ix = 0; ix < gridSize; ix++) {
			for (int iy = 0; iy < gridSize; iy++) {
				int v = iGrid[ix, iy];
				if (v != -1) uvs[v] = new Vector2(ix, iy);
			}
		}

		mesh.uv = uvs;

		return mesh;
	}

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

	public static int[] HexGridToTris(int[,] iGrid) {
		// Creating triangles.
		List<int> tris = new List<int>();

		int width = iGrid.GetLength(0);
		int height = iGrid.GetLength(1);

		for (int ix = 0; ix < width; ix++) {
			for (int iz = 0; iz < height - 1; iz++) {
				if (ix + 1 - (iz & 1) >= width) continue;
				int v0 = iGrid[ix, iz]; // FlattenCoords(res, ix, iz);
				int v2 = iGrid[ix + 1 - (iz & 1), iz + 1]; // FlattenCoords(res, ix + (iz & 1), iz + 1);

				if (v0 == -1 || v2 == -1) continue;

				// Adding triangle 1.
				if (ix < width - 1) {
					int v1 = iGrid[ix + 1, iz]; // FlattenCoords(res, ix + 1, iz);
					if (v1 != -1) {
						tris.Add(v0);
						tris.Add(v2);
						tris.Add(v1);
					}
				}

				// Adding triangle 2.
				if (ix >= (iz & 1)) {
					int v3 = iGrid[ix + 1 - (iz & 1) - 1, iz + 1]; // FlattenCoords(res, ix + (iz & 1) - 1, iz + 1);
					if (v3 != -1) {
						tris.Add(v0);
						tris.Add(v3);
						tris.Add(v2);
					}
				}
			}
		}

		return tris.ToArray();
	}

	// Modifies iGrid
	public static Vector3[] HexGridToVerts(Vector2 worldSize, HexGrid<float> hexGrid, int[,] iGrid) {
		int gridSize = hexGrid.Size;

		Vector3[] outVerts = new Vector3[gridSize * gridSize];
		//iGrid = new int[gridWidth, gridHeight];

		int i = 0;
		for (int ix = 0; ix < gridSize; ix++) {
			for (int iz = 0; iz < gridSize; iz++) {
				if (hexGrid[ix, iz] < 0) {
					iGrid[ix, iz] = -1;
				} else {
					Vector3 testVert = HexToWorld(gridSize, gridSize, worldSize, ix, iz) + new Vector3(0, hexGrid[ix, iz], 0);
					outVerts[i] = testVert;
					iGrid[ix, iz] = i;
					i++;
				}
			}
		}

		return outVerts;
	}

	public static Vector3 HexToWorld(int gridWidth, int gridHeight, Vector2 worldSize, int x, int z) {
		return new Vector3(worldSize.x * ((x + 0.5f * (1 - (z & 1))) / (gridWidth - 1.0f) - 0.5f), 0, worldSize.y * (z / (gridHeight - 1.0f) - 0.5f));
	}

	public static int FlattenCoords(int rowSize, int x, int y) {
		return x + y * rowSize;
	}

	public static Vector2Int UnflattenCoords(int rowSize, int i) {
		return new Vector2Int(i % rowSize, i / rowSize);
	}
}
