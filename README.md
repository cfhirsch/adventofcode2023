# adventofcode2023

Days that have been solved so far (both parts are solved unless otherwise specified).

- Dec 1

Part 1 was a straightforward scan through each input line to look for digits.

Solving Part 2 entailed scanning through the line to look for words for digits, in addition to digits. One gotcha was to be mindful of 
overlap between digit words. For example, "twone" contains 2 AND 1 and both digits need to be added to the list.

- Dec 2

Solving both parts was straightforward; split each line into the list of samples, use a regex to determine number and color of blocks,
then apply whatever logic was applicable to each part.

- Dec 3

-- Part One:
A little more involved than the previous two days. I loaded the input into a char array, then walked through line by line. Booleans keep track of 
whether (i) we are building up a digit and (ii) whether the digit we're building is next to a symbol.

-- Part Two:
This time, as I scanned through the lines, I needed to keep track of when there was a neighboring gear. When I was finished parsing 
a number, I updated a dictionary whose keys were gear coordinates and whose values were lists of neighboring numbers.

- Dec 4

-- Part One:
Straightforward. Read in each line of input, trim off "Card <card number>:", split into two strings, first containing winning numbers, second the card numbers.
Then, parse each of these strings into two lists of numbers, count the matches.

-- Part Two:
In this part I setup a dictionary whose keys are card ids, and whose values are the current numbers of cards with that id. Then I simply iterated through
the input lines, and updated card counts based on the number of winning numbers. Finally I summed up the card counts to get the answer.

- Dec 5

-- Part One:
Parsing the input was tedious and I'm sure I could have done that part more elegantly. Once that was done though, the process of mapping inputs to outputs
was straightforward. Specifically, when mapping, say, seed to soil, look for an entry where source_range_start <= seed <= source_range_start + range_length.
If one exists, then return destination_range_start + seed - source_range_start. Otherwise return seed.

I suppose there might be a "fluid" way to do the sequence of seed -> soil -> .. -> location.
