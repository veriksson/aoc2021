<Query Kind="Statements" />

int[,] parse(List<string> input)
{
	var mx = input.First().Length;
	var my = input.Count;

	var map = new int[my, mx];
	for (int y = 0; y < my; y++)
	{
		for (int x = 0; x < mx; x++)
		{
			map[y, x] = int.Parse("" + input[y][x]);
		}
	}
	return map;
}

var directions = new (int y, int x)[]{
	(1,0),
	(-1,0),
	(0, 1),
	(0,-1)
};

int dfs(int[,] map, (int y, int x) current, (int y, int x) end, HashSet<(int y, int x)> visited, int min)
{
	foreach (var d in directions)
	{
		(int y, int x) next = (current.y + d.y, current.x + d.x);

		if (next == end)
		{
			var count = visited.Select(step => map[step.y, step.x]).Sum();
			return count + map[next.y, next.x];
		}

		if (visited.Contains(next))
			continue;

		if ((next.y < 0 || next.y >= map.GetLength(0)) || (next.x < 0 || next.x >= map.GetLength(1)))
		{
			// out of bounds
			continue;
		}


		visited.Add(current);
		var c = dfs(map, next, end, new HashSet<(int, int)>(visited), min);
		if (c > 0 && c < min)
		{
			min = c;
		}
		visited.Remove(current);
	}

	return min;
}

(Dictionary<(int y, int x), long>, Dictionary<(int y, int x), (int y, int x)>) djikstra(int[,] map, (int y, int x) start, (int y, int x) end)
{
	var set = new HashSet<(int y, int x)>();
	var dist = new Dictionary<(int y, int x), long>();
	var prev = new Dictionary<(int y, int x), (int y, int x)>();
	map.Iterate((y, x, _) =>
	{
		dist[(y, x)] = long.MaxValue;
		set.Add((y, x));
	});

	dist[start] = 0;

	while (set.Count() > 0)
	{
		var s = set.MinBy(ss => dist[ss]);
		set.Remove(s);
		foreach (var d in directions)
		{
			(int y, int x) next = (s.y + d.y, s.x + d.x);
			if (!set.Contains(next)) continue;
			var alt = dist[s] + map[next.y, next.x];
			if (alt < dist[next])
			{
				dist[next] = alt;
				prev[next] = s;
			}
		}
	}
	return (dist, prev);
}

bool inBounds(int[,] map, (int y, int x) coord)
{
	var my = map.GetLength(0);
	var mx = map.GetLength(1);
	return coord.y > -1 && coord.y < my && coord.x > -1 && coord.x < mx;
}

HashSet<(int y, int x)> stupidSolve(int[,] map, (int y, int x) start, (int y, int x) end)
{
	var visited = new HashSet<(int, int)>();
	visited.Add(start);
	while (start != end)
	{
		//var next = directions.Select(
	}
	return visited;
}

int solve1(List<string> input)
{
	var map = parse(input);
	var visited = new HashSet<(int y, int x)>();

	(int y, int x) start = (0, 0);
	(int y, int x) end = (map.GetLength(1) - 1, map.GetLength(0) - 1);
	var count = djikstra(map, start, end);
	//count -= map[start.y, start.x];
	//count += map[end.y, end.x];
	//var cs = paths.Select(steps => (steps, steps.Select(step => map[step.y, step.x]).Sum())).MinBy(k => k.Item2);
	//foreach (var k in visited)
	//{
	//	map[k.y, k.x] = 10;
	//}
	map.Dump();
	//map.Draw();

	return (int)count.Item1[end];
}

int solve2(List<string> input)
{
	return 2;
}

var testInput = new List<string>() {
	"1163751742",
	"1381373672",
	"2136511328",
	"3694931569",
	"7463417111",
	"1319128137",
	"1359912421",
	"3125421639",
	"1293138521",
	"2311944581",
};

solve1(testInput).Dump();
//solve2(testInput).Dump();

var smallInput = new List<string>() {
	"116",
	"138",
	"213",
};
solve1(smallInput).Dump();

var real = File.ReadAllLines(@"C:\Users\gdh1c\Desktop\aoc2021_day15").ToList();

solve1(real).Dump();
