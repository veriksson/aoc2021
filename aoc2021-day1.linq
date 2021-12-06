<Query Kind="Statements" />

int increases(int[] report)
{
	int last = report[0], inc = 0;
	for (var i = 1; i < report.Length; i++)
	{
		if (report[i] > last)
		{
			inc++;
		}
		last = report[i];
	}
	return inc;
}

IEnumerable<int> sliding(int[] report)
{
	for (var i = 0; i < report.Length - 2; i++)
	{
		yield return report[i] + report[i + 1] + report[i + 2];
	}
}

int solve1(int[] input) => increases(input);
int solve2(int[] input) => increases(sliding(input).ToArray());


var testInput = new[]{
	199,
	200,
	208,
	210,
	200,
	207,
	240,
	269,
	260,
	263
};

solve1(testInput).Dump();
solve2(testInput).Dump();