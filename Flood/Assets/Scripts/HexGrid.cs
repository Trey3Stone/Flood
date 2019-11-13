using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid<T>
{
	// Even-row offset hexagonal grid

	// int3(x, y, useOddOffset)
	static readonly Vector3Int[] NEIGHBOR_OFFSETS = {
	new Vector3Int(  0,  0,  0),
	new Vector3Int(  1,  0,  0),
	new Vector3Int(  1, -1,  1),
	new Vector3Int(  0, -1,  1),
	new Vector3Int( -1,  0,  0),
	new Vector3Int(  0,  1,  1),
	new Vector3Int(  1,  1,  1)
};


	private T[,] grid;

	public int Size { get; set; } // Side length


	public HexGrid(int size){
		Size = size;

		grid = new T[Size, Size];
	}

	public T this[int x, int y] {
		get { return grid[x, y]; }
		set { grid[x, y] = value; }
	}

	public T this[Vector2Int v] {
		get { return this[v.x, v.y]; }
		set { this[v.x, v.y] = value; }
	}

	public T GetNeighbor(int x, int y, int n) {
		Vector2Int nVec = NeighborCoords(x, y, n);
		return this[nVec.x, nVec.y];
	}

	public bool HasNeighbor(int x, int y, int n) {
		Vector2Int nVec = NeighborCoords(x, y, n);
		return (nVec.x >= 0 && nVec.x < Size) && (nVec.y >= 0 && nVec.y < Size);
	}

	private Vector2Int NeighborCoords(int x, int y, int n) {
		Vector3Int nOffset = NEIGHBOR_OFFSETS[n];
		int oddOffset = -(y & 1);

		return new Vector2Int(x + nOffset.x + nOffset.z * oddOffset, y + nOffset.y);
	}

}

/*

    5---------6
   /           \
  /             \
 /               \
4        0        1
 \               /
  \             /
   \           /
    3---------2

*/
