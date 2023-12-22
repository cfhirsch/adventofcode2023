using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day20PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            return Solve(test, isPartTwo: false);
        }

        public string SolvePartTwo(bool test = false)
        {
            return Solve(test, isPartTwo: true);
        }

        private static string Solve(bool test, bool isPartTwo)
        {
            var components = new Dictionary<string, Component>();
            var lines = PuzzleReader.ReadLines(20, test).ToList();

            // First pass, get the components.
            foreach (string line in lines)
            {
                string[] lineParts = line.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                string label = lineParts[0].Trim();
                if (label.StartsWith("%"))
                {
                    components.Add(label.Substring(1), new FlipFlop { Label = label.Substring(1) });
                }
                else if (label.StartsWith("&"))
                {
                    components.Add(label.Substring(1), new Conjunction { Label = label.Substring(1) });
                }
                else if (label == "output")
                {
                    components.Add(label, new Output());
                }
                else
                {
                    components.Add(label, new Broadcaster() { Label = label });
                }
            }

            // Second pass, set up the connections.
            foreach (string line in lines)
            {
                string[] lineParts = line.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                string label = lineParts[0].Trim();
                string[] destinations = lineParts[1].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                if (label.StartsWith("%") || (label.StartsWith("&")))
                {
                    label = label.Substring(1);
                }

                Component source = components[label];
                foreach (string dest in destinations)
                {
                    string key = dest.Trim();

                    if (!components.ContainsKey(key))
                    {
                        components.Add(key, new Output { Label = key });
                    }

                    Component destination = components[key];
                    source.DestinationModules.Add(destination);
                    if (destination is Conjunction)
                    {
                        ((Conjunction)destination).Inputs.Add(source.Label, false);
                    }
                }
            }

            if (!isPartTwo)
            {
                long numLowPulses = 0;
                long numHighPulses = 0;
                bool flipFlopOn = true;
                int numButtonPresses = 0;
                while (flipFlopOn && numButtonPresses < 1000)
                {
                    numButtonPresses++;
                    // Press the button.
                    var queue = new Queue<Tuple<Component, Component, bool>>();
                    queue.Enqueue(new Tuple<Component, Component, bool>(null, components["broadcaster"], false));
                    while (queue.Count > 0)
                    {
                        Tuple<Component, Component, bool> current = queue.Dequeue();
                        Component source = current.Item1;
                        Component destiation = current.Item2;
                        bool pulse = current.Item3;
                        if (pulse)
                        {
                            numHighPulses++;
                        }
                        else
                        {
                            numLowPulses++;
                        }

                        foreach (Tuple<Component, bool> tuple in destiation.Pulse(source, pulse))
                        {
                            if (test)
                            {
                                string pulseStr = tuple.Item2 ? "high" : "low";
                                //string sourceStr = (source == null) ? "button" : source.Label;
                                Console.WriteLine($"{destiation.Label} -{pulseStr}-> {tuple.Item1.Label}");
                            }

                            queue.Enqueue(new Tuple<Component, Component, bool>(destiation, tuple.Item1, tuple.Item2));
                        }
                    }

                    flipFlopOn = false;
                    foreach (KeyValuePair<string, Component> kvp in components)
                    {
                        if (kvp.Value is FlipFlop && ((FlipFlop)kvp.Value).State)
                        {
                            flipFlopOn = true;
                            break;
                        }
                    }

                }

                numLowPulses *= (1000 / numButtonPresses);
                numHighPulses *= (1000 / numButtonPresses);
                return (numLowPulses * numHighPulses).ToString();
            }
            else
            {
                var lowPulseDict = new Dictionary<string, long>();

                Component penultimate = components.First(c => c.Value.DestinationModules.Any(m => m.Label == "rx")).Value;

                foreach (Component component in components.Where(c => c.Value.DestinationModules.Any(m => m.Label == penultimate.Label)).Select(kvp => kvp.Value))
                {
                    lowPulseDict[component.Label] = 0;
                }

                long numButtonPresses = 0;
                while (lowPulseDict.Any(kvp => kvp.Value == 0))
                {
                    numButtonPresses++;
                    // Press the button.
                    var queue = new Queue<Tuple<Component, Component, bool>>();
                    queue.Enqueue(new Tuple<Component, Component, bool>(null, components["broadcaster"], false));
                    while (queue.Count > 0)
                    {
                        Tuple<Component, Component, bool> current = queue.Dequeue();
                        Component source = current.Item1;
                        Component destination = current.Item2;
                        bool pulse = current.Item3;

                        if (pulse && source != null && lowPulseDict.ContainsKey(source.Label) && lowPulseDict[source.Label] == 0)
                        {
                            Console.WriteLine($"{source.Label} sends a low pulse after {numButtonPresses} button presses.");
                            lowPulseDict[source.Label] = numButtonPresses;
                        }

                        foreach (Tuple<Component, bool> tuple in destination.Pulse(source, pulse))
                        {
                            queue.Enqueue(new Tuple<Component, Component, bool>(destination, tuple.Item1, tuple.Item2));
                        }
                    }
                }

                long result = 1;
                foreach (KeyValuePair<string, long> kvp in lowPulseDict)
                {
                    result *= kvp.Value;
                }

                return result.ToString();
            }
        }
    }

    internal abstract class Component
    {
        public List<Component> DestinationModules = new List<Component>();

        public string Label { get; set; }

        public abstract IEnumerable<Tuple<Component, bool>> Pulse(Component source, bool pulse);
    }

    internal class Broadcaster: Component
    {
        public override IEnumerable<Tuple<Component, bool>> Pulse(Component source, bool pulse)
        {
            foreach (Component component in this.DestinationModules)
            {
                yield return new Tuple<Component, bool>(component, pulse);
            }
        }
    }

    internal class FlipFlop: Component
    {
        // False = off, true = on.
        private bool state;

        public bool State {  get { return this.state;  } }

        public override IEnumerable<Tuple<Component, bool>> Pulse(Component source, bool pulse)
        {
            if (!pulse)
            {
                this.state = !this.state;
                foreach (Component component in this.DestinationModules)
                {
                    yield return new Tuple<Component, bool>(component, this.state);
                }
            }
        }
    }

    internal class Conjunction: Component
    { 
        public Conjunction()
        {
            this.Inputs = new Dictionary<string, bool>();
        }

        public Dictionary<string, bool> Inputs { get; set; }

        public override IEnumerable<Tuple<Component, bool>> Pulse(Component source, bool pulse)
        {
            this.Inputs[source.Label] = pulse;

            bool output = !this.Inputs.Values.All(x => x);
            
            foreach (Component component in this.DestinationModules)
            {
                yield return new Tuple<Component, bool> (component, output);
            }
        }

    }

    internal class Output : Component
    {
        public override IEnumerable<Tuple<Component, bool>> Pulse(Component source, bool pulse)
        {
            return Enumerable.Empty<Tuple<Component, bool>>();
        }
    }
}
