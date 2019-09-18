using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidGenerator
{
	private static GameObject fluidPrefab = Resources.Load("Prefabs/Fluid") as GameObject;

	public static Fluid Create(int size) {
		Debug.Log("FluidGenerator Create");
		Fluid outFluid = GameObject.Instantiate(fluidPrefab).GetComponent<Fluid>();

		Mesh fluidMesh = new Mesh();
		fluidMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

		float[,] grid = new float[size + 1, size + 1];
		//grid[5, 5] = 1;

		fluidMesh.vertices = MeshHelper.GridToVerts(size, grid);
		fluidMesh.triangles = MeshHelper.GridTriCoords(size + 1);

		Vector2[] uvs = new Vector2[fluidMesh.vertexCount];
		for (int i = 0; i < uvs.Length; i++) {
			uvs[i] = new Vector2(i, 0);
		}

		fluidMesh.uv = uvs;

		fluidMesh.RecalculateNormals();

		outFluid.Load(size, grid, fluidMesh);

		// TODO: Determine best resolution


		return outFluid;
	}
}
