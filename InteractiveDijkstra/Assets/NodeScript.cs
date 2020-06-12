using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour, IComparable<NodeScript> {

    public GameObject parent;
    public NodeScript nodeScript;
    public List<NodeScript> neighborList;
    public List<NodeScript> pathList;
    public Vector3 pos;

    public bool isStart = false;
    public bool isEnd = false;

    private int nodeID;
    public float cost;
    public bool visited = false;

    public HeapNode<NodeScript> heapNode = null;

    public NodeScript(GameObject parent, Vector3 pos) 
    {
        this.parent = parent;
        this.pos = pos;
        neighborList = new List<NodeScript>();
        pathList = new List<NodeScript>();
        cost = float.MaxValue;
    }

    public void setID(int id)
    {
        this.nodeID = id;
    }

    public int getID()
    {
        return this.nodeID;
    }

    public void setCost(float cost)
    {
        this.cost = cost;
    }

    public float getCost()
    {
        return this.cost;
    }

    public List<NodeScript> getPathList()
    {
        List<NodeScript> pList = new List<NodeScript>();
        foreach (NodeScript n in pathList)
        {
            pList.Add(n);
        }
        return pList;
    }

    public void setHeapNode(HeapNode<NodeScript> heapNode)
    {
        this.heapNode = heapNode;
    }

    public HeapNode<NodeScript> getHeapNode()
    {
        return this.heapNode;
    }

    public void resetNode()
    {
        pathList.Clear();
        visited = false;
        cost = float.MaxValue;
    }

    public int CompareTo(NodeScript other)
    {
        if (this.cost < other.cost)
        {
            return -1;
        }
        else if (this.cost > other.cost)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
