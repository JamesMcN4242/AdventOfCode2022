using System.Text.Json;
using System.Text.Json.Nodes;

const string fileName = "input.txt";
PartOne();
PartTwo();

void PartOne()
{
  var input = Array.ConvertAll(File.ReadAllText(fileName).Replace("\r", "").Split("\n\n"), str => str.Split('\n'));

  int sum = 0;
  for (int i = 0; i < input.Length; i++)
  {
    string[] toCompare = input[i];
    if (InCorrectOrder(toCompare[0], toCompare[1])) sum += i + 1;
  }

  Console.WriteLine($"Part One: {sum}");
}

void PartTwo()
{
  var partTwoInput = new List<string>(File.ReadAllText(fileName).Replace("\r", "").Replace("\n\n", "\n").Split("\n"));
  partTwoInput.Add("[[2]]");
  partTwoInput.Add("[[6]]");
  partTwoInput.Sort((a, b) => InCorrectOrder(a, b) ? -1 : 1);

  Console.WriteLine($"Part Two: {(partTwoInput.IndexOf("[[2]]") + 1) * (partTwoInput.IndexOf("[[6]]") + 1)}");
}

bool InCorrectOrder(string toCompareOne, string toCompareTwo)
{
  var jsonOne = JsonNode.Parse(toCompareOne).AsArray();
  var jsonTwo = JsonNode.Parse(toCompareTwo).AsArray();

  for(int i = 0; i < jsonOne.Count && i < jsonTwo.Count; ++i)
  {
    var result = ValuesAreOutOfOrder(jsonOne[i], jsonTwo[i]);
    if (result.outOfOrder) return false;
    if (!result.shouldCont) return true;
  }

  return jsonTwo.Count > jsonOne.Count;
}

(bool outOfOrder, bool shouldCont) ValuesAreOutOfOrder(JsonNode leftSide, JsonNode rightSide)
{
  if (leftSide is JsonArray leftArr && rightSide is JsonArray rightArr)
  {
    for (int i = 0; i < leftArr.Count && i < rightArr.Count; ++i)
    {
      var result = ValuesAreOutOfOrder(leftArr[i], rightArr[i]);
      if (!result.shouldCont) return result;
    }

    return (leftArr.Count > rightArr.Count, leftArr.Count == rightArr.Count);
  }
  else if (leftSide is JsonArray)
  {
    return ValuesAreOutOfOrder(leftSide, new JsonArray(rightSide.Deserialize<JsonNode>()));
  }
  else if (rightSide is JsonArray)
  {
    return ValuesAreOutOfOrder(new JsonArray(leftSide.Deserialize<JsonNode>()), rightSide);
  }

  return (leftSide.GetValue<int>() > rightSide.GetValue<int>(), leftSide.GetValue<int>() == rightSide.GetValue<int>());
}
