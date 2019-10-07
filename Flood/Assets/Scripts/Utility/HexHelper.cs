using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexHelper
{
	public const float THETA = 0.866025403784438646763723170752936183471402626905190314027f; // sqrt(3)/2

	// Odd-row offset grid



	public static RenderTexture HexTexture(float[,] hexGrid) {
		// TODO: Ensure that hexTexture.Release() is called.
		// NOTE: If fluidTexture is higher res than terrainTexture, interpolation will be used for volume determination

		int width = hexGrid.GetLength(0);
		int height = hexGrid.GetLength(1);

		RenderTexture outHex = new RenderTexture(width, height, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
		outHex.enableRandomWrite = true;
		outHex.filterMode = FilterMode.Point;
		outHex.wrapMode = TextureWrapMode.Clamp;
		
		Texture2D blitTex = new Texture2D(width, height, TextureFormat.RFloat, false, true);

		var texData = blitTex.GetRawTextureData<float>();
		
		int i = 0;
		for (int ix = 0; ix < width; ix++) {
			for (int iy = 0; iy < height; iy++) {
				texData[i++] = hexGrid[ix, iy];
			}
		}

		blitTex.Apply(); // TODO: Is this needed?
		//outHex.Create();
		Graphics.Blit(blitTex, outHex);

		return outHex;
	}

	public static RenderTexture HexMask(float[,] hexGrid) {
		int size = hexGrid.GetLength(0);

		RenderTexture outMask = new RenderTexture(size, size, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
		outMask.enableRandomWrite = true;
		outMask.filterMode = FilterMode.Point;
		outMask.wrapMode = TextureWrapMode.Clamp;

		Texture2D blitTex = new Texture2D(size, size, TextureFormat.RFloat, false, true);

		var texData = blitTex.GetRawTextureData<float>();

		int i = 0;
		for (int ix = 0; ix < size; ix++) {
			for (int iy = 0; iy < size; iy++) {
				texData[i++] = hexGrid[ix, iy] < 0 ? 0f : 1f;
			}
		}
		

		blitTex.Apply(); // TODO: Is this needed?

		Graphics.Blit(blitTex, outMask);

		return outMask;
	}

	public static void FillHexGrid(float[,] hexGrid, int sideLength) {
		int gridSize = hexGrid.GetLength(0);

		int xMin, xSize;
		xMin = (sideLength - 1) / 2;
		for (int iy = 0; iy < gridSize; iy++) {
			xSize = gridSize - Mathf.Abs(iy - sideLength);
			for (int ix = 0; ix < gridSize; ix++) {
				if (ix < xMin || ix >= xMin + xSize) {
					hexGrid[ix, iy] = -1;
				} else {
					//hexGrid[ix, iy] = 1;
				}
			}

			//int sign = (int)Mathf.Sign(iy - sideLength);
			if (iy < sideLength) {
				xMin -= (iy & 1);
			} else if (iy > sideLength) {
				xMin += 1 - (iy & 1);
			}
			

		}



	}

	//public static bool 


}
