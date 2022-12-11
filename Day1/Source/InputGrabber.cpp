#include "InputGrabber.h"
#include <fstream>

using std::getline;
using std::ifstream;
using std::stoi;

const string InputGrabber::GetTextInput(string fileLocation)
{
	ifstream file(fileLocation);
	std::string str;

	file.seekg(0, std::ios::end);
	str.reserve(file.tellg());
	file.seekg(0, std::ios::beg);

	str.assign((std::istreambuf_iterator<char>(file)),
		std::istreambuf_iterator<char>());

	return str;
}

const vector<string> InputGrabber::GetEachStringLine(string fileAddress)
{
	string input = GetTextInput(fileAddress);
	return GetEachLine(input, '\n');
}

const vector<string> InputGrabber::GetStringsSeperatedByBlankLine(const string fileAddress)
{
	const string inputToBreak = GetTextInput(fileAddress);
	const int inputLen = inputToBreak.size();

	vector<string> entries;
	string line = "";
	line += inputToBreak[0];

	for (int i = 1; i < inputLen; i++)
	{
		line += inputToBreak[i];
		if (i == inputLen - 1 || (inputToBreak[i] == '\n' && inputToBreak[i - 1] == '\n'))
		{
			entries.push_back(line);
			line.clear();
		}
	}

	return entries;
}

const vector<int> InputGrabber::GetEachIntLine(string fileAddress)
{
	string input = GetTextInput(fileAddress);
	return GetEachInt(input, '\n');
}

const vector<int> InputGrabber::GetCommaSeperatedIntLine(string fileAddress)
{
	string input = GetTextInput(fileAddress);
	return GetEachInt(input, ',');
}

const vector<int> InputGrabber::GetEachInt(const string input, const char seperator)
{
	vector<int> entries;
	string line = "";

	for (int i = 0, len = input.length(); i < len; i++)
	{
		bool newLine = false;
		if (input[i] != seperator)
		{
			line += input[i];
		}
		else
		{
			newLine = true;
		}

		if ((newLine || i == len - 1) && line != "")
		{
			entries.push_back(stoi(line));
			line.clear();
		}
	}
	return entries;
}

const vector<string> InputGrabber::GetEachLine(const string input, const char seperator)
{
	vector<string> entries;
	string line = "";

	for (int i = 0, len = input.length(); i < len; i++)
	{
		bool newLine = false;
		if (input[i] != seperator)
		{
			line += input[i];
		}
		else
		{
			newLine = true;
		}

		if ((newLine || i == len - 1) && line != "")
		{
			entries.push_back(line);
			line.clear();
		}
	}
	return entries;
}