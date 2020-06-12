using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap <T>
{
    private HeapNode<T> root = null;

    public bool isEmpty()
    {
        return root == null;
    }

    public HeapNode<T> addHeapNode(T node)
    {
        if (root == null)
        {
            root = new HeapNode<T>(node, null);
            return root;
        }
        else
        {
            Queue<HeapNode<T>> q = new Queue<HeapNode<T>>();
            q.Enqueue(root);
            HeapNode<T> curr = root;
            while (curr != null)
            {
                curr = q.Dequeue();
                HeapNode<T> l = curr.getLeft();
                if (l == null)
                {
                    HeapNode<T> heapNode = new HeapNode<T>(node, curr);
                    curr.setLeft(heapNode);
                    root = heapNode.sortParent();
                    return heapNode;
                }
                else
                {
                    q.Enqueue(l);
                }

                HeapNode<T> r = curr.getRight();
                if (r == null)
                {
                    HeapNode<T> heapNode = new HeapNode<T>(node, curr);
                    curr.setRight(heapNode);
                    root = heapNode.sortParent();
                    return heapNode;
                }
                else
                {
                    q.Enqueue(r);
                }
            }
            return curr;

        }

    }

    public T pop()
    {
        if (root == null)
        {
            return default(T);
        }

        Queue<HeapNode<T>> q = new Queue<HeapNode<T>>();
        HeapNode<T> curr = root;
        HeapNode<T> prev = null;
        while (curr != null)
        {
            HeapNode<T> l = curr.getLeft();
            q.Enqueue(l);

            HeapNode<T> r = curr.getRight();
            q.Enqueue(r);

            prev = curr;
            curr = q.Dequeue();
        }

        //case where root is the only one
        //handle it here to avoid null pointers below
        if (prev.Equals(root))
        {
            root = null;
            return prev.getNode();
        }

        //set last element in tree to root node
        //set roots left and right to new root left and right
        prev.setLeft(root.getLeft());
        prev.setRight(root.getRight());

        //set new root parents to this
        if (prev.getLeft() != null) {
            prev.getLeft().setParent(prev);
        }
        if (prev.getRight() != null)
        {
            prev.getRight().setParent(prev);
        }

        //set prev's old parent's left or right branch to null
        if(prev.getParent().getLeft().Equals(prev))
        {
            prev.getParent().setLeft(null);
        }
        else if (prev.getParent().getRight().Equals(prev))
        {
            prev.getParent().setRight(null);
        }

        //set prev parent to null since it will be the root
        prev.setParent(null);

        HeapNode<T> popVal = root;
        root = prev;

        //place prev in its correct place
        //root.sort();
        root = root.sortChild();

        //completely disconnect popped value from tree
        popVal.setParent(null);
        popVal.setLeft(null);
        popVal.setRight(null);

        return popVal.getNode();
    }

    public void setRoot(HeapNode<T> n)
    {
        this.root = n;
    }

}

public class HeapNode <T>
{
    HeapNode<T> left;
    HeapNode<T> right;
    HeapNode<T> parent;

    T node;
    IComparable<T> nodeComparer;

    public HeapNode(T node, HeapNode<T> parent)
    {
        if (!(node is IComparable<T>))
        {
            throw new System.ArgumentException("Node must implement IComparable interface");
        }

        this.parent = parent;
        right = null;
        left = null;

        this.node = node;
        this.nodeComparer = (IComparable<T>)node;
    }

    public T getNode()
    {
        return this.node;
    }

    /*
     * The way this works is TOP->DOWN ONLY, is to be used when changing the value of the rooot node. Will place
     * the root node in its correct spot, cannot be used to arbitrarily sort all values. 
     */
    public HeapNode<T> sortChild()
    {
        //there is nothing below the current node
        //also handles root node case
        if (left == null && right == null)
        {
            return this;
        }
        //case where only left exists or left is smaller than right
        else if (right == null || left.nodeComparer.CompareTo(right.node) < 0)
        {
            //current node is larger than left, return and break recursion
            if (nodeComparer.CompareTo(left.node) < 0)
            {
                return this;
            }

            HeapNode<T> childLeft = left.left;
            HeapNode<T> childRight = left.right;

            HeapNode<T> newParent = this.left;

            //set left
            newParent.parent = this.parent;
            newParent.left = this;
            newParent.right = this.right;

            if(newParent.right != null)
            {
                newParent.right.parent = newParent;
            }

            //set curr
            this.left = childLeft;
            this.right = childRight;
            this.parent = newParent;

            if (this.left != null)
            {
                left.parent = this;
            }
            if (this.right != null)
            {
                right.parent = this;
            }

            //continue sorting down the current node
            //set return value to left.left to link previous child to this node's parent
            newParent.left = this.sortChild();
            return newParent;
        }
        //case where right is less than left
        else
        {
            //current node is smaller than right
            if (nodeComparer.CompareTo(right.node) < 0)
            {
                return this;
            }

            HeapNode<T> childLeft = right.left;
            HeapNode<T> childRight = right.right;

            HeapNode<T> newParent = this.right;

            //set left
            newParent.parent = this.parent;
            newParent.left = this.left;
            newParent.right = this;

            newParent.left.parent = newParent;

            //set curr
            this.left = childLeft;
            this.right = childRight;
            this.parent = newParent;

            if (this.left != null)
            {
                left.parent = this;
            }
            if (this.right != null)
            {
                right.parent = this;
            }

            //continue sorting down the current node
            //set return value to left.left to link previous child to this node's parent
            newParent.right = this.sortChild();
            return newParent;
        }
    }

    /*
     * Checks the value of the parent. to be called when inserting a new value into the heap. Checks nodes above itself ONLY.
     * GOES BOTTOM->UP
     */
    public HeapNode<T> sortParent()
    {
        //this is the root case
        if (this.parent == null)
        {
            return this;
        }

        if (parent.nodeComparer.CompareTo(node) > 0)
        {
            HeapNode<T> tempParent = parent.parent;
            HeapNode<T> tempLeft = parent.left;
            HeapNode<T> tempRight = parent.right;

            parent.left = this.left;
            parent.right = this.right;
            parent.parent = this;

            if (tempLeft != null && this.Equals(tempLeft))
            {
                this.left = parent;
                this.right = tempRight;
                this.parent = tempParent;

                if (this.right != null)
                {
                    this.right.parent = this;
                }

                //set new nodes below old parent such that the old parent is the lower nodes
                //new parent
                if (this.left.left != null)
                {
                    this.left.left.parent = this.left;
                }
                if (this.left.right != null)
                {
                    this.left.right.parent = this.left;
                }

                //sets the parent's parent left/right nodes to the new node
                if (parent != null)
                {
                    if (parent.left.Equals(this.left))
                    {
                        parent.left = this;
                    }
                    else if (parent.right.Equals(this.left))
                    {
                        parent.right = this;
                    }
                }
            }
            else if (tempRight != null && this.Equals(tempRight))
            {
                this.left = tempLeft;
                this.right = parent;
                this.parent = tempParent;

                if (this.left != null)
                {
                    this.left.parent = this;
                }

                //set new nodes below old parent such that the old parent is the lower nodes
                //new parent
                if (this.right.left != null)
                {
                    this.right.left.parent = this.right;
                }
                if (this.right.right != null)
                {
                    this.right.right.parent = this.right;
                }

                //sets the parent's parent left/right nodes to the new node
                if (parent != null) 
                {
                    if (parent.left.Equals(this.right))
                    {
                        parent.left = this;
                    }
                    else if (parent.right.Equals(this.right))
                    {
                        parent.right = this;
                    }
                }
            }

            return this.sortParent();
        }
        else
        {
            return parent.sortParent();
        }
    }

    public HeapNode<T> getLeft()
    {
        return this.left;
    }

    public void setLeft(HeapNode<T> n)
    {
        this.left = n;
    }

    public HeapNode<T> getRight()
    {
        return this.right;
    }

    public void setRight(HeapNode<T> n)
    {
        this.right = n;
    }

    public HeapNode<T> getParent()
    {
        return this.parent;
    }

    public void setParent(HeapNode<T> n)
    {
        this.parent = n;
    }

}
