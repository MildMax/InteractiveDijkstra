using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject node;
    public GameObject line;

    public int xStart;
    public int xEnd;
    public int yStart;
    public int yEnd;

    private Dijkstra dijkstra;
    private Drawer drawer;

    private void Awake()
    {
        drawer = new Drawer(node, line, xStart, xEnd, yStart, yEnd);
    }

    public void drawLines()
    {
        drawer.clearNodes();
        drawer.drawLines();
    }

    public void runDijkstra()
    {
        drawer.clearNodes();
        Dijkstra d = new Dijkstra(drawer.nodeList);
        d.resetNodes();
        d.Run();
    }
}
