# adventofcode2023

Days that have been solved so far (both parts are solved unless otherwise specified).

- Dec 1

Part 1 was a straightforward scan through each input line to look for digits.

Solving Part 2 entailed scanning through the line to look for words for digits, in addition to digits. One gotcha was to be mindful of 
overlap between digit words. For example, "twone" contains 2 AND 1 and both digits need to be added to the list.

- Dec 2

Solving both parts was straightforward; split each line into the list of samples, use a regex to determine number and color of blocks,
then apply whatever logic was applicable to each part.
