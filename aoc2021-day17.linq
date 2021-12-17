<Query Kind="Statements" />

((int y, int x) start, (int y, int x) end) parse(string input)
{
	var m = Regex.Matches(input, @"(-?\d+)", RegexOptions.None);
	var x1 = int.Parse(m[0].Groups[0].Value);
	var x2 = int.Parse(m[1].Groups[0].Value);
	var y1 = int.Parse(m[2].Groups[0].Value);
	var y2 = int.Parse(m[3].Groups[0].Value);

	return ((y1, x1), (y2, x2));
}

(int, int) solve(string input)
{
	var (start, end) = parse(input);

	bool oob((int y, int x) pos)
	{
		if (pos.y < start.y) return true;
		if (pos.x > end.x) return true;
		return false;
	}

	bool inside((int y, int x) pos)
	{
		if (pos.x < start.x) return false;
		if (pos.x > end.x) return false;
		if (pos.y < start.y) return false;
		if (pos.y > end.y) return false;
		return true;
	}
	
	(bool, int) shoot(int vy, int vx)
	{
		(int y, int x) pos = (0, 0);
		(int y, int x) vel = (vy, vx);
		var my = pos.y;
		do
		{
			pos = (pos.y + vel.y, pos.x + vel.x);

			my = Math.Max(my, pos.y);
			var nx = vel.x;
			if (nx > 0)
			{
				nx--;
			}
			else if (nx < 0)
			{
				nx++;
			}
			vel = (vel.y - 1, nx);
			if (inside(pos))
			{
				return (true, my);
			}
		} while (!oob(pos));
		return (false, 0);
	}

	var found = new HashSet<(int y, int x)>();
	var highest = 0;
	
	// tbh I don't know how to get these parameters.
	// use the start / end of the target somehow?
	for (int yv = -200; yv < 200; yv++)
	{
		for (int xv = -200; xv < 200; xv++)
		{
			var (ok, high) = shoot(yv, xv);
			if (ok)
			{
				highest = Math.Max(highest, high);
				found.Add((yv, xv));
			}
		}
	}

	return (highest, found.Count);
}


var test = "target area: x=20..30, y=-10..-5";
solve(test).Dump();