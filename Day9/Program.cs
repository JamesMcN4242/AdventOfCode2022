var formattedInput = Array.ConvertAll(File.ReadAllLines("input.txt"), str =>
    (str.Split(' ')[0][0], int.Parse(str.Split(' ')[1])));
Console.WriteLine($"Part One: {Solve(formattedInput, 2)}");
Console.WriteLine($"Part One: {Solve(formattedInput, 10)}");

int Solve((char dir, int num)[] input, int knots)
{
    var knotPositions = new List<(int x, int y)>(knots);
    for (int i = 0; i < knots; ++i) knotPositions.Add((0, 0));
    var tailPositions = new HashSet<(int, int)>(input.Length) { knotPositions[0] };

    foreach (var instruction in input)
    {
        for (var i = 0; i < instruction.num; ++i)
        {
            var headPos = knotPositions[0];
            switch (instruction.dir)
            {
                case 'R': ++headPos.x; break;
                case 'L': --headPos.x; break;
                case 'U': ++headPos.y; break;
                case 'D': --headPos.y; break;
            }

            knotPositions[0] = headPos;
            for (int j = 1; j < knots; ++j)
            {
                headPos = knotPositions[j - 1];
                var tailPos = knotPositions[j];
                
                if (Math.Abs(headPos.x - tailPos.x) + Math.Abs(headPos.y - tailPos.y) == 2
                    && (headPos.x == tailPos.x || headPos.y == tailPos.y)
                    || Math.Abs(headPos.x - tailPos.x) + Math.Abs(headPos.y - tailPos.y) > 2)
                {
                    if (headPos.y == tailPos.y) tailPos.x += Math.Sign(headPos.x - tailPos.x);
                    else if (headPos.x == tailPos.x) tailPos.y += Math.Sign(headPos.y - tailPos.y);
                    else tailPos = (tailPos.x + Math.Sign(headPos.x - tailPos.x), tailPos.y + Math.Sign(headPos.y - tailPos.y));

                    knotPositions[j] = tailPos;
                    if (j == knots - 1) tailPositions.Add(tailPos);
                }
            }
        }
    }
    
    return tailPositions.Count;
}