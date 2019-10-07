using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidGenerator
{
	private static GameObject fluidPrefab = Resources.Load("Prefabs/Fluid") as GameObject;

	public static Fluid Create(int sideLength) {
		Debug.Log("FluidGenerator Create");
		Fluid outFluid = GameObject.Instantiate(fluidPrefab).GetComponent<Fluid>();

		Mesh fluidMesh = new Mesh();
		fluidMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

		Vector2 fluidSize = new Vector2(2 * sideLength, 2 * HexHelper.THETA * sideLength);

		int gridX = 2 * sideLength + 1; // TODO: redundant
		int gridY = 2 * sideLength + 1;

		float[,] fluidGrid = new float[gridX, gridY];
		HexHelper.FillHexGrid(fluidGrid, sideLength);

		


		//grid[5, 5] = 1;

		// Compiling triangles
		int[,] iGrid = new int[gridX, gridY];
		fluidMesh.vertices = MeshHelper.HexGridToVerts(fluidSize, fluidGrid, iGrid);
		fluidMesh.triangles = MeshHelper.HexGridToTris(iGrid);

		Vector2[] uvs = new Vector2[fluidMesh.vertexCount];
		for (int ix = 0; ix < gridX; ix++) {
			for (int iy = 0; iy < gridY; iy++) {
				int v = iGrid[ix, iy];
				if (v != -1) uvs[v] = new Vector2(ix, iy);
			}
		}

		fluidMesh.uv = uvs;

		//fluidMesh.RecalculateNormals();
		
		float total = 0;
		for (int ix = 0; ix < gridX; ix++) {
			for (int iy = 0; iy < gridY; iy++) {
				if (fluidGrid[ix, iy] < 0) {
					continue;
				}
				var dist = (new Vector2(gridX / 2f, gridY / 2f * HexHelper.THETA) - new Vector2(ix, iy * HexHelper.THETA)).magnitude;
				if (dist < gridX / 6f) {
					fluidGrid[ix, iy] = 40;
					total += 20;
				} else {
					fluidGrid[ix, iy] = 20;
					total += 5;
				}
			}
		}

		Debug.Log("TOTAL: " + total);
		

		//fluidGrid[1,1] = 5;

		RenderTexture fluidTexture = HexHelper.HexTexture(fluidGrid);
		Debug.Log(fluidTexture.wrapMode + " " + fluidTexture.filterMode);
		outFluid.Load(fluidSize, fluidGrid, fluidMesh, fluidTexture);

		// TODO: Determine best resolution


		return outFluid;
	}
}
