using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AdventOfCode2023.Utilities;
using Microsoft.SqlServer.Server;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day5PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            return Solve(test, isPartTwo: false);
        }

        public string SolvePartTwo(bool test = false)
        {
            return Solve(test, isPartTwo: true);
        }

        private static string Solve(bool test = false, bool isPartTwo = false)
        {
            var seedToSoil = new Dictionary<int, int>();
            var seeds = new List<long>();
            Map seedToSoilMap = new Map();
            Map soilToFertilizerMap = new Map();
            Map fertilizerToWaterMap = new Map();
            Map waterToLightMap = new Map();
            Map lightToTemperatureMap = new Map();
            Map temperatureToHumidityMap = new Map();
            Map humidityToLocationMap = new Map();

            var lines = PuzzleReader.ReadLines(5, test).ToList();
            int line = 0;

            while (line < lines.Count)
            {
                string current = lines[line];
                if (current.StartsWith("seeds: "))
                {
                    string seedStr = current.Substring("seeds: ".Length);
                    seeds = seedStr.Split(' ').Select(x => Int64.Parse(x)).ToList();
                    line++;
                    continue;
                }

                if (string.IsNullOrEmpty(current))
                {
                    line++;
                    continue;
                }

                if (current.StartsWith("seed-to-soil map:"))
                {
                    line++;
                    current = lines[line];
                    while (!string.IsNullOrEmpty(current))
                    {
                        var nums = current.Split(' ');
                        seedToSoilMap.MapEntries.Add(new MapEntry
                        {
                            DestinationRangeStart = Int64.Parse(nums[0]),
                            SourceRangeStart = Int64.Parse(nums[1]),
                            RangeLength = Int64.Parse(nums[2])
                        });

                        line++;
                        current = lines[line];
                    }
                }
                else if (current.StartsWith("soil-to-fertilizer map:"))
                {
                    line++;
                    current = lines[line];
                    while (!string.IsNullOrEmpty(current))
                    {
                        var nums = current.Split(' ');
                        soilToFertilizerMap.MapEntries.Add(new MapEntry
                        {
                            DestinationRangeStart = Int64.Parse(nums[0]),
                            SourceRangeStart = Int64.Parse(nums[1]),
                            RangeLength = Int64.Parse(nums[2])
                        });

                        line++;
                        current = lines[line];
                    }
                }
                else if (current.StartsWith("fertilizer-to-water map:"))
                {
                    line++;
                    current = lines[line];
                    while (!string.IsNullOrEmpty(current))
                    {
                        var nums = current.Split(' ');
                        fertilizerToWaterMap.MapEntries.Add(new MapEntry
                        {
                            DestinationRangeStart = Int64.Parse(nums[0]),
                            SourceRangeStart = Int64.Parse(nums[1]),
                            RangeLength = Int64.Parse(nums[2])
                        });

                        line++;
                        current = lines[line];
                    }
                }
                else if (current.StartsWith("water-to-light map:"))
                {
                    line++;
                    current = lines[line];
                    while (!string.IsNullOrEmpty(current))
                    {
                        var nums = current.Split(' ');
                        waterToLightMap.MapEntries.Add(new MapEntry
                        {
                            DestinationRangeStart = Int64.Parse(nums[0]),
                            SourceRangeStart = Int64.Parse(nums[1]),
                            RangeLength = Int64.Parse(nums[2])
                        });

                        line++;
                        current = lines[line];
                    }
                }
                else if (current.StartsWith("light-to-temperature map:"))
                {
                    line++;
                    current = lines[line];
                    while (!string.IsNullOrEmpty(current))
                    {
                        var nums = current.Split(' ');
                        lightToTemperatureMap.MapEntries.Add(new MapEntry
                        {
                            DestinationRangeStart = Int64.Parse(nums[0]),
                            SourceRangeStart = Int64.Parse(nums[1]),
                            RangeLength = Int64.Parse(nums[2])
                        });

                        line++;
                        current = lines[line];
                    }
                }
                else if (current.StartsWith("temperature-to-humidity map:"))
                {
                    line++;
                    current = lines[line];
                    while (!string.IsNullOrEmpty(current))
                    {
                        var nums = current.Split(' ');
                        temperatureToHumidityMap.MapEntries.Add(new MapEntry
                        {
                            DestinationRangeStart = Int64.Parse(nums[0]),
                            SourceRangeStart = Int64.Parse(nums[1]),
                            RangeLength = Int64.Parse(nums[2])
                        });

                        line++;
                        current = lines[line];
                    }
                }
                else if (current.StartsWith("humidity-to-location map:"))
                {
                    line++;
                    current = lines[line];
                    while (line < lines.Count && !string.IsNullOrEmpty(current))
                    {
                        var nums = current.Split(' ');
                        humidityToLocationMap.MapEntries.Add(new MapEntry
                        {
                            DestinationRangeStart = Int64.Parse(nums[0]),
                            SourceRangeStart = Int64.Parse(nums[1]),
                            RangeLength = Int64.Parse(nums[2])
                        });

                        line++;
                        if (line < lines.Count)
                        {
                            current = lines[line];
                        }
                    }
                }
            }

            long minLocation = Int64.MaxValue;
            if (isPartTwo)
            {
                for (int i = 0; i < seeds.Count - 1; i+=2)
                {
                    long seedNum = seeds[i];
                    long seedRange = seeds[i + 1];

                    long j = 0;
                    while (j < seedRange)
                    {
                        long currentSeed = seedNum + j;
                        (long soil, long soilRange) = seedToSoilMap.GetMapValuePartTwo(currentSeed);

                        (long fertilizer, long fertRange) = soilToFertilizerMap.GetMapValuePartTwo(soil);

                        (long water, long waterRange) = fertilizerToWaterMap.GetMapValuePartTwo(fertilizer);

                        (long light, long lightRange) = waterToLightMap.GetMapValuePartTwo(water);

                        (long temperature, long tempRange) = lightToTemperatureMap.GetMapValuePartTwo(light);

                        (long humidity, long humRange) = temperatureToHumidityMap.GetMapValuePartTwo(temperature);

                        (long location, long locRange) = humidityToLocationMap.GetMapValuePartTwo(humidity);

                        if (location < minLocation)
                        {
                            minLocation = location;
                        }

                        long minRange = Min(soilRange, fertRange, waterRange, lightRange, tempRange, humRange, locRange);

                        if (minRange == Int64.MaxValue)
                        {
                            throw new ApplicationException("oops");
                        }

                        j += minRange;
                    }
                }
            }
            else
            {
                foreach (long seed in seeds)
                {
                    long soil = seedToSoilMap.GetMapValue(seed);

                    long fertilizer = soilToFertilizerMap.GetMapValue(soil);

                    long water = fertilizerToWaterMap.GetMapValue(fertilizer);

                    long light = waterToLightMap.GetMapValue(water);

                    long temperature = lightToTemperatureMap.GetMapValue(light);

                    long humidity = temperatureToHumidityMap.GetMapValue(temperature);

                    long location = humidityToLocationMap.GetMapValue(humidity);

                    if (location < minLocation)
                    {
                        minLocation = location;
                    }
                }
            }

            return minLocation.ToString();
        }

        private static long Min(params long[] vals)
        {
            long min = Int64.MaxValue;
            foreach (long val in vals)
            {
                if (val > 0 && val < min)
                {
                    min = val;
                }
            }

            return min;
        }

        private class Map
        {
            public Map()
            {
                this.MapEntries = new List<MapEntry>();
            }

            public List<MapEntry> MapEntries { get; }

            public long GetMapValue(long val)
            {
                var entry = this.MapEntries.FirstOrDefault(
                    m => val >= m.SourceRangeStart && val < m.SourceRangeStart + m.RangeLength);

                if (entry != null)
                {
                    return entry.DestinationRangeStart + val - entry.SourceRangeStart;
                }

                return val;
            }

            public Tuple<long, long> GetMapValuePartTwo(long val)
            {
                var entry = this.MapEntries.FirstOrDefault(
                    m => val >= m.SourceRangeStart && val < m.SourceRangeStart + m.RangeLength);

                if (entry != null)
                {
                    long diff = val - entry.SourceRangeStart;
                    return new Tuple<long, long>(
                        entry.DestinationRangeStart + diff,
                        entry.RangeLength - diff);
                }

                return new Tuple<long, long>(val, 0);
            }
        }

        private class MapEntry
        {
            public long DestinationRangeStart { get; set; }

            public long SourceRangeStart { get; set; }

            public long RangeLength { get; set; }
        }
    }
}
