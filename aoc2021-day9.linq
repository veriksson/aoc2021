<Query Kind="Statements" />

IEnumerable<(int x, int y)> neighbours(int x, int y)
{

	yield return (x - 1, y);
	yield return (x + 1, y);
	yield return (x, y + 1);
	yield return (x, y - 1);
	//yield return (x + 1, y + 1);
	//yield return (x + 1, y - 1);
	//yield return (x - 1, y - 1);
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
			if (neighbours(x, y)
				.Where(p => p.x > -1 && p.x < mx && p.y > -1 && p.y < my)
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
	var lowPoints = new List<(int x, int y, int v)>();

	for (var x = 0; x < mx; x++)
	{
		for (var y = 0; y < my; y++)
		{
			if (neighbours(x, y)
				.Where(p => p.x > -1 && p.x < mx && p.y > -1 && p.y < my)
				.Select(p => map[p.y, p.x]).All(v => v > map[y, x]))
			{
				lowPoints.Add((x, y, map[y, x]));
			}
		}
	}

	IEnumerable<(int x, int y)> spread(int x, int y, int v)
	{
		yield return (x, y);
		var mn = neighbours(x, y)
				.Where(p => p.x > -1 && p.x < mx && p.y > -1 && p.y < my && map[p.y, p.x] - 1 == v && map[p.y, p.x] != 9);
		if (!mn.Any())
			yield break;

		foreach (var n in mn)
		{
			var nn = spread(n.x, n.y, map[n.y, n.x]);
			foreach (var nnn in nn)
				yield return nnn;
		}
	}

	var basins = new List<List<(int x, int y)>>();
	int[,] marked = new int[my, mx];
	foreach (var point in lowPoints)
	{
		var n = spread(point.x, point.y, point.v);

		basins.Add(n.Distinct().ToList());
	}
	$"Number of basins: {basins.Count()}".Dump();


	//List<List<(int x, int y)>> join(List<List<(int x, int y)>> bs)
	//{
	//	var joined = new List<List<(int x, int y)>>();
	//	joined.Add(basins.First().ToList());
	//	foreach (var basin in basins.Skip(1))
	//	{
	//		bool added = false;
	//		foreach (var j in joined)
	//		{
	//			if (basin.Any(p => j.Contains(p)))
	//			{
	//				j.AddRange(basin);
	//				added = true;
	//				break;
	//			}
	//		}
	//		if (!added)
	//		{
	//			joined.Add(basin.ToList());
	//		}
	//	}
	//	return joined.Select(j => j.Distinct().ToList()).ToList();
	//}
	//while (true)
	//{
	//	var currentCount = basins.Count;
	//	basins = join(basins);
	//	var newCount = basins.Count();
	//	if (newCount == currentCount)
	//		break;
	//}
	//
	//foreach (var basin in basins)
	//{
	//	foreach (var p in basin)
	//	{
	//		marked[p.y, p.x]++;
	//	}
	//}
	var b = new System.Drawing.Bitmap(mx, my);
	using (var graphics = System.Drawing.Graphics.FromImage(b))
	{
		graphics.Clear(System.Drawing.Color.DarkRed);
	}

	foreach (var basin in basins)
	{

		foreach (var p in basin)
		{
			b.SetPixel(p.x, p.y, System.Drawing.Color.Blue);
		}
	}
	//marked.Dump();

	var biggest = basins.OrderByDescending(j => j.Count).Take(3);
	
	foreach (var basin in biggest)
	{

		foreach (var p in basin)
		{
			b.SetPixel(p.x, p.y, System.Drawing.Color.DarkCyan);
			//marked[p.y, p.x]++;//= map[p.y,p.x];
		}
	}
	b.Dump();

	return biggest.Select(b => b.Count).Aggregate(1, (c, n) => c * n);
}

var testInput = new List<string>(){
	"2199943210",
	"3987894921",
	"9856789892",
	"8767896789",
	"9899965678",
};
//
solve1(testInput).Dump();
solve2(testInput).Dump();

var realInput = File.ReadAllLines(@"C:\Users\gdh1c\Desktop\aoc2021_day9").ToList();
solve1(realInput).Dump();
solve2(realInput).Dump();