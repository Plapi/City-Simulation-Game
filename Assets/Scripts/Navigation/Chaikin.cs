using System.Collections.Generic;
using UnityEngine;

public static class Chaikin {

	public static List<Vector3> SmoothPath(List<Vector3> path, int passes) {
		List<Vector3> sPath = new List<Vector3>(path);
		for (int i = 0; i < passes; i++) {
			sPath = SmoothPath(sPath);
		}
		return sPath;
	}

	public static List<Vector3> SmoothPath(Vector3[] path, int passes) {
		List<Vector3> sPath = new List<Vector3>(path);
		for (int i = 0; i < passes; i++) {
			sPath = SmoothPath(sPath);
		}
		return sPath;
	}

	public static List<Vector3> SmoothPath(List<Vector3> path) {
		var output = new List<Vector3>();

		if (path.Count > 0) {
			output.Add(path[0]);
		}

		for (int i = 0; i < path.Count - 1; i++) {
			var p0 = path[i];
			var p1 = path[i + 1];
			var p0x = p0.x;
			var p0y = p0.z;
			var p1x = p1.x;
			var p1y = p1.z;

			var qx = 0.75f * p0x + 0.25f * p1x;
			var qy = 0.75f * p0y + 0.25f * p1y;
			var Q = new Vector3(qx, 0, qy);

			var rx = 0.25f * p0x + 0.75f * p1x;
			var ry = 0.25f * p0y + 0.75f * p1y;
			var R = new Vector3(rx, 0, ry);

			output.Add(Q);
			output.Add(R);
		}

		if (path.Count > 1) {
			output.Add(path[path.Count - 1]);
		}

		return output;
	}
}
