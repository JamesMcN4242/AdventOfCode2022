#pragma once

#include <iostream>
#include <string>
#include <vector>

using namespace std;

class Day1 {
public:
	void SolvePartOne(string textDir);
	void SolvePartTwo(string textDir);

private:
	vector<int> FormatInput(string textDir);
	int GetSmallestIndex(vector<int>& toCompare);
};