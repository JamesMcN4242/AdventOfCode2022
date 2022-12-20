Console.WriteLine($"Part One: {RunSimulation()}");
Console.WriteLine($"Part Two: {RunSimulation(10, 811589153)}");

long RunSimulation(int iterations = 1, long multiplyBy = 1)
{
  var startingArrangement = Array.ConvertAll(File.ReadAllLines("input.txt"), str => long.Parse(str));
  var nodeList = new List<Node>(startingArrangement.Select(i => new Node { ToMove = (i * multiplyBy) }));

  for (int i = 0; i < nodeList.Count; i++)
  {
    nodeList[i].Before = nodeList[i == 0 ? nodeList.Count - 1 : i - 1];
    nodeList[i].After = nodeList[(i + 1) % nodeList.Count];
  }

  for (int i = 0; i < iterations; ++i)
  {
    for (int j = 0; j < nodeList.Count; j++)
    {
      var node = nodeList[j];
      if (node.ToMove == 0) continue;

      node.Before.After = node.After;
      node.After.Before = node.Before;

      var nodeBeforeConnection = node.Before;
      long toMove = Math.Abs(node.ToMove) % (nodeList.Count - 1);
      for (long k = 0; k < toMove; ++k)
      {
        nodeBeforeConnection = node.ToMove < 0 ? nodeBeforeConnection.Before : nodeBeforeConnection.After;
      }

      node.Before = nodeBeforeConnection;
      node.After = nodeBeforeConnection.After;

      nodeBeforeConnection.After = node;
      node.After.Before = node;
    }
  }

  Node startPoint = nodeList.First(node => node.ToMove == 0);
  return GetNodeValueXAway(startPoint, 1000, nodeList) + GetNodeValueXAway(startPoint, 2000, nodeList) + GetNodeValueXAway(startPoint, 3000, nodeList);
}

long GetNodeValueXAway(Node startPoint, int countToMove, List<Node> nodeList)
{
  countToMove %= nodeList.Count;
  Node nextNode = startPoint;

  for (int i = 0; i < countToMove; ++i)
  {
    nextNode = nextNode.After;
  }

  return nextNode.ToMove;
}

class Node
{
  public long ToMove { get; init; }
  public Node Before { get; set; }
  public Node After { get; set; }
}
