<Query Kind="Statements" />

Func<IEnumerable<char>, int> ones = (chars) => chars.Count(c => c == '1');
Func<IEnumerable<char>, int> zeroes = (chars) => chars.Count(c => c == '0');
Func<IEnumerable<string>, int, IEnumerable<char>> chars =
	(lines, index) => lines.Select(l => l.ElementAt(index));

Func<IEnumerable<string>, int, char> mostCommonOr1 = (lines, index) =>
{
	var vals = chars(lines, index);
	return ones(vals) >= zeroes(vals) ? '1' : '0';
};

Func<IEnumerable<string>, int, char> leastCommonOr0 = (lines, index) =>
{
	var vals = chars(lines, index);
	return zeroes(vals) <= ones(vals) ? '0' : '1';
};

int solve1(List<string> lines)
{
	var len = lines[0].Length;
	var gamma = "";
	for (var i = 0; i < len; i++)
	{
		gamma += mostCommonOr1(lines, i);
	}
	var epsilon = string.Join("", gamma.Select(c => c == '1' ? '0' : '1'));
	return Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);
}

int solve2(List<string> lines)
{
	string filter(IEnumerable<string> lines, 
				  Func<IEnumerable<string>, int, char> f, 
				  int index = 0)
	{
		while (lines.Count() > 1)
		{
			var keep = f(lines, index);
			lines = lines.Where(l => l.ElementAt(index) == keep).ToList();
			index++;
		}
		return lines.First();
	}

	var oxy = Convert.ToInt32(filter(lines, mostCommonOr1), 2);
	var co = Convert.ToInt32(filter(lines, leastCommonOr0), 2);
	return oxy * co;
}

var testInput = new List<string>{
	"00100",
	"11110",
	"10110",
	"10111",
	"10101",
	"01111",
	"00111",
	"11100",
	"10000",
	"11001",
	"00010",
	"01010",
};

solve1(testInput).Dump();
solve2(testInput).Dump();