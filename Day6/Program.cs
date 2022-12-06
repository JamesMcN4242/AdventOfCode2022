string input = File.ReadAllText("input.txt");
Console.WriteLine($"Part One: {GetMarkerPosition(input, 4)}");
Console.WriteLine($"Part One: {GetMarkerPosition(input, 14)}");

int GetMarkerPosition(string input, int uniqueToMatch)
{
  HashSet<char> characters = new HashSet<char>(uniqueToMatch);

  for (int i = 0; i < input.Length - uniqueToMatch; ++i)
  {
    characters.Clear();

    for (int j = i; j < i + uniqueToMatch; ++j)
      characters.Add(input[j]);

    if (characters.Count == uniqueToMatch)
      return i + uniqueToMatch;
  }

  return -1;
}
