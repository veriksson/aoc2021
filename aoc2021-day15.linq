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

Dictionary<(int y, int x), long> djikstra(int[,] map, (int y, int x) start, (int y, int x) end)
{
	var q = new PriorityQueue<(int y, int x), long>();
	var dist = new Dictionary<(int y, int x), long>();

	var my = map.GetLength(0);
	var mx = map.GetLength(1);
	for (var y = 0; y < my; y++)
	{
		for (var x = 0; x < mx; x++)
		{
			if ((y, x) == start) continue;
			dist[(y, x)] = long.MaxValue;
			q.Enqueue((y, x), dist[(y, x)]);
		}
	}

	dist[start] = 0;
	q.Enqueue(start, 0);
	while (q.Count > 0)
	{
		var s = q.Dequeue();
		foreach (var d in directions)
		{
			(int y, int x) next = (s.y + d.y, s.x + d.x);
			if (next.y < 0 || next.y > my - 1) continue;
			if (next.x < 0 || next.x > mx - 1) continue;

			var newDist = dist[s] + map[next.y, next.x];
			if (newDist < dist[next])
			{
				dist[next] = newDist;
				q.Enqueue(next, newDist);
			}
		}
	}
	return dist;
}

long solve1(List<string> input)
{
	var map = parse(input);
	(int y, int x) start = (0, 0);
	(int y, int x) end = (map.GetLength(1) - 1, map.GetLength(0) - 1);
	var distances = djikstra(map, start, end);
	return distances[end];
}

int[,] expand(int[,] map)
{
	var origy = map.GetLength(0);
	var origx = map.GetLength(1);
	int col = 0, row = 0, vx = 0, vy = 0;
	var expanded = new int[origy * 5, origx * 5];
	for (var y = 0; y < expanded.GetLength(0); y++)
	{
		for (var x = 0; x < expanded.GetLength(1); x++)
		{
			var yy = y % origy;
			var xx = x % origx;
			expanded[y, x] = (map[yy, xx] + vx + vy);
			if (expanded[y, x] > 9)
				expanded[y, x] -= 9;

			if (++col == origx)
			{
				vx++;
				col = 0;
			}
		}
		col = 0;
		vx = 0;
		if (++row == origy)
		{
			row = 0;
			vy++;
		}
	}

	return expanded;
}

long solve2(List<string> input)
{
	var map = expand(parse(input));
	(int y, int x) start = (0, 0);
	(int y, int x) end = (map.GetLength(1) - 1, map.GetLength(0) - 1);
	var distances = djikstra(map, start, end);
	return distances[end];
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
solve2(testInput).Dump();
