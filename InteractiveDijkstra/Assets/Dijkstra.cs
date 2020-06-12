using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dijkstra
{
    private NodeScript start;
    private NodeScript end;

    private List<NodeScript> nodeScripts;

    public Dijkstra(List<NodeScript> nodeList)
    {
        this.nodeScripts = nodeList;
    }

    public void Run()
    {
        //resetNodes();
        tempSet();
        RunDijkstra();
        colorNodes();
        costText();
    }

    public void costText()
    {
        GameObject textGO = GameObject.FindGameObjectWithTag("CostText");
        Text text = textGO.GetComponent<Text>();
        if (end.cost == float.MaxValue)
        {
            text.text = "Cost: Infinity";
        }
        else
        {
            text.text = "Cost: " + end.cost;
        }
        
    }

    public void colorNodes()
    {
        foreach (NodeScript n in end.pathList)
        {
            n.parent.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        start.parent.GetComponent<SpriteRenderer>().color = Color.green;
        end.parent.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void resetNodes()
    {
        foreach (NodeScript n in nodeScripts)
        {
            n.resetNode();
        }
    }

    public void tempSet()
    {
        Random.InitState(System.DateTime.Now.Second);
        int a = Random.Range(0, nodeScripts.Count);
        start = nodeScripts[a];
        int b = a;
        while (b == a)
        {
            b = Random.Range(0, nodeScripts.Count);
        }
        end = nodeScripts[b];
    }

    
    public void RunDijkstra()
    {
        //test run time
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        start.setCost(0);

        //set heap and add elements from start to heap
        Heap<NodeScript> heap = new Heap<NodeScript>();
        foreach (NodeScript n in start.neighborList)
        {
            float cost = Mathf.Abs(start.pos.x - n.pos.x) + Mathf.Abs(start.pos.z - n.pos.z);
            n.setCost(cost);
            n.pathList.Add(start);
            n.setHeapNode(heap.addHeapNode(n));
            
        }

        while (!heap.isEmpty())
        {
            NodeScript currScript = heap.pop();
            currScript.heapNode = null;

            if (currScript.pos.Equals(end))
            {
                break;
            }

            currScript.visited = true;

            foreach (NodeScript n in currScript.neighborList)
            {

                if (n.visited == true)
                {
                    continue;
                }

                float cost = Mathf.Abs(currScript.pos.x - n.pos.x) 
                    + Mathf.Abs(currScript.pos.z - n.pos.z) + currScript.getCost();
                
                if (cost < n.getCost())
                {
                    n.setCost(cost);
                    n.pathList.Clear();
                    foreach (NodeScript t in currScript.pathList)
                    {
                        n.pathList.Add(t);
                    }
                    n.pathList.Add(currScript);
                    if (n.heapNode == null)
                    {
                        n.setHeapNode(heap.addHeapNode(n));
                    }
                    else
                    {
                        heap.setRoot(n.heapNode.sortParent());
                    }
                }
                
            }
        }

        stopwatch.Stop();
        Debug.Log("Time for Dijkstra's v1: " + stopwatch.ElapsedMilliseconds);

    }
   
}
