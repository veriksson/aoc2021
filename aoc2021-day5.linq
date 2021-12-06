<Query Kind="Statements" />

(int x1, int y1, int x2, int y2) parseLine(string line)
{
	var parts = line.Split();
	var firstPair = parts.First().Split(",").Select(int.Parse).ToArray();
	var secondPair = parts.Last().Split(",").Select(int.Parse).ToArray();

	return (firstPair[0], firstPair[1], secondPair[0], secondPair[1]);
}

IEnumerable<(int x, int y)> gen(int x1, int x2, int y1, int y2)
{
	List<int> xSteps = new();
	List<int> ySteps = new();

	if (x1 < x2)
	{
		xSteps = Enumerable.Range(x1, x2 - x1 + 1).ToList();
	}
	else
	{
		xSteps = Enumerable.Range(x2, x1 - x2 + 1).Reverse().ToList();
	}
	if (y1 < y2)
	{
		ySteps = Enumerable.Range(y1, y2 - y1 + 1).ToList();
	}
	else
	{
		ySteps = Enumerable.Range(y2, y1 - y2 + 1).Reverse().ToList();
	}
	return xSteps.Zip(ySteps);
}

int solveDay5(List<string> input)
{
	var lines = input.Select(parseLine).ToList();
	var maxX1 = lines.MaxBy(line => line.x1).x1;
	var maxX2 = lines.MaxBy(line => line.x2).x2;
	var maxY1 = lines.MaxBy(line => line.y1).y1;
	var maxY2 = lines.MaxBy(line => line.y2).y2;

	var mapMaxX = Math.Max(maxX1, maxX2) + 1;
	var mapMaxY = Math.Max(maxY1, maxY2) + 1;
	int[,] map = new int[mapMaxY, mapMaxX];
	foreach (var line in lines)
	{
		var startX = Math.Min(line.x1, line.x2);
		var endX = Math.Max(line.x1, line.x2);

		var startY = Math.Min(line.y1, line.y2);
		var endY = Math.Max(line.y1, line.y2);

		if (startX == endX || startY == endY)
		{
			for (var x = startX; x < endX + 1; x++)
			{
				for (var y = startY; y < endY + 1; y++)
				{
					map[y, x]++;
				}
			}
		}
		else
		{
			foreach (var pair in gen(line.x1, line.x2, line.y1, line.y2))
			{
				map[pair.y, pair.x]++;
			}
		}
	}
	var min2 = 0;
	for (var x = 0; x < mapMaxX; x++)
	{
		for (var y = 0; y < mapMaxY; y++)
		{
			if (map[y, x] > 1)
			{
				min2++;
			}
		}
	}
	return min2;
}

var testInput = new List<string>(){
	"0,9 -> 5,9",
	"8,0 -> 0,8",
	"9,4 -> 3,4",
	"2,2 -> 2,1",
	"7,0 -> 7,4",
	"6,4 -> 2,0",
	"0,9 -> 2,9",
	"3,4 -> 1,4",
	"0,0 -> 8,8",
	"5,5 -> 8,2",
};

solveDay5(testInput).Dump();