using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidGenerator
{
	private static GameObject fluidPrefab = Resources.Load("Prefabs/Fluid") as GameObject;

	public static Fluid Create(int sideLength) {
		Debug.Log("FluidGenerator Create");
		Fluid outFluid = GameObject.Instantiate(fluidPrefab).GetComponent<Fluid>();

		Vector2 fluidSize = new Vector2(2 * sideLength, 2 * HexHelper.THETA * sideLength);

		int gridSize = 2 * sideLength + 1;

		HexGrid<float> fluidGrid = new HexGrid<float>(gridSize);
		HexHelper.FillHexGrid(fluidGrid, sideLength);

		//fluidMesh.RecalculateNormals();
		/*
		float total = 0;
		for (int ix = 0; ix < gridSize; ix++) {
			for (int iy = 0; iy < gridSize; iy++) {
				if (fluidGrid[ix, iy] < 0) {
					continue;
				}
				var dist = (new Vector2(gridSize / 2f, gridSize / 2f * HexHelper.THETA) - new Vector2(ix, iy * HexHelper.THETA)).magnitude;

				int height = (iy < (gridSize / 6)) ? 10 : 0;
				fluidGrid[ix, iy] = height;
				total += height;
				
			}
		}

		Debug.Log("TOTAL: " + total);
		
	*/
		//fluidGrid[1,1] = 5;

		outFluid.Load(fluidSize, fluidGrid);

		// TODO: Determine best resolution


		return outFluid;
	}
}
