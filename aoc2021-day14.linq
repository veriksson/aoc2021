<Query Kind="Statements" />

(Dictionary<char, long> counter, Dictionary<(char, char), long> pairs, Dictionary<(char, char), char> rules) parse(List<string> input)
{
	var rules = new Dictionary<(char, char), char>();
	foreach (var line in input.Skip(2))
	{
		var parts = line.Split();
		rules[(parts[0][0], parts[0][1])] = parts.Last()[0];
	}

	var template = input.First().ToCharArray();
	var counter = new Dictionary<char, long>();
	for (int i = 0; i < template.Length; i++)
	{
		add(counter, template[i], 1);
	}
	var pairs = new Dictionary<(char, char), long>();
	for (int i = 1; i < template.Length; i++)
	{
		add(pairs, (template[i - 1], template[i]), 1);
	}

	return (counter, pairs, rules);
}

void add<T>(Dictionary<T, long> dict, T key, long v)
{
	if (dict.ContainsKey(key))
	{
		dict[key] += v;
	}
	else
	{
		dict[key] = v;
	}
}

void step(Dictionary<char, long> current, Dictionary<(char, char), long> pairs, Dictionary<(char, char), char> rules)
{
	var keys = pairs.Keys.ToList();
	var copy = pairs.ToDictionary(k => k.Key, v => v.Value);
	foreach (var key in keys)
	{
		if (pairs[key] > 0 && rules.TryGetValue(key, out var v))
		{
			var pairCount = copy[key];
			// remove the pairs since they are now being replaced
			add(pairs, key, pairCount * -1);
			// increment character count for each pair removed
			add(current, v, pairCount);
			// increment for new pairs
			add(pairs, (key.Item1, v), pairCount);
			add(pairs, (v, key.Item2), pairCount);
		}
	}
}

long solve(List<string> input, int steps = 10)
{
	var (counter, pairs, rules) = parse(input);
	for (int i = 0; i < steps; i++)
	{
		step(counter, pairs, rules);
	}
	return counter.Values.Max() - counter.Values.Min();
}

var testInput = new List<string>() {
	   "NNCB",
	   "",
	   "CH -> B",
	   "HH -> N",
	   "CB -> H",
	   "NH -> C",
	   "HB -> C",
	   "HC -> B",
	   "HN -> C",
	   "NN -> C",
	   "BH -> H",
	   "NC -> B",
	   "NB -> B",
	   "BN -> B",
	   "BB -> N",
	   "BC -> B",
	   "CC -> N",
	   "CN -> C",
};

solve(testInput).Dump();
solve(testInput, 40).Dump();