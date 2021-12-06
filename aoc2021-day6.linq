<Query Kind="Statements">
  <Namespace>System.Numerics</Namespace>
</Query>

long solve(string input, int days)
{
	var nums = input.Split(",").Select(int.Parse).ToList();

	var fish = new Dictionary<int, long>();

	for (var k = 0; k < 9; k++)
	{
		fish[k] = 0;
	}

	foreach (var num in nums)
	{
		fish[num]++;
	}

	for (int i = 0; i < days; i++)
	{
		var z = fish[0];

		for (var k = 1; k < 9; k++)
		{
			fish[k - 1] = fish[k];
			fish[k] = 0;
		}
		fish[6] += z;
		fish[8] = z;

	}
	return fish.Values.Sum();
}

var testInput = "3,4,3,1,2";

solve(testInput, 80).Dump();
solve(testInput, 256).Dump();