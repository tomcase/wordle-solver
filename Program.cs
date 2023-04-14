var wordList = (await File.ReadAllLinesAsync("wordleList.txt")).ToList();
var letterList = "abcdefghijklmnopqrstuvwxyz".ToList();
const int guesses = 6;

for (var y = 0; y < guesses; y++)
{
	var (guess, mask) = GetGuess();
	wordList = FilterByMask(wordList, guess, mask);
	foreach (var word in wordList.ToList())
	{
		foreach (var letter in word)
		{
			if (!letterList.Contains(letter))
			{
				wordList.Remove(word);
			}
		}
	}

	Console.WriteLine($"Narrowed it down to: {wordList.Count} words:");

	foreach (var word in wordList.Take(5))
	{
		Console.WriteLine($"{word}");
	}
}

List<string> FilterByMask(List<string> wordList, string guess, string mask)
{
	var newList = wordList;
	for (var i = 0; i < 5; i++)
	{
		// Remove wronguns
		if (mask[i] == '-' || mask[i] == 'o')
		{
			var wrongLetter = guess.ElementAt(i);
			foreach (var word in newList.ToList())
			{
				if (word.ElementAt(i) == wrongLetter)
				{
					newList.Remove(word);
				}

				if (mask[i] == '-')
				{
					letterList.Remove(wrongLetter);
				}

				if (mask[i] == 'o')
				{
					if (!word.Contains(wrongLetter))
					{
						newList.Remove(word);
					}
				}
			}
		}

		// Include right uns
		if (mask[i] == 'x')
		{
			var rightLetter = guess.ElementAt(i);
			foreach (var word in newList.ToList())
			{
				if (word.ElementAt(i) != rightLetter)
				{
					newList.Remove(word);
				}
			}
		}
	}

	return newList;
}

(string, string) GetGuess()
{
	Console.WriteLine("What was your guess?");
	var guess = (Console.ReadLine() ?? string.Empty).ToLowerInvariant();
	Console.WriteLine("What was the mask? ('X': Correct, 'O': Semi-Correct, '-': Wrong)");
	var mask = (Console.ReadLine() ?? string.Empty).ToLowerInvariant();
	return (guess, mask);
}
