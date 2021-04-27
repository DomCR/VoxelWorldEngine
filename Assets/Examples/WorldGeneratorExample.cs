using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneratorExample : MonoBehaviour
{
	public VoxelWorldEngine.World World;

	void Start()
	{
		World.Initialize();

		World.BuildWorld();

		StartCoroutine(World.DrawWorld());
	}
}
