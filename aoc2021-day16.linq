<Query Kind="Statements" />

string parse4(string input)
{
	//var ops = new List<int>();
	var s = "";
	for (var i = 6; i + 5 < input.Length; i += 5)
	{
		//var op = Convert.ToInt32(input.Substring(i, 1), 2);
		//ops.Add(op);
		s += s.Substring(i + 1, 4);
	}
	return s;
}

int i(string input, int start, int length) => Convert.ToInt32(input.Substring(start, length), 2);

(int, int) lit(string input)
{
	var version = i(input,0,3);
	var type = i(input,3,3);
	
	return (version, i(input, 6, input.Length-6));
}

string parseOp(string input)
{
	var lenType = i(input, 0, 1);
	int len = 0;
	if (lenType == 1)
	{
		len = i(input, 1, 15);
		parseOp(input.Substring(15, len));
	}
	else if (lenType == 0)
	{
		len = i(input, 1, 10);
		input.Substring(11).Dump();
		for (var x = 0; x < len; x++)
		{
			lit(input.Substring((x+1)*11, 11)).Dump();
		}
	}
	else
	{
		throw new Exception("should not be here");
	}
	return input;
}

string parse1(string input)
{
	return input;
}

int solve1(string input)
{
	var a = Convert.ToInt64(input, 16);
	var b = Convert.ToString(a, 2);
	var version = Convert.ToInt32(b.Substring(0, 3), 2);
	var type = Convert.ToInt32(b.Substring(3, 3), 2);
	switch(type) {
		case 4:
			parse4(input);
			break;
		default:
			parseOp(b.Substring(7));
			break;
	}
	return 1;
}

int solve2(string input)
{
	return 2;
}

var testInput = "D2FE28";

//solve1(testInput).Dump();
//solve2(testInput).Dump();

var testInput3 = "EE00D40C823060";
solve1(testInput3);

//var realInput = File.ReadAllLines(@"C:\Users\gdh1c\Desktop\aoc2021_day16").ToList();
//solve1(realInput).Dump();
//solve2(realInput).Dump();
//