using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InterviewPractice
{
    public class Solution {
        public static void Main() {
            if(!File.Exists("input.txt")) {
                Console.WriteLine("input.txt does not exists!!!");
                return ;    
            }

            // creating the same instance of running class
            var solver = new Solution();
            solver.Solve();
        }

        public void Solve() {
            using var reader = new StreamReader("input.txt");
            using var writer = new StreamWriter("output.txt");

            if(!int.TryParse(reader.ReadLine(), out int k)) return; // if k is not there in input return 

            List<int[]> sortedArrays = new List<int[]>();

            for(int i = 0; i < k; i++ ) {
                string line = reader.ReadLine();
                if(line == null) break;

                int[] arr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

                Array.Sort(arr, (a,b) => a.CompareTo(b)); // ascending order
                sortedArrays.Add(arr);
            }

            // create a priority Queue having value, array index , element index 
            var pq = new PriorityQueue<(int Value, int ArrayIdx, int ElementIdx), int >(Comparer<int>.Create((a,b) => a.CompareTo(b)));
            // pq is minHeap

            int[] currentElements = new int[k];
            int currentMax = int.MinValue;

            // add the first elements into the currentElements array as well as in minheap
            for(int i = 0; i < k; i++) {
                int value = sortedArrays[i][0];
                currentElements[i] = value;
                pq.Enqueue((value, i, 0), value);
                currentMax = Math.Max(currentMax, value);
            }

            int[] bestSet = new int[k];
            int minRange = int.MaxValue;

            while(pq.Count == k) {
                var current = pq.Dequeue();

                int currentMin = current.Value;
                if(currentMax - currentMin < minRange) {
                    minRange = currentMax - currentMin;
                    // copy currentElements array to bestSet for k elements
                    Array.Copy(currentElements, bestSet, k);
                }

                int nextIdx = current.ElementIdx + 1;
                if(nextIdx < sortedArrays[current.ArrayIdx].Length) {
                    int nextVal = sortedArrays[current.ArrayIdx][nextIdx];
                    currentMax = Math.Max(nextVal, currentMax);
                    currentElements[current.ArrayIdx] = nextVal;
                    pq.Enqueue((nextVal, current.ArrayIdx, nextIdx), nextVal);
                } else {
                    break;
                }
            }

            writer.WriteLine($"Minimum Difference {minRange}");
            writer.WriteLine($"Selected Set: {string.Join(", ", bestSet)}");
            Console.WriteLine("Output has been updated to output.txt");
        }
    }
}