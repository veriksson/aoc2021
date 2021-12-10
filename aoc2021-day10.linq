<Query Kind="Statements" />

char parse(string line)
{
	var s = new Stack<char>();
	for (var i = 0; i < line.Length; i++)
	{
		switch (line[i])
		{
			case '(': s.Push(')'); break;
			case ')' when s.Pop() != ')':
				return ')';
			case '[': s.Push(']'); break;
			case ']' when s.Pop() != ']':
				return ']';
			case '{': s.Push('}'); break;
			case '}' when s.Pop() != '}':
				return '}';
			case '<': s.Push('>'); break;
			case '>' when s.Pop() != '>':
				return '>';
		}
	}
	return 'x';
}

int solve1(List<string> input)
{
	var v = new int[4];
	foreach (var line in input)
	{
		switch (parse(line))
		{
			case ')': v[0]++; break;
			case ']': v[1]++; break;
			case '}': v[2]++; break;
			case '>': v[3]++; break;
		}
	}
	return (v[0] * 3) + (v[1] * 57) + (v[2] * 1197) + (v[3] * 25137);
}

string autocomplete(string line)
{
	var s = new Stack<char>();
	for (var i = 0; i < line.Length; i++)
	{
		switch (line[i])
		{
			case '(': s.Push('('); break;
			case ')': s.Pop(); break;
			case '[': s.Push('['); break;
			case ']': s.Pop(); break;
			case '{': s.Push('{'); break;
			case '}': s.Pop(); break;
			case '<': s.Push('<'); break;
			case '>': s.Pop(); break;
		}
	}
	var sb = new StringBuilder();
	var v = new int[4];
	while (s.TryPop(out var c))
	{
		switch (c)
		{
			case '(': sb.Append(')'); break;
			case '[': sb.Append(']'); break;
			case '{': sb.Append('}'); break;
			case '<': sb.Append('>'); break;
		}
	}
	return sb.ToString();
}

long score(string complete)
{
	long i = 0;
	foreach (var c in complete)
	{
		i *= 5;
		switch (c)
		{
			case ')': i += 1; break;
			case ']': i += 2; break;
			case '}': i += 3; break;
			case '>': i += 4; break;
		}
	}
	return i;
}

long solve2(List<string> input)
{
	var incomplete = input.Where(i => parse(i) == 'x').ToList();
	var completes = incomplete.Select(autocomplete).ToList();
	var scores = completes.Select(score).OrderBy(s => s).ToList();
	return scores[scores.Count / 2];
}

var testInput = new List<string>()
{
	"[({(<(())[]>[[{[]{<()<>>",
	"[(()[<>])]({[<{<<[]>>(",
	"{([(<{}[<>[]}>{[]{[(<()>", // corrupted
	"(((({<>}<{<{<>}{[]{[]{}",
	"[[<[([]))<([[{}[[()]]]", // corrupted
	"[{[{({}]{}}([{[{{{}}([]", // corrupted
	"{<[[]]>}<{[{[{[]{()[[[]",
	"[<(<(<(<{}))><([]([]()", // corrupted
	"<{([([[(<>()){}]>(<<{{", // corrupted
	"<{([{{}}[<[[[<>{}]]]>[]]"
};

solve1(testInput).Dump();
solve2(testInput).Dump();