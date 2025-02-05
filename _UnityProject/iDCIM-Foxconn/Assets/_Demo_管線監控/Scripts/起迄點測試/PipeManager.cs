using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    public List<GameObject> pipeSegments; // 存放所有水管段

    public List<GameObject> FindPath(GameObject startPipe, GameObject endPipe)
    {
        Dictionary<GameObject, List<GameObject>> pipeGraph = BuildPipeGraph();
        return BFS_FindPath(pipeGraph, startPipe, endPipe);
    }

    private Dictionary<GameObject, List<GameObject>> BuildPipeGraph()
    {
        Dictionary<GameObject, List<GameObject>> graph = new Dictionary<GameObject, List<GameObject>>();

        foreach (GameObject pipe in pipeSegments)
        {
            graph[pipe] = new List<GameObject>();

            foreach (GameObject otherPipe in pipeSegments)
            {
                if (pipe == otherPipe) continue;
                if (IsAdjacent(pipe, otherPipe))
                {
                    graph[pipe].Add(otherPipe);
                }
            }
        }

        return graph;
    }

    private bool IsAdjacent(GameObject pipe1, GameObject pipe2)
    {
        float distance = Vector3.Distance(pipe1.transform.position, pipe2.transform.position);
        return distance < 0.1f; // 允許一些誤差
    }

    private List<GameObject> BFS_FindPath(Dictionary<GameObject, List<GameObject>> graph, GameObject start, GameObject end)
    {
        Queue<List<GameObject>> queue = new Queue<List<GameObject>>();
        HashSet<GameObject> visited = new HashSet<GameObject>();

        queue.Enqueue(new List<GameObject> { start });
        visited.Add(start);

        while (queue.Count > 0)
        {
            List<GameObject> path = queue.Dequeue();
            GameObject lastPipe = path[path.Count - 1];

            if (lastPipe == end) return path;

            foreach (GameObject neighbor in graph[lastPipe])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    List<GameObject> newPath = new List<GameObject>(path) { neighbor };
                    queue.Enqueue(newPath);
                }
            }
        }

        return new List<GameObject>(); // 無法找到路徑
    }
}
