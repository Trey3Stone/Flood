using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexHelper
{
	public const float THETA = 0.866025403784438646763723170752936183471402626905190314027f; // sqrt(3)/2

	// Odd-row offset grid



	public static RenderTexture HexTexture(HexGrid<float> hexGrid) {
		// TODO: Ensure that hexTexture.Release() is called.
		// NOTE: If fluidTexture is higher res than terrainTexture, interpolation will be used for volume determination

		int gridSize = hexGrid.Size;

		RenderTexture outHex = new RenderTexture(gridSize, gridSize, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
		outHex.enableRandomWrite = true;
		outHex.filterMode = FilterMode.Point;
		outHex.wrapMode = TextureWrapMode.Clamp;
		
		Texture2D blitTex = new Texture2D(gridSize, gridSize, TextureFormat.RFloat, false, true);

		var texData = blitTex.GetRawTextureData<float>();
		
		int i = 0;
		for (int ix = 0; ix < gridSize; ix++) {
			for (int iy = 0; iy < gridSize; iy++) {
				texData[i++] = hexGrid[ix, iy];
			}
		}

		blitTex.Apply(); // TODO: Is this needed?
		//outHex.Create();
		Graphics.Blit(blitTex, outHex);

		return outHex;
	}

	public static HexGrid<byte> HexMask(HexGrid<float> hexGrid) {
		int gridSize = hexGrid.Size;
		HexGrid<byte> mask = new HexGrid<byte>(gridSize);

		for (int ix = 0; ix < gridSize; ix++) {
			for (int iy = 0; iy < gridSize; iy++) {
				bool isFull = hexGrid[ix, iy] >= 0;
				int maskVal = isFull ? 1 : 0;
				for (int n = 1; n <= 6; n++) {
					if (hexGrid.HasNeighbor(ix, iy, n) && hexGrid.GetNeighbor(ix, iy, n) >= 0) {
						maskVal |= 1 << n;
					} else {
						isFull = false;
					}
				}

				if (isFull)
					maskVal |= 1 << 7;

				mask[ix, iy] = (byte)maskVal;
				
			}
		}

		return mask;
	}

	public static RenderTexture HexMaskTexture(HexGrid<byte> hexGrid) {
		int gridSize = hexGrid.Size;

		RenderTexture outMask = new RenderTexture(gridSize, gridSize, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
		outMask.enableRandomWrite = true;
		outMask.filterMode = FilterMode.Point;
		outMask.wrapMode = TextureWrapMode.Clamp;

		Texture2D blitTex = new Texture2D(gridSize, gridSize, TextureFormat.RFloat, false, true);

		var texData = blitTex.GetRawTextureData<float>();

		//Debug.Log((hexGrid.Size * hexGrid.Size) + " !!! " + texData.Length);

		int i = 0;
		for (int ix = 0; ix < gridSize; ix++) {
			for (int iy = 0; iy < gridSize; iy++) {

				//texData[i++] = ((hexGrid[ix, iy] < 0) || (height > 0)) ? 0f : 1f;
				texData[i++] = hexGrid[ix, iy];
				//Debug.Log(new Vector2Int(ix, iy) + " = " + System.Convert.ToString(texData[i-1], 2));
			}
		}
		

		blitTex.Apply(); // TODO: Is this needed?
		//GL.sRGBWrite = false;
		Graphics.Blit(blitTex, outMask);
		//outMask.Create();

		/*
		RenderTexture.active = outMask;
		Texture2D tex = new Texture2D(gridSize, gridSize, TextureFormat.ARGB32, false, true);
		tex.ReadPixels(new Rect(0, 0, gridSize, gridSize), 0, 0);
		tex.Apply();
		var texData2 = tex.GetRawTextureData<int>();

		int i2 = 0;
		for (int ix = 0; ix < gridSize; ix++) {
			for (int iy = 0; iy < gridSize; iy++) {
				Debug.Log(new Vector2Int(ix, iy) + " = " + System.Convert.ToString(texData2[i2++], 2));
			}
		}

		RenderTexture.active = null;
		*/
		return outMask;
	}

	public static void FillHexGrid(HexGrid<float> hexGrid, int sideLength) {
		int gridSize = hexGrid.Size;

		int xMin, xSize;
		xMin = (sideLength - 1) / 2;
		for (int iy = 0; iy < gridSize; iy++) {
			xSize = gridSize - Mathf.Abs(iy - sideLength);
			for (int ix = 0; ix < gridSize; ix++) {
				if (ix < xMin || ix >= xMin + xSize) {
					hexGrid[ix, iy] = -1;
				} // Default value is zero already
			}

			//int sign = (int)Mathf.Sign(iy - sideLength);
			if (iy < sideLength) {
				xMin -= (iy & 1);
			} else if (iy > sideLength) {
				xMin += 1 - (iy & 1);
			}
			

		}



	}

	public static Vector2Int WorldToHex(Vector3 pos, Vector2 worldSize) {
		return WorldToHex(new Vector2(pos.x, pos.z), worldSize);
	}

	public static Vector2Int WorldToHex(Vector2 pos, Vector2 worldSize) {

		Vector2 normPos = new Vector2(pos.x, pos.y) + worldSize / 2;

		int row = Mathf.RoundToInt(normPos.y / HexHelper.THETA);

		int col = Mathf.RoundToInt(normPos.x + 0.5f * (1 - (row % 2)));

		return new Vector2Int(col, row);
	}

	public static Vector3 HexToWorld(Vector2Int pos, World world) {
		//Vector3 pos = MeshHelper.HexToWorld(2 * SIZE + 1, WorldManager.Self.World.Size, gridPos.x, gridPos.y);
		//pos.y = WorldManager.Self.World.HeightGrid[gridPos];

		return new Vector3(
			world.Size.x * ((pos.x + 0.5f * (1 - (pos.y & 1))) / (world.GridSize - 1.0f) - 0.5f),
			world.HeightGrid[pos],
			world.Size.y * (pos.y / (world.GridSize - 1.0f) - 0.5f)
		);
	}

}
