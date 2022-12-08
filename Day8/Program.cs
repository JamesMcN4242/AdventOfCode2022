var formattedInput = Array.ConvertAll(File.ReadAllLines("input.txt"), str => Array.ConvertAll(str.ToCharArray(), c => byte.Parse(c.ToString())));
Console.WriteLine($"Part One: {SolvePartOne(formattedInput)}");
Console.WriteLine($"Part Two: {SolvePartTwo(formattedInput)}");

int SolvePartOne(byte[][] input)
{
    int visibleTrees = input.Length * 2 + input[0].Length * 2 - 4;
    List<byte> maxTop = new List<byte>(input[0]);
    
    for (int i = 1; i < input.Length - 1; ++i)
    {
        byte maxLeft = input[i][0];

        for (int j = 1; j < input[i].Length - 1; ++j)
        {
            byte maxRight = input[i].Skip(j+1).Max();
            byte maxBelow = input.Skip(i+1).Max(b => b[j]);
            
            if (input[i][j] > maxLeft || input[i][j] > maxTop[j] || input[i][j] > maxRight || input[i][j] > maxBelow)
            {
                ++visibleTrees;
            }

            maxLeft = Math.Max(maxLeft, input[i][j]);
            maxTop[j] = Math.Max(maxTop[j], input[i][j]);
        }
    }
    
    return visibleTrees;
}

long SolvePartTwo(byte[][] input)
{
    var verticalArrangement = new List<byte[]>(input[0].Length);
    for (int i = 0; i < input[0].Length; ++i)
    {
        int index = i;
        verticalArrangement.Add(Array.ConvertAll(input, bytes => bytes[index]));
    }

    long maxVisScore = 0;
    for (int i = 1; i < input.Length - 1; ++i)
    {
        for (int j = 1; j < input[i].Length - 1; ++j)
        {
            long visScore =  GetLAndRSight(input[i], j) * GetLAndRSight(verticalArrangement[j], i);
            maxVisScore = Math.Max(maxVisScore, visScore);
        }
    }
    
    return maxVisScore;
}

long GetLAndRSight(byte[] line, int index)
{
    int startingIndex = index;

    long treesOnLeft = 0;
    for (int i = startingIndex - 1; i >= 0; --i)
    {
        ++treesOnLeft;
        if (line[startingIndex] <= line[i]) break;
    }
    
    long treesOnRight = 0;
    for (int i = startingIndex + 1; i < line.Length; ++i)
    {
        ++treesOnRight;
        if (line[startingIndex] <= line[i]) break;
    }
    
    return treesOnLeft * treesOnRight;
}