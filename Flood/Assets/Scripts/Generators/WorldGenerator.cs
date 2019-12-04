using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGenerator {
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


		//Mathf.PerlinNoise()

		const float MAX_AMPLITUDE = 12;

		for (int ix = 0; ix < gridSize; ix++) {
			for (int iy = 0; iy < gridSize; iy++) {
				if (heightGrid[ix, iy] < 0)
					continue;


				Vector3 pos = MeshHelper.HexToWorld(gridSize, worldSize, ix, iy);
				float newHeight = MAX_AMPLITUDE * (1 - (pos.magnitude / sideLength));
				if (newHeight < 0) Debug.Log("oops " + newHeight);
				heightGrid[ix, iy] = MAX_AMPLITUDE;// + newHeight;


			}
		}

		const int OCTAVES = 8;

		int xOff = Mathf.RoundToInt(Random.Range(0, 1000000f));
		int yOff = Mathf.RoundToInt(Random.Range(0, 1000000f));


		//Debug.Log(Mathf.PerlinNoise(2f, 2f));
		//Debug.Log(Mathf.PerlinNoise(2f, 14f));

		for (int io = 1; io < OCTAVES; io++) {
			for (int ix = 0; ix < gridSize; ix++) {
				for (int iy = 0; iy < gridSize; iy++) {
					if (heightGrid[ix, iy] < 0)
						continue;

					float wavelength = sideLength / Mathf.Pow(2, io);

					Vector3 pos = MeshHelper.HexToWorld(gridSize, worldSize, ix, iy);



					float xSeed = xOff + pos.x / wavelength;
					float ySeed = yOff + pos.z / wavelength;


					heightGrid[ix, iy] += 2 * (MAX_AMPLITUDE / Mathf.Pow(1.6f, io)) * ((PerlinFix(xSeed, ySeed) - 0.5f));


				}
			}

		}

		float total = 0;
		float count = 0;
		
		for (int ix = 0; ix < gridSize; ix++) {
			for (int iy = 0; iy < gridSize; iy++) {
				if (heightGrid[ix, iy] < 0)
					continue;

				const float SNAP = 1.0f;
				const float WEIGHT = 2.0f;

				heightGrid[ix, iy] = (heightGrid[ix, iy] + WEIGHT * SNAP * Mathf.Round(heightGrid[ix, iy] / SNAP)) / (1 + WEIGHT);

				total += heightGrid[ix, iy];
				count++;
			}
		}

		for (int ix = 0; ix < gridSize; ix++) {
			for (int iy = 0; iy < gridSize; iy++) {
				if (heightGrid[ix, iy] < 0)
					continue;

				Vector3 pos = MeshHelper.HexToWorld(gridSize, worldSize, ix, iy);

				

				heightGrid[ix, iy] = Mathf.Lerp(total / count, heightGrid[ix, iy], Mathf.Clamp01((pos.magnitude - 0.2f * GameManager.SIZE)/ 10) + 0.2f);
			}
		}




		//TestWalls(gridSize, heightGrid);

		world.Load(worldSize, heightGrid);
	}

	/*

		a---b
		|   |
		c---d


	*/

	private static float PerlinFix(float x, float y) {
		//return 0;
		int xInt = Mathf.FloorToInt(x);
		int yInt = Mathf.FloorToInt(y);

		float xFrac = x - xInt;
		float yFrac = y - yInt;

		float xMin = Mathf.PI * (Mathf.PI + xInt);
		float yMin = Mathf.PI * (Mathf.PI + yInt);

		float xMax = Mathf.PI * (Mathf.PI + xInt + 1);
		float yMax = Mathf.PI * (Mathf.PI + yInt + 1);

		float a = Mathf.PerlinNoise(xMin, yMin);
		float b = Mathf.PerlinNoise(xMax, yMin);
		float c = Mathf.PerlinNoise(xMin, yMax);
		float d = Mathf.PerlinNoise(xMax, yMax);

		return Mathf.SmoothStep(Mathf.SmoothStep(a, b, xFrac), Mathf.SmoothStep(c, d, xFrac), yFrac);
		//return Mathf.Lerp(Mathf.Lerp(a, b, xFrac), Mathf.Lerp(c, d, xFrac), yFrac);

	}

	public static void SpawnHives(World world) {
		int hivesLeft = (int) (Mathf.Pow(GameManager.SIZE, 2) / 200.0f);

		float rad = world.GridSize / 2 * HexHelper.THETA;


		while (hivesLeft > 0) {
			float ang = Random.Range(0.0f, 2*Mathf.PI);
			float dist = Random.Range(0.4f, 1.0f) * rad;

			Vector2 pos = dist * new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));

			WorldManager.Self.PlaceEnt(Resources.Load<GameObject>("Prefabs/Hive"), HexHelper.WorldToHex(pos, world.Size), 0.8f);
			hivesLeft--;
		}


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
