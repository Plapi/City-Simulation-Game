using System.Collections.Generic;

public class BFS<T> where T : BFSNode {

	public static int[][] GetAdjacents(List<T> nodes) {
		int[][] adjacents = new int[nodes.Count][];
		for (int i = 0; i < nodes.Count; i++) {
			nodes[i].Index = i;
		}
		for (int i = 0; i < nodes.Count; i++) {
			BFSNode[] nextNodes = nodes[i].GetNextNodes();
			adjacents[i] = new int[nextNodes.Length];
			for (int j = 0; j < nextNodes.Length; j++) {
				adjacents[i][j] = nextNodes[j].Index;
			}
		}
		return adjacents;
	}

	public static bool FindPath(List<T> nodes, T startNode, T endNode, out List<T> path) {
		return FindPath(nodes, GetAdjacents(nodes), startNode, endNode, out path);
	}

	public static bool FindPath(List<T> nodes, int[][] adjacents, T startNode, T endNode, out List<T> path) {
		path = new List<T>();
		int length = adjacents.Length;
		int[] pred = new int[length];
		int[] dist = new int[length];

		int s = startNode.Index;
		int dest = endNode.Index;

		if (BFSAlgorithm(adjacents, s, dest, length, pred, dist) == false) {
			return false;
		}

		int crawl = dest;
		path.Add(nodes[crawl]);
		while (pred[crawl] != -1) {
			path.Add(nodes[pred[crawl]]);
			crawl = pred[crawl];
		}
		path.Reverse();
		return true;
	}

	private static bool BFSAlgorithm(int[][] adj, int src, int dest, int v, int[] pred, int[] dist) {
		if (src == -1 || dest == -1) {
			return false;
		}

		Queue<int> queue = new Queue<int>();
		bool[] visited = new bool[v];

		for (int i = 0; i < v; i++) {
			visited[i] = false;
			dist[i] = int.MaxValue;
			pred[i] = -1;
		}

		visited[src] = true;
		dist[src] = 0;
		queue.Enqueue(src);

		while (queue.Count != 0) {
			int u = queue.Dequeue();
			for (int i = 0; i < adj[u].Length; i++) {
				if (visited[adj[u][i]] == false) {
					visited[adj[u][i]] = true;
					dist[adj[u][i]] = dist[u] + 1;
					pred[adj[u][i]] = u;
					queue.Enqueue(adj[u][i]);

					if (adj[u][i] == dest) {
						return true;
					}
				}
			}
		}

		return false;
	}
}

public interface BFSNode {
	BFSNode[] GetNextNodes();
	int Index { get; set; }
}
