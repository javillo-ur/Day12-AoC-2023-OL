Console.WriteLine(File.ReadAllLines("input.txt")
    .Select(r => new{line = r.Split(" ")[0].Select((c, i) => new{character = c, index = i}),
                    groups = r.Split(" ")[1].Split(",").Where(t => int.TryParse(t, out _)).Select(t => int.Parse(t)).ToList()})
    .Select(r => new{
        line = r.line.Aggregate(new List<Tuple<int,bool>>(), (previous, current) => current.character == '#' ? [.. previous , new(current.index, true)] : current.character == '?' ? [.. previous , new(current.index, false)] : previous)
            .ToDictionary(t => t.Item1, t => t.Item2),
        groups = r.groups.Select((g , i) => new{ g, i }).ToList()})
    .Select(r => r.groups.Aggregate(new List<Tuple<int, int,string>>(){new(0, r.groups.Count, "")}, (previous, current) => 
        previous.SelectMany(d => r.line.Keys.Where(t => t >= d.Item1
                                && r.line.Keys.Where(w => w < t && w >= d.Item1).All(s => !r.line[s])
                                && Enumerable.Range(0, current.g).All(s => r.line.ContainsKey(t+s))
                                && (!r.line.ContainsKey(t + current.g) || !r.line[t + current.g])
                                && (d.Item2 != 1 || r.line.Keys.Where(k => k > t + current.g).All(t => !r.line[t])))
                                .Select(ind => new Tuple<int,int,string>(current.g + ind + 1, d.Item2 - 1, d.Item3 + ind))).ToList()))
    .Sum(r => r.Count));