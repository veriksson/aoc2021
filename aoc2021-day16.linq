<Query Kind="Program" />

string binaryString(string hex)
{
	var bin = "";
	foreach (var c in hex)
	{
		switch (c)
		{
			case '0': bin += "0000"; break;
			case '1': bin += "0001"; break;
			case '2': bin += "0010"; break;
			case '3': bin += "0011"; break;
			case '4': bin += "0100"; break;
			case '5': bin += "0101"; break;
			case '6': bin += "0110"; break;
			case '7': bin += "0111"; break;
			case '8': bin += "1000"; break;
			case '9': bin += "1001"; break;
			case 'A': bin += "1010"; break;
			case 'B': bin += "1011"; break;
			case 'C': bin += "1100"; break;
			case 'D': bin += "1101"; break;
			case 'E': bin += "1110"; break;
			case 'F': bin += "1111"; break;
		}
	}
	return bin;
}

bool zeroes(string bin) => bin.All(c => c == '0');

(string bin, int version) version(string bin)
{
	var s = bin[0..3];
	return (bin[3..], Convert.ToInt32(s, 2));
}

// just a wrapper
(string bin, int type) type(string bin) => version(bin);

(string bin, int lenType) lenType(string bin)
{
	var lt = Convert.ToInt32(bin[0..1]);
	return (bin[1..], lt);
}

(string bin, long val) literal(string bin)
{
	var n = bin.Length / 5;
	var binval = "";
	var more = true;
	while (more)
	{
		more = bin[0] == '1';
		binval += bin[1..5];
		bin = bin[5..];
	}
	return (bin, Convert.ToInt64(binval, 2));
}

(string bin, int bitlen) bitlen(string bin, int c = 15)
{
	var bl = bin[0..c];
	return (bin[c..], Convert.ToInt32(bl, 2));
}

(string bin, int opversion, long opval) operation(string bin, int optype)
{
	var packets = new List<Packet>();
	(bin, var lt) = lenType(bin);
	switch (lt)
	{
		case 0:
			(bin, var spl) = bitlen(bin);
			var sub = bin[0..spl];
			bool more() => !zeroes(sub);
			do
			{
				(sub, var sp) = next(sub);
				packets.Add(sp);
			} while (more());
			bin = bin[spl..];
			break;
		case 1:
			(bin, var spc) = bitlen(bin, c: 11);
			do
			{
				(bin, var sp) = next(bin);
				packets.Add(sp);
			} while (--spc > 0);
			break;
	}

	var opversion = packets.Sum(sp => sp.version);
	long opval = 0;
	switch (optype)
	{
		case 0: opval = packets.Sum(sp => sp.value); break;
		case 1: opval = packets.Aggregate(1L, (c, n) => c * n.value); break;
		case 2: opval = packets.MinBy(sp => sp.value).value; break;
		case 3: opval = packets.MaxBy(sp => sp.value).value; break;
		case 5: opval = packets.First().value > packets.Last().value ? 1 : 0; break;
		case 6: opval = packets.First().value < packets.Last().value ? 1 : 0; break;
		case 7: opval = packets.First().value == packets.Last().value ? 1 : 0; break;
	}
	return (bin, opversion, opval);
}

(string bin, Packet) next(string bin)
{
	(bin, var v) = version(bin);
	(bin, var t) = type(bin);
	switch (t)
	{
		case 4:
			// literal
			(bin, var val) = literal(bin);
			return (bin, new(v, t, val));
		default:
			// operator
			(bin, var opver, var opval) = operation(bin, t);
			return (bin, new(v + opver, t, opval));
	}
}

IEnumerable<Packet> parsePackets(string bin)
{
	bool more() => !zeroes(bin);

	do
	{
		(bin, var packet) = next(bin);
		yield return packet;
	} while (more());
}


(long, long) solve(string input)
{
	var bin = binaryString(input);
	var packet = parsePackets(bin).First();

	//packets.Dump();
	return (packet.version, packet.value);
}

record struct Packet(int version, int type, long value);

void Main()
{
	var test = "9C0141080250320F1802104A08";
	solve(test).Dump();
}