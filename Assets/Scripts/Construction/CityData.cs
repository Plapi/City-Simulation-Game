using System;
using UnityEngine;
using System.Collections.Generic;

public class CityData : ScriptableObjectSingleton<CityData> {

	[SerializeField] private List<ConstructionData> constructions = new List<ConstructionData>();

	public List<ConstructionData> Constructions => constructions;

	public void AddConstruction(int id, int x, int z) {
		constructions.Add(new ConstructionData(id, x, z));
	}

	[Serializable]
	public class ConstructionData {

		[SerializeField] private int id;
		[SerializeField] private int x;
		[SerializeField] private int z;

		public ConstructionData(int id, int x, int z) {
			this.id = id;
			this.x = x;
			this.z = z;
		}

		public int Id => id;
		public int X => x;
		public int Z => z;
	}
}
