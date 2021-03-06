<Query Kind="Statements" />

int median(List<int> xs)
{
	var ys = xs.OrderBy(x => x).ToList();
	double mid = (ys.Count - 1) / 2.0;
	return (ys[(int)(mid)] + ys[(int)(mid + 0.5)]) / 2;
}

int solve1(string input)
{

	var crabs = input.Split(",").Select(int.Parse).ToList();

	var m = median(crabs);

	var cost = 0;
	foreach (var crab in crabs)
	{
		cost += Math.Abs(crab - m);
	}
	return cost;
}

int solve2(string input)
{
	var crabs = input.Split(",").Select(int.Parse).ToList();

	int stepCost(int start, int end)
	{
		var c = Math.Abs(start - end);
		for (var i = 1; i < Math.Abs(start - end); i++)
			c += i;

		return c;
	}

	var cost = int.MaxValue;
	for (var i = 0; i < crabs.Count/2; i++)
	{
		cost = Math.Min(cost, crabs.Sum(c => stepCost(c, i)));
	}
	return cost;
}

int solve2Fast(string input)
{
	var crabs = input.Split(",").Select(int.Parse).ToArray();
	var c = int.MaxValue;
	for (var i = 0; i < crabs.Length / 2; i++)
	{
		c = Math.Min(c, crabs.Sum(cr => Math.Abs(cr - i) * (Math.Abs(cr - i) + 1) / 2));
	}
	return c;
}

var testInput = "16,1,2,0,4,2,7,1,2,14";

solve1(testInput).Dump();
solve2(testInput).Dump();
solve2Fast(testInput).Dump();
