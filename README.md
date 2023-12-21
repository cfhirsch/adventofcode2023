# adventofcode2023

What follows are some notes about Days and Parts that have been solved so far. If I don't mention part two on a particular day, it means I haven't solved it yet.

The code for each day's solutions is in the PuzzleSolver folder.

## Dec 1

### Part One:
Part 1 was a straightforward scan through each input line to look for digits.

### Part Two:
Solving Part 2 entailed scanning through the line to look for words for digits, in addition to digits. One gotcha was to be mindful of 
overlap between digit words. For example, "twone" contains 2 AND 1 and both digits need to be added to the list.

## Dec 2

### Part One:
### Part Two:
Solving both parts was straightforward; split each line into the list of samples, use a regex to determine number and color of blocks,
then apply whatever logic was applicable to each part.

## Dec 3

### Part One:
A little more involved than the previous two days. I loaded the input into a char array, then walked through line by line. Booleans keep track of 
whether (i) we are building up a digit and (ii) whether the digit we're building is next to a symbol.

### Part Two:
This time, as I scanned through the lines, I needed to keep track of when there was a neighboring gear. When I was finished parsing 
a number, I updated a dictionary whose keys were gear coordinates and whose values were lists of neighboring numbers.

## Dec 4

### Part One:
Straightforward. Read in each line of input, trim off "Card <card number>:", split into two strings, first containing winning numbers, second the card numbers.
Then, parse each of these strings into two lists of numbers, count the matches.

### Part Two:
In this part I setup a dictionary whose keys are card ids, and whose values are the current numbers of cards with that id. Then I simply iterated through
the input lines, and updated card counts based on the number of winning numbers. Finally I summed up the card counts to get the answer.

## Dec 5

### Part One:
Parsing the input was tedious and I'm sure I could have done that part more elegantly. Once that was done though, the process of mapping inputs to outputs
was straightforward. Specifically, when mapping, say, seed to soil, look for an entry where source_range_start <= seed <= source_range_start + range_length.
If one exists, then return destination_range_start + seed - source_range_start. Otherwise return seed.

I suppose there might be a "fluid" way to do the sequence of seed -> soil -> .. -> location.

### Part Two:
Very pleasantly surprised that I got this in one. Each map is monotonically increasing within its range. So, for example, in the final map from humidity to location,
if location equals, say 80, and there are 10 values left in the current range, then we know that the next 10 values are going to be greater than 80; since
we're looking for the minimum, we don't need to check those values. So what I did here, to avoid having to loop through every 
possible seed value, was to calculate how many values were left in the current range for each map, and take the minimum (ignoring maps where there was no range, 
and we were just returning the passed in value). I then leap forward by that minimum. Glad it worked :).

## Dec 6

### Part One:
Straightforward. For each pair of times and distances, just loop through the resulting distance from holding the button for 0 to time milliseconds.

### Part Two:
Also straightforward. Just removed the spaces between the numbers in each line, parsed to a number, looped through all the possible times to hold the button.

## Dec 7

### Part One:
I created a Card class that stores the hand as a char array, and a dictionary that contains aggregated counts by card. The dictionary is used to determine hand type.
In my first attempt I made a mistake in the code that handles ties in hand type; specifically, I was casting digit chars to the ASCII code and not the digit they 
represented (d'oh!). Once I fixed that I got the right answer.

### Part Two:
Took me two tries to get this right. To calculate the type of a hand that has Jokers, I calculated:

-  The number of Jokers.
- The number of non-Joker ranks in the hand.
- The number of cards in the non-Joker rank that has the highest number of cards.	

If there are at least 4 jokers (JJJJX or JJJJJ, where X is any rank other than Joker), then we have five of a kind. 

If we have 3 jokers, then we need to consider two cases. If there is one non-joker rank in the hand, then the hand is of the form JJJ XX (where X is any rank other than J),
so we have five of a kind. If there are two non-joker types, then the hand is of the form JJJ XY and the best we can do is four of a kind.

If there are 2 jokers, then we have the following cases:

- 1 non-joker type (JJXXX): five of a kind.
- 2 non-joker types (JJXXY): four of a kind.
- 3 non-Joker types (JJXYZ): three of a kind.

If there is 1 joker, then we have the following cases:

- 1 non-joker type (JXXXX): five of a kind.
- 2 non-joker types: Two sub-cases - if the highest number of a non-joker ranked card is 3 (JXXXY), then best we can do is four of a kind. Otherwise (JXXYY), Full House.
- 3 non-joker types (JXXYZ): three of a kind.
- 4 non-joker types (JXYZW): one pair.

## Dec 8

### Part One:
Easy. Just parsed the input and followed the instructions.

### Part Two:
A little more involved. Brute force won't work here (or at least will take too long :)). Suppose there are n starting nodes, and let S_i, 1 <= i <= n be the ith 
starting node. We need to calculate P_i, 1 <= i <= n, where P_i is the number of steps it takes to get from S_i to a node that ends with "Z". The answer is then
LCM(P_1, P_2, ..., P_n), the least common multiple of the P_i's. I had to crib an algorithm for computing the LCD from the internet.

## Dec 9
Both parts were really easy. I'll let the code speak for itself.

Fun fact, the puzzle is about the Method of Differences, used by Charles Babbage in his Difference Engine.

## Dec 10

### Part One:
Writing the code to figure out which tiles were neighbors (i.e. reachable via a pipe) of a current tile was straightforward if slightly tedious. Between the examples
and my puzzle input, there were only two possible kinds of tiles that the start tile could be; instead of writing a function that could handle all conceivable cases,
my code only works for the specific instances that I've been given, so very well may not work for all puzzle inputs.

### Part Two:
Whew! This one took me a while (longer than it should have, honestly). What I ended up doing was expanding the original map by adding new rows between each existing row, 
and then new columns between each existing column. If two tiles were joined in the loop before, the new tile between them will keep them connected, otherwise there will
be a space between them. Thus we can explicitly model the gaps between pipes. Then I went through the boundaries of the map and added any obvious external tiles (i.e.,
any boundary tile that is not in the loop would be external). Finally,
I looked at tiles that I didn't already know are in the loop or external, and for each one, did a breadth first search of tiles reachable from that tile (being reachable
if there is a path of tiles, going either left/right or up/down, none of which are on the loop). If I reach a tile known to be external, then every tile on that path is
marked as external (if it hasn't already). If the search exhausts without reaching an external tile, then this is an internal tile.

I made one of those kinds of programming mistakes that would be obvious if I had an extra pair of eyes, and I initially screwed up the breadth first search; namely not marking tiles
as visited at the right place, which didn't effect the correctness of my code, but caused the queue to explode at one tile. Once I fixed that the right answer was generated 
in a few seconds.

## Dec 11

### Part One:
Initially just expanded the initial input array (well, a list of strings) into the expanded "sky", then calculated distance using Metropolitan distance metric.

### Part Two:
This one forced me to refactor the original code to instead keep track of which rows and columns need to be expanded. Then, to calculate distance,
iterate from minX to maxX, and from minY to maxY, incrementing by 1 or by the inflation factor, depending on whether the row/column needs to be expanded.

## Dec 12

### Part One:
My implementation of this could certainly be more elegant, but I came up with the following recursive formulation of the solution. Here I'll use pseudo-mathy, Haskell-like
notation.

Let C(str, list) be the number of possible arrangements on str, given list. Let's consider base cases first. We first consider the cases where str is empty:

- C("", []) = 1 (There is 1 way to assign zero groups of broken springs to an empty string.)
- C("", x:xs) = 0 (There are 0 ways to assign a non-zero number of groups of broken springs to an empty string.)

Now assume str is not empty:
- C(str, []) = 1 if str does not contain any '#' symbols (we can replace any '?' symbols with '.'), 0 if it does (we're out of groups of broken springs to allocate).
- C(str, x:xs) = 0 if str does not contain enough '#' and '?' symbols to fit all of the groups in x:xs (I could have been tighter in this constraint, but doing so seemed
more error prone than it was worth).

We can now come up with the following recursive formulation of the remaining cases:

- C('.' + str, x:xs) = C(str, x:xs) (i.e. if the first character in the string is '.', then all allocations have to happen in the rest of the string).
- C('#' + str, x:xs) = C(str', xs) if '#' + str is either equal to x instances of '#', or has a prefix of x '#'s, followed by a '?' or '.' (i.e. we can place the first group
of broken springs at the start of the passed in string; if the following character is '?', we replace it with '.'). Here, str' is either the empty string, or is what is left
of '#' + str after removing x '#'s and the subsequent character, which must be '.' or '?'.
- C('#' + str, x:xs) = 0 otherwise (we would have to place the first group of x springs at the start of the string, but we can't, so there are 0 ways to arrange).
- C('?' + str, x:xs) = C('.' + str, x:xs) + C('#' + str, x:xs)

I added a memoization dictionary as an optimization, too lazy to see what effect that had on performance :).

### Part Two:
Apparently my approach in Part One was fast enough to efficiently solve Part Two as well :).

## Dec 13

### Part One:
Not that difficult. I read each map into a char array. Then I went through each column to find potentional pivot points for a reflection, and similarly for each row.

### Part Two:
This part tripped me up for a bit. The insight I was missing was that, for a given mirror, once the smudge was removed, the original AND new reflections could both be present.
So I needed to modify the code from part one; instead of looking for the first reflection (or null if no reflection of the given orientation exists), I needed to return a list of 
reflections. For each mirror I had to check if there was a reflection in the list that didn't match what I found prior to removing the smudge.

## Dec 14

### Part One:
Straightforward. I read input into a char array and moved any rocks "north" as far as they could go by updated the array in place.

### Part Two:
A "find the cycle" problem, which is a common pattern in AdventOfCode puzzles. I created a "visited" dictionary whose keys are the current map configuration seralized into a string, and 
whose values are the cycle at which the given configuration was first encountered. Cycle and add to the visited dictionary until you find a configuration that's been visited before.
Let cycleStart be the cycle at which the given pattern was first encountered. Let cycleLength = current_cycle_number - cycleStart. Then we only need to 
cycle another (1000000000 - cycleStart) % cycleLength times to get the answer.

## Dec 15

### Part One:
Very straightforward. I'll let the code speak for itself.

### Part Two:
Also straightforward, as long as one follows the instructions carefully :).

## Dec 16

### Part One:
Kind of having an off day. The approach is straightforward, I just kept making mistakes with respect to updating the coordinates of moving beams correctly.
I also couldn't figure out how to know when I didn't need to iterate any more; I ended up experimenting with my puzzle input to figure out a minimal number
of steps to run to get the correct answer.

### Part Two:
UPDATE: Duh, I needed to be using a queue and, as with the solution to many of Advent puzzles, a breadth first search. We've visited a node if location and direction
are the same. Once I made those changes I got the solution to part two as well. Although it runs a tiny bit slower than I would like.

## Dec 17

### Part One:
This day took me way longer than it should have. My solution is still slower than I would like (about 5-8 seconds). I ended up using A* search, which, from looking
at other people's solutions, was likely overkill. The node state consists of position, direction of travel, and number of steps already taken in the given direction.
The method to get neighbors tries out turning right. turning left, and, if number of steps taken in current direction is less than three, going foward (subject to bounds
checking of course).

### Part Two:
Again, my solution is much slower than I would like (took a minute or two). But with some tweaks for the new movement rules, my solution found the correct answer For
Part Two as well.

## Dec 18

### Part One:
I ended up using a similar approach to Day 10 to calculate the number of interior tiles. First I collect all the points in the loop. Then I translate them to a char array; 
'.' means we didn't dig here, '#' means we did.
I seed a list of tiles I know are external to the loop. The for each tile in the array, I do a depth first search (I skip this search if I already know the tile is internal
or external). If the tile is connected via a path of '.' tiles to an external tile, than it's external. Otherwise it's internal. Then I could the number of internal tiles.

### Part Two:
My approach in Part One certainly does not scale for this part. I ended up cribbing from https://github.com/mnvr/advent-of-code-2023/blob/main/18.swift; only thing I did here
was do the trivial port into C#.

## Dec 19

### Part One:
Straightforward. Just wrote a parser to follow the rules and got the answer first try.

## Day 20

### Part One:
Fairly straightforward, although it took a little bit of thought to get to using a queue to make sure the signals are sequenced properly. I had to run my puzzle input for 1000
cycles to get the answer; if there's a cycle it's much larger than 1000 button presses.

## Day 21

### Part One:
Tricker than it seemed at first glance. I solved this with breadth-first search, where the nodes to explore were tuples contain a location and the number of steps taken.