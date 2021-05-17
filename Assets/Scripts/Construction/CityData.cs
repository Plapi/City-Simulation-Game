using System;
using UnityEngine;
using System.Collections.Generic;

public class CityData {

	private const string CITY_DATA_KEY = "CITY_DATA";

	private static CityData instance;

	public static CityData Instance {
		get {
			if (instance == null) {
				instance = JsonUtility.FromJson<CityData>(PlayerPrefs.GetString(CITY_DATA_KEY, "{}"));
			}
			return instance;
		}
	}

	[SerializeField] private List<ConstructionData> constructions = new List<ConstructionData>();

	public List<ConstructionData> Constructions => constructions;

	public void AddConstruction(int id, int x, int z) {
		constructions.Add(new ConstructionData(id, x, z));
	}

	public void Save() {
		PlayerPrefs.SetString(CITY_DATA_KEY, JsonUtility.ToJson(this));
		PlayerPrefs.Save();
	}

	public static void Delete() {
		PlayerPrefs.DeleteKey(CITY_DATA_KEY);
		PlayerPrefs.Save();
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
