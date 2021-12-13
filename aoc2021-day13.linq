<Query Kind="Statements" />

(bool[,], (string direction, int at)[]) parse(List<string> input)
{

	var dots = input.TakeWhile(l => l != "").Select(l =>
	{
		var parts = l.Split(",");
		return (int.Parse(parts[0]), int.Parse(parts[1]));
	}).ToArray();

	var folds = input.SkipWhile(l => l != "").Skip(1).Select(l =>
		{
			var parts = l.Split().Last().Split("=");
			return (parts.First(), int.Parse(parts.Last()));
		}).ToArray();

	// figure out biggest fold to get width & height, since folds are always
	// in the middle of the "paper".
	var maxX = 1 + folds.Where(f => f.Item1 == "x").Select(f => f.Item2).Max() * 2;
	var maxY = 1 + folds.Where(f => f.Item1 == "y").Select(f => f.Item2).Max() * 2;
	var map = new bool[maxY, maxX];

	foreach (var d in dots)
	{
		map[d.Item2, d.Item1] = true;
	}
	return (map, folds);
}

void copyh(bool[,] map1, bool[,] map2)
{
	var m1y = map1.GetLength(0) - 1;
	map2.Iterate((y, x, _) =>
	{
		map2[y, x] = map1[y, x] | map1[m1y - y, x];
	});
}

void copyw(bool[,] map1, bool[,] map2)
{
	var m1x = map1.GetLength(1) - 1;
	map2.Iterate((y, x, _) =>
	{
		map2[y, x] = map1[y, x] | map1[y, m1x - x];
	});
}

bool[,] fold(bool[,] map, (string dir, int at) fold)
{
	bool[,] folded = null;
	switch (fold.dir)
	{
		case "y":
			folded = new bool[fold.at, map.GetLength(1)];
			copyh(map, folded);
			break;
		case "x":
			folded = new bool[map.GetLength(0), fold.at];
			copyw(map, folded);
			break;
	}

	return folded;
}

int solve1(List<string> input)
{
	var (map, folds) = parse(input);
	var folded = fold(map, folds.First());
	var dotCount = 0;
	foreach (var element in folded)
		if (element) dotCount++;

	return dotCount;

}

void solve2(List<string> input)
{
	var (map, folds) = parse(input);
	folds.Aggregate(map, fold).Draw();
}

var testInput = new List<string>() {
	"6,10",
	"0,14",
	"9,10",
	"0,3",
	"10,4",
	"4,11",
	"6,0",
	"6,12",
	"4,1",
	"0,13",
	"10,12",
	"3,4",
	"3,0",
	"8,4",
	"1,10",
	"2,14",
	"8,10",
	"9,0",
	"",
	"fold along y=7",
	"fold along x=5"
};

solve1(testInput).Dump();
solve2(testInput);