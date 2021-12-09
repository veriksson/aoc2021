<Query Kind="Statements" />

IEnumerable<(int x, int y)> neighbours(int x, int y, int mx, int my)
{
	if (x > 0)
		yield return (x - 1, y);
	if (x < mx - 1)
		yield return (x + 1, y);
	if (y > 0)
		yield return (x, y - 1);
	if (y < my - 1)
		yield return (x, y + 1);
}

int[,] genMap(List<string> input)
{
	var mx = input.First().Length;
	var my = input.Count;
	int[,] map = new int[my, mx];

	for (int x = 0; x < mx; x++)
	{
		for (int y = 0; y < my; y++)
		{
			map[y, x] = int.Parse("" + input[y][x]);
		}
	}
	return map;
}

int solve1(List<string> input)
{
	var map = genMap(input);
	var mx = map.GetLength(1);
	var my = map.GetLength(0);

	var risk = 0;
	for (var x = 0; x < mx; x++)
	{
		for (var y = 0; y < my; y++)
		{
			if (neighbours(x, y, mx, my)
				.Select(p => map[p.y, p.x]).All(v => v > map[y, x]))
			{
				risk += (1 + map[y, x]);
			}
		}
	}
	return risk;
}

int solve2(List<string> input)
{
	var map = genMap(input);
	var mx = map.GetLength(1);
	var my = map.GetLength(0);
	
	var lowPoints = new List<(int x, int y)>();

	for (var x = 0; x < mx; x++)
	{
		for (var y = 0; y < my; y++)
		{
			if (neighbours(x, y, mx, my)
				.Select(p => map[p.y, p.x]).All(v => v > map[y, x]))
			{
				lowPoints.Add((x, y));
			}
		}
	}

	var basins = lowPoints.Select(lp => genBasin(map, lp.x, lp.y));
	var top3 = basins.Select(b => b.Distinct().Count()).OrderByDescending(b => b).Take(3);

	return top3.Aggregate(1, (curr, next) => curr * next);
}

List<(int x, int y)> genBasin(int[,] map, int x, int y)
{
	var basin = new List<(int, int)>()
	{
		(x,y)
	};

	var ns = neighbours(x, y, map.GetLength(1), map.GetLength(0)).Where(p => map[p.y, p.x] > map[y, x] && map[p.y, p.x] != 9);
	foreach (var n in ns)
	{
		basin.AddRange(genBasin(map, n.x, n.y));
	}
	return basin;
}

var testInput = new List<string>(){
	"2199943210",
	"3987894921",
	"9856789892",
	"8767896789",
	"9899965678",
};

solve1(testInput).Dump();
solve2(testInput).Dump();
