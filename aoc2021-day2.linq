<Query Kind="Statements" />

int solve1(IEnumerable<string> lines)
{
	int depth = 0, horizontal = 0;
	foreach (var line in lines)
	{
		var parts = line.Split();

		var direction = parts[0];
		var amount = int.Parse(parts[1]);

		switch (direction)
		{
			case "forward":
				horizontal += amount;
				break;
			case "down":
				depth += amount;
				break;
			case "up":
				depth -= amount;
				break;
		}
	}
	return horizontal * depth;
}

int solve2(IEnumerable<string> lines)
{
	int depth = 0, horizontal = 0, aim = 0;
	foreach (var line in lines)
	{
		var parts = line.Split();
		
		var direction = parts[0];
		var amount = int.Parse(parts[1]);
		
		switch (direction)
		{
			case "forward":
				horizontal += amount;
				depth += aim * amount;
				break;
			case "down":
				aim += amount;
				break;
			case "up":
				aim -= amount;
				break;
		}
	}
	return horizontal * depth;
}

solve1(new[]{
	"forward 5",
	"down 5",
	"forward 8",
	"up 3",
	"down 8",
	"forward 2"
}).Dump();

solve2(new[]{
	"forward 5",
	"down 5",
	"forward 8",
	"up 3",
	"down 8",
	"forward 2"
}).Dump();