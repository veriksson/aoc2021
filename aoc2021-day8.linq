<Query Kind="Statements" />

bool deduceOne(string s) => s.Length == 2;
bool deduceSeven(string s) => s.Length == 3;
bool deduceFour(string s) => s.Length == 4;
bool deduceEight(string s) => s.Length == 7;
Func<string, bool>[] deducers = new[] { deduceOne, deduceSeven, deduceFour, deduceEight };
bool deduceAny(string s) => deducers.Any(d => d.Invoke(s));

int solve1(List<string> input)
{
	var ret = 0;
	foreach (var line in input)
	{
		var parts = line.Split("|", StringSplitOptions.TrimEntries);
		var signal = parts[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		var output = parts[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

		foreach (var thing in output)
		{
			if (deduceAny(thing))
				ret++;
		}
	}
	return ret;
}

var fours = new Dictionary<(int, int), string>() {
	{(3,2), "2"},
	{(0,0), "4"},
	{(2,0), "9"},
};

var ones = new Dictionary<(int, int), string>() {
	{(0,0), "1"},
	{(5,1), "6"},
};

var sevens = new Dictionary<(int, int), string>() {
	{(0,0), "7"},
	{(2,0), "3"},
	{(3,0), "0"},
	{(3,1), "5"},
};

int solve2(List<string> input)
{
	var ret = 0;
	foreach (var line in input)
	{
		var parts = line.Split("|", StringSplitOptions.TrimEntries);
		var signal = parts[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
		var output = parts[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

		string one = "", four = "", seven = "";
		foreach (var s in signal)
		{
			if (deduceOne(s))
			{
				one = s;
			}
			else if (deduceSeven(s))
			{
				seven = s;
			}
			else if (deduceFour(s))
			{
				four = s;
			}
		}

		(int a, int b) remainder(string s1, string s2)
		{
			var a = s1.Except(s2).Count();
			var b = s2.Except(s1).Count();
			return (a, b);
		}
		var num = string.Empty;
		foreach (var o in output)
		{
			if (deduceEight(o))
			{
				num += "8";
			}
			else if (ones.TryGetValue(remainder(o, one), out var k1))
			{
				num += k1;
			}
			else if (fours.TryGetValue(remainder(o, four), out var k2))
			{
				num += k2;
			}

			else if (sevens.TryGetValue(remainder(o, seven), out var k3))
			{
				num += k3;
			}
		}

		ret += Convert.ToInt32(num);
	}
	return ret;
}

var testInput = new List<string>() {
"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe",
"edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc",
"fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg",
"fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb",
"aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea",
"fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb",
"dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe",
"bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef",
"egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb",
"gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce",
};

solve1(testInput).Dump();
solve2(testInput).Dump();