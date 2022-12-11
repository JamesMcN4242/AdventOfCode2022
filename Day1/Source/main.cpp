// C++Solution.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "Day1.h"

void RunDayOne();

int main()
{
    RunDayOne();
}

void RunDayOne()
{
    Day1 day1;
    const string inputDir = "Input/Day1.txt";

    day1.SolvePartOne(inputDir);
    day1.SolvePartTwo(inputDir);
}