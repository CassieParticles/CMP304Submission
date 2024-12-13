using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public delegate void NodeDecisionFunction(object parameters);

    private NodeDecisionFunction function;
    private List<Node> childNodes;
    public Node(NodeDecisionFunction decisionFunction)
    {
        function = decisionFunction;
    }

    public void addChildNode(Node node)
    {
        childNodes.Add(node);
    }

    public void CallFunction(object parameters)
    {
        function(parameters);
    }
}
