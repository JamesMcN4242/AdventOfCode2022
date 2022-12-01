#include "Day1.h"
#include "InputGrabber.h"

#include <numeric>

void Day1::SolvePartOne(string textDir)
{
	vector<int> weighting = FormatInput(textDir);

	int largestWeight = weighting[0];
	for (int i = 1, size = weighting.size(); i < size; ++i)
	{
		if (largestWeight < weighting[i])
		{
			largestWeight = weighting[i];
		}
	}

	cout << "Part One: The most calories carried by an elf is " << largestWeight << endl;
}

void Day1::SolvePartTwo(string textDir)
{
	vector<int> weighting = FormatInput(textDir);

	vector<int> largestWeights;
	int smallestIndex = 0;
	for (int i = 0, size = weighting.size(); i < size; ++i)
	{
		if (largestWeights.size() < 3)
		{
			largestWeights.push_back(weighting[i]);
			continue;
		}

		if (largestWeights[smallestIndex] < weighting[i])
		{
			largestWeights[smallestIndex] = weighting[i];
			smallestIndex = GetSmallestIndex(largestWeights);
		}
	}

	int combinedWeight = accumulate(largestWeights.begin(), largestWeights.end(), 0);
	cout << "Part Two: The combined most calories carried by 3 elves is " << combinedWeight << endl;
}

vector<int> Day1::FormatInput(string textDir)
{
	vector<string> chunks = InputGrabber::GetStringsSeperatedByBlankLine(textDir);
	vector<int> returnValues;

	for (int i = 0, size = chunks.size(); i < size; ++i)
	{
		vector<int> integers = InputGrabber::GetEachInt(chunks[i], '\n');
		returnValues.push_back(accumulate(integers.begin(), integers.end(), 0));
	}

	return returnValues;
}

int Day1::GetSmallestIndex(vector<int>& toCompare)
{
	int smallestIndex = 0;
	for (int i = 1, size = toCompare.size(); i < size; ++i)
	{
		if (toCompare[i] < toCompare[smallestIndex])
		{
			smallestIndex = i;
		}
	}

	return smallestIndex;
}