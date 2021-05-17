using System.Collections.Generic;
using UnityEngine;

public class NavigationPoint {

	public Vector3 point;

	public bool hasCar = false;

	public List<NavigationPoint> priorityPoints = new List<NavigationPoint>();
}
