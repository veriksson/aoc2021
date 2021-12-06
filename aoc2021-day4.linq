<Query Kind="Program" />

void Main()
{
	var input = @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7".Split("\r\n");

	var numbers = ParseNumbers(input.First());
	var cards = ParseCards(input.Skip(2).ToList()).ToList();

	foreach (var number in numbers)
	{
		foreach (var card in cards.Where(c => !c.HasWon))
		{
			card.Set(number);
		}
	}
}

List<int> ParseNumbers(string numbers)
{
	return numbers.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToList();
}

IEnumerable<BingoCard> ParseCards(List<string> cards)
{
	var sb = new StringBuilder();
	foreach (var line in cards)
	{
		if (string.IsNullOrEmpty(line))
		{
			yield return new BingoCard(sb.ToString());
			sb = new StringBuilder();
			continue;
		}

		sb.AppendLine(line);
	}
	var lastBoard = sb.ToString();
	if(!string.IsNullOrEmpty(lastBoard))
		yield return new BingoCard(lastBoard);
	yield return new BingoCard(sb.ToString());
}
class BingoCard
{
	class Coord
	{
		public int X = -1, Y = -1;
		public bool C = false;
		public static Coord New(int x, int y)
		{
			return new Coord()
			{
				X = x,
				Y = y,
			};
		}

		public override string ToString()
		{
			return $"({X}, {Y}) = {C}";
		}
	}



	private readonly Dictionary<int, Coord> board = new Dictionary<int, Coord>();
	private readonly int size = 5;
	
	public bool HasWon;
	public BingoCard(string card)
	{
		SetBoard(card);
	}

	void SetBoard(string card)
	{
		var values = card.Split(new string[] { " ", "\n" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
		for (var x = 0; x < size; x++)
		{
			for (var y = 0; y < size; y++)
			{
				board[values[size * x + y]] = Coord.New(x, y);
			}
		}
	}

	public bool Set(int v)
	{
		if (!board.TryGetValue(v, out var coord))
		{
			return false;
		}
		coord.C = true;
		var win = CheckForWin(coord);
		if (win)
		{
			HasWon = true;
			(UnmarkedSum() * v).Dump();
		}
		return win;
	}

	bool CheckForWin(Coord key)
	{
		return HorizontalWin(key) || VerticalWin(key);
	}

	bool VerticalWin(Coord key)
	{
		for (var y = 0; y < size; y++)
		{
			var v = board.Values.SingleOrDefault(c => c.X == key.X && c.Y == y);
			if (!v.C)
			{
				return false;
			}
		}
		return true;
	}

	bool HorizontalWin(Coord key)
	{
		for (var x = 0; x < size; x++)
		{
			var v = board.Values.SingleOrDefault(c => c.X == x && c.Y == key.Y);
			if (!v.C)
			{
				return false;
			}
		}
		return true;
	}

	int UnmarkedSum()
	{
		return board.Where(v => !v.Value.C).Sum(v => v.Key);
	}
}
