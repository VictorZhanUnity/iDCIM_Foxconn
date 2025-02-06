using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    public string pipeTag = "Pipe"; // 假設水管物件都有 "Pipe" Tag

    public GameObject startPipe, endPipe;

    public List<GameObject> result;
    [ContextMenu("FindPath")]
    public List<GameObject> FindPath()
    {
        List<GameObject> pipes = new List<GameObject>(GameObject.FindGameObjectsWithTag(pipeTag));
        Dictionary<GameObject, List<GameObject>> pipeGraph = BuildPipeGraph(pipes);
        result = BFS_FindPath(pipeGraph, startPipe, endPipe);
        return result;
    }

    private Dictionary<GameObject, List<GameObject>> BuildPipeGraph(List<GameObject> pipes)
    {
        Dictionary<GameObject, List<GameObject>> graph = new Dictionary<GameObject, List<GameObject>>();

        foreach (GameObject pipe in pipes)
        {
            graph[pipe] = new List<GameObject>();

            foreach (GameObject otherPipe in pipes)
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
        MeshRenderer mesh1 = pipe1.GetComponent<MeshRenderer>();
        MeshRenderer mesh2 = pipe2.GetComponent<MeshRenderer>();

        if (mesh1 == null || mesh2 == null) return false;

        Bounds bounds1 = mesh1.bounds;
        Bounds bounds2 = mesh2.bounds;

        return bounds1.Intersects(bounds2); // 檢查兩個水管是否相交
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
