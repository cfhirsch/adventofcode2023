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
            var seedToSoil = new Dictionary<int, int>();
            var parseState = ParseState.Seeds;
            var seeds = new HashSet<long>();
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
                    seeds = seedStr.Split(' ').Select(x => Int64.Parse(x)).ToHashSet();
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

            return minLocation.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
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
        }

        private class MapEntry
        {
            public long DestinationRangeStart { get; set; }

            public long SourceRangeStart { get; set; }

            public long RangeLength { get; set; }
        }

        private enum ParseState
        {
            Seeds,
            SeedToSoil,
            SoilToFertilizer,
            FertilizerToWater,
            WaterToLight,
            LightToTemperature,
            TemperatureToHumidity,
            HumidityToLocation
        }
    }
}
