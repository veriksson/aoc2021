<Query Kind="Statements" />

Dictionary<string, HashSet<string>> parse(List<string> input)
{
	var dict = new Dictionary<string, HashSet<string>>();
	foreach (var line in input)
	{
		var parts = line.Split("-");

		var k = parts[0];
		var v = parts[1];
		if (dict.ContainsKey(k))
		{
			dict[k].Add(v);
		}
		else
		{
			dict[k] = new HashSet<string>() {
				v
			};
		}
		if (dict.ContainsKey(v))
		{
			dict[v].Add(k);
		}
		else
		{
			dict[v] = new HashSet<string>() {
				k
			};
		}
	}
	return dict;
}

IEnumerable<string> travel(Dictionary<string, HashSet<string>> map, HashSet<string> visited, string currentNode = "start", string ignore = "")
{
	if (currentNode == "end")
	{
		yield return currentNode;
		yield break;
	}

	var routes = map[currentNode];

	if (currentNode.All(char.IsLower) && currentNode != ignore)
		visited.Add(currentNode);

	var head = currentNode;
	if (currentNode == ignore)
	{
		ignore = "";
	}
	foreach (var route in routes)
	{
		if (visited.Contains(route))
			continue;

		foreach (var next in travel(map, new HashSet<string>(visited), route, ignore))
			yield return head + "," + next;
	}

}

int solve1(List<string> input)
{
	var map = parse(input);
	var v = travel(map, new HashSet<string>());

	return v.Count();
}

int solve2(List<string> input)
{
	var map = parse(input);
	var ignores = map.Keys.Where(k => k != "start" && k != "end" && k.All(char.IsLower)).ToList();
	var routes = new List<string>();
	foreach (var ignore in ignores)
	{
		routes.AddRange(travel(map, new HashSet<string>(), ignore: ignore));
	}

	return routes.Distinct().Count();
}

var testInput = new List<string>() {
	"start-A",
	"start-b",
	"A-c",
	"A-b",
	"b-d",
	"A-end",
	"b-end",
};

solve1(testInput).Dump();
solve2(testInput).Dump();

var difficult = new List<string>() {
	"dc-end",
	"HN-start",
	"start-kj",
	"dc-start",
	"dc-HN",
	"LN-dc",
	"HN-end",
	"kj-sa",
	"kj-HN",
	"kj-dc",
};
solve1(difficult).Dump();
solve2(difficult).Dump();

var hardestTest = new List<string>() {
	"fs-end",
	"he-DX",
	"fs-he",
	"start-DX",
	"pj-DX",
	"end-zg",
	"zg-sl",
	"zg-pj",
	"pj-he",
	"RW-he",
	"fs-DX",
	"pj-RW",
	"zg-RW",
	"start-pj",
	"he-WI",
	"zg-he",
	"pj-fs",
	"start-RW",
};

solve1(hardestTest).Dump();
solve2(hardestTest).Dump();