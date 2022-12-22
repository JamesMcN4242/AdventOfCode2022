# AdventOfCode2022
A repository to hold my solutions to the advent of code puzzles for the year 2022. I think I will create solutions in C++ or C# for most puzzles when the difficulty starts to rise, and maybe do some of the easier days again in some other languages to get a little more comfortable with the syntax. That or I may embrace the python life style.

# Challenges For This Year
- [ ] Complete all 25 challenge days. Due by the 1st of January.
- [ ] Use assembly to solve at least one problem part. Can be done in another language first.
- [ ] Use a brand new language or one I've had minimal interaction with (< 2 hours experience).
- [x] Complete one part in a single line of code (a list of linq queries is acceptable, I just can't make anything new really).
- [ ] Try to visualise one problem in VR or a data graph.

# Day One (C++)
Today I have visitors from Scotland, so my plan of using assembly on the first day is a bit shot for now. As I'd need more time to really get to grips with the very basics I'd need for this challenge. 
I may come back to do the first part at least in assembly later, as it's one of my goals to achieve for this year.

For now, this showed me it's been a while since I've wrote C++ as the file loading seemed an immediate pain, which made me re-use the InputGrabber class I made for AOC two years ago.

# Day Two (C#)
I completed the first part of the problem in one line, purely because of the challenges above. Boi was that a horrible line. More magical than the Harry Potter series.
The second one still uses some ugly ASCII maths, but hey, at least it's out the way quickly.

# Day Three (C#)
Today I embraced the LINQ life. It kind of hurts to know how little care I put into performance, but at least for now performance isn't critical for these tasks. So it's not the worst. 
Definitely doesn't feel like I'm making good use of the Sustainable Software Engineering talks I just attended.

# Day Four (C#)
Could have done both parts in one loop, but the dataset is still really small, so two separate Counts is definitely okay to go for instead.
Today's was really easy, so I probably should have done today's in a different language. Oh well. Mistakes were made.

# Day Five (C#)
The problem itself was really simple, but parsing the input text was a bit more annoying today. 
I'm sure there must be other nice reading ways to do this, but the whole solution to me was just: `Stack`.

# Day Six (C#)
I should really start to use other languages before the puzzles get more difficult. This would have been a great one to quickly do in another language.. But no. I used C# because I was lazy and wanted to get it out the way for the day. Likewise with my other challenges, it's only going to get more difficult if I don't start soon for it.

# Day Seven (C#)
Day seven was another simple to do one, but tedious to setup. I think tomorrow during the national holiday I may visualise this one, as it feels like a nice one to show in a tree building like fashion.
Edit: Visualisation needs to wait for another day, as the holiday has now been claimed to visit my partner's family.

# Day Eight (C#)
I don't think I went about this the best way. But I guess it's clear at this point I never think I've gone the right way with a solution for AoC, even if it completes pretty instantly.
Today since this was completed on a train, I've not taken any time at all to clean it. So there is probably easy to fix segments.

# Day Nine (C#)
This one wasn't as bad as I was expecting it to be. Part two required just extending my solution slightly to work for both parts which was nice. Initially I thought that was going to result in a much bigger change. I'm interested to see what weird and nice mathematical ways people come up with to solve today's puzzle. Doing it interatively was the only thing that came to mind for me... Ohhh. Unless I'd done a Sonic companion like approach where I use the same inputs delayed for the following knots, as opposed to stopping the head for each single step. That seems like more effort though.

# Day Ten (C#)
The only struggle I had with today's puzzle was actually reading it. The logic itself is easy to follow, but how it was worded threw my brain for a loop today. Made 3 or 4 small errors in a row until I read through it all again. Mein Gehirn ist kaputt.

# Day Eleven (C#)
Some real monkey business going on here.... I'll see myself out.
Today was the first day I needed to find a hint for the part 2 solution. In hindsight it was pretty obvious, but isn't something that I need generally in life, so it completely stumped me.

# Day Twelve (C#)
Simple mapping exercise. Reminds me of some old coursework at uni in first or second year. Used a hashset instead of a list of nodes to check, purely to try stop processing the same node twice. Didn't compare timings on this approach though, so List may have been faster. Either way it's pretty instant again, so no harm, no foul.

# Day Thirteen (C#)
Initially I started writing my own parser for this, which was a very rewarding 15 minutes.. And also a terrible idea. I can't believe I didn't notice it was valid json arrays from the get go. Once I noticed that it was pretty easy to do, although my code looks very messy.

# Day Fourteen (C#)
This is another good candidate to visualise, but today for speed I went for just a hashmap implementation which makes it harder (but not impossible) to draw from. I really liked this puzzle, simple, to the point, and a nice concept.

# Day Fifteen (C#)
This one took me a while. The idea I ended up going with for part two was one I tried near the start, but ended up making a mistake and abandoning the approach in favour for trying something else. That lost me a lot of time. 

I really enjoyed thinking about this puzzle though, it's something that forces you not the brute force it. The concept itself once you think about the problem isn't terrible either, but I admit it showed my maths was rusty that it took me so long to implement this correctly for integer positions. And even then I added some safety gaps just in case.

# Day Sixteen (C#)
Can't believe I didn't see the elephant twist coming. Part two, I'm going to rant a bit and give the solution away. So if you don't want that, stop reading now!
Final warning.

Part one gave me confidence in how fast I could generate all possible paths for a singular actor. I did dabble with extending part one initially, but realised quickly that a solution like that would take a long time to complete. So I went back to the concept of storing every possible path and finding the first two highest values that did not overlap. The only thing that shot me in the foot initially was the fact that I did not immediately consider incomplete paths. This meant it wouldn't have worked for either my input or the example input. I'm sure there has to be a nicer way than the excensive memory usage I used for this puzzle. But in terms of memory vs computation speed, this one was by far the better choice.

# Day Seventeen (C#)
After getting part one last night, it should have taken me about ten minutes to do part two. 
However, I named one of my `long`s wrongly when making calculations for skipping rocks. And that led to a lot of confusion for ten-fifteen minutes alone. This meant I lost my lunch break looking at this small problem. What a life.

Part One was the bane of my life, as I had an issue that only occurred in my input and not the sample input, and trying to pin point exactly where that was a nightmare. Did lead to some (now removed) visualisation methods though.

# Day Eighteen (C#)
Today was suspiciously easy. I expected a big last minute twist, but none ever came. It was a nice break from the days before it.

# Day Twenty (C#)
Since I'm a couple days behind but seen this one was significantly quicker to make than the rest I did it out of order. Standard stuff, just a doubly linked list. Could have done it in place with a List<(int currentIndex, int moveValue)> then sort by currentIndex, but I don't know how much benefit that would have even given since we'd still be jumping around memory similar to the linked list is anyway. There is no way the full list would have sit or been indexed nicely on caches.
