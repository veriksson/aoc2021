<Query Kind="Statements" />

int[,] parse(List<string> input)
{
	var arr = new int[input.Count, input[0].Length];
	for (int y = 0; y < input.Count; y++)
	{
		for (int x = 0; x < input[0].Length; x++)
		{
			arr[y, x] = Convert.ToInt32("" + input[y][x]);
		}
	}
	return arr;
}


int solve(List<string> input, bool getStepCount = false)
{
	var octopuses = parse(input);
	var flashes = new bool[octopuses.GetLength(0), octopuses.GetLength(1)];

	int flash(int y, int x, int acc = 0)
	{
		if (flashes[y, x])
		{
			return 0;
		}

		octopuses[y, x]++;
		if (octopuses[y, x] > 9)
		{
			flashes[y, x] = true;
			acc++;
			octopuses[y, x] = 0;
			foreach (var n in octopuses.Neighbors(y, x))
			{
				acc += flash(n.y, n.x);
			}
		}

		return acc;
	}

	var flashCount = 0;
	bool step()
	{
		octopuses.Iterate((y, x, v) =>
		{
			flashCount += flash(y, x);
		});
		var all = true;
		flashes.Iterate((y, x, v) =>
		{
			flashes[y, x] = false;
			all = all ? v : false;
		});
		return all;
	}
	if (getStepCount)
	{
		var stepCount = 0;
		do
		{
			stepCount++;
		} while (!step());
		return stepCount;
	}
	else
	{
		for (int i = 0; i < 100; i++)
		{
			step();
		}
		return flashCount;
	}
}

var testInput = new List<string>() {
	"5483143223",
	"2745854711",
	"5264556173",
	"6141336146",
	"6357385478",
	"4167524645",
	"2176841721",
	"6882881134",
	"4846848554",
	"5283751526",
};

solve(testInput, getStepCount: false).Dump();
solve(testInput, getStepCount: true).Dump();