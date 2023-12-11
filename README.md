# adventofcode2023

Days that have been solved so far (both parts are solved unless otherwise specified). The code for each day's solutions is in the PuzzleSolver folder.

## Dec 1

Part 1 was a straightforward scan through each input line to look for digits.

Solving Part 2 entailed scanning through the line to look for words for digits, in addition to digits. One gotcha was to be mindful of 
overlap between digit words. For example, "twone" contains 2 AND 1 and both digits need to be added to the list.

## Dec 2

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

## Dec 10

### Part One:
Writing the code to figure out which tiles were neighbors (i.e. reachable via a pipe) of a current tile was straightforward if slightly tedious. Between the examples
and my puzzle input, there were only two possible kinds of tiles that the start tile could be; instead of writing a function that could handle all conceivable cases,
my code only works for the specific instances that I've been given, so very well may not work for all puzzle inputs.
