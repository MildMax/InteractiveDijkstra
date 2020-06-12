using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    private GameObject node;
    private GameObject line;
    private int xstart;
    private int xend;
    private int ystart;
    private int yend;

    public List<NodeScript> nodeList;
    public List<GameObject> lineList;

    public Drawer(GameObject node, GameObject line, int xstart, int xend, int ystart, int yend)
    {
        this.node = node;
        this.line = line;
        this.xstart = xstart;
        this.xend = xend;
        this.ystart = ystart;
        this.yend = yend;

        nodeList = new List<NodeScript>();
        lineList = new List<GameObject>();

        drawNodes();
    }

    public void clearNodes()
    {
        foreach (NodeScript n in nodeList)
        {
            SpriteRenderer r = n.parent.GetComponent<SpriteRenderer>();
            r.color = Color.blue;
        }
    }

    public void drawNodes()
    {
        for (int i = xstart; i < xend + 1; ++i)
        {
            for (int j = ystart; j < yend + 1; ++j)
            {
                Vector3 pos = new Vector3(i, 0, j);
                GameObject n = Instantiate(node, pos, Quaternion.Euler(90f, 0f, 0f));
                NodeScript ns = new NodeScript(n, pos);
                nodeList.Add(ns);
                NodeScript temp = n.GetComponent<NodeScript>();
                temp.nodeScript = ns;
                //Debug.Log("writing node...");
            }
        }
    }

    public void drawLines()
    {
        foreach (GameObject l in lineList)
        {
            Destroy(l);
        }
        foreach (NodeScript n in nodeList)
        {
            n.resetNode();
            n.neighborList.Clear();
        }
        //draw vertical
        for (int i = xstart; i < xend + 1; ++i)
        {
            for (int j = ystart; j < yend; ++j)
            {
                int val = (int)(Random.value * 10000);
                if (val % 4 != 0) {
                    NodeScript node1 = nodeList.Find(a => a.pos == new Vector3(i, 0, j));
                    NodeScript node2 = nodeList.Find(a => a.pos == new Vector3(i, 0, j + 1));
                    node1.neighborList.Add(node2);
                    node2.neighborList.Add(node1);

                    GameObject li = Instantiate(line);
                    lineList.Add(li);
                    LineRenderer r = li.GetComponent<LineRenderer>();
                    Vector3[] rA = { node1.pos, node2.pos };
                    r.SetPositions(rA);
                }
            }
        }

        //draw horizontal
        for (int i = ystart; i < yend + 1; ++i)
        {
            for (int j = xstart; j < xend; ++j)
            {
                int val = (int)(Random.value * 10000);
                if (val % 4 != 0)
                {
                    NodeScript node1 = nodeList.Find(a => a.pos == new Vector3(j, 0, i));
                    NodeScript node2 = nodeList.Find(a => a.pos == new Vector3(j + 1, 0, i));
                    node1.neighborList.Add(node2);
                    node2.neighborList.Add(node1);

                    GameObject li = Instantiate(line);
                    lineList.Add(li);
                    LineRenderer r = li.GetComponent<LineRenderer>();
                    Vector3[] rA = { node1.pos, node2.pos };
                    r.SetPositions(rA);
                }
            }
        }
    }
}
