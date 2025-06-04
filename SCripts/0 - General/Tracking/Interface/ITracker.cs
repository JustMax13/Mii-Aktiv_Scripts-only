using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace General.Tracking
{
    public interface ITracker
    {
        public string NameInJson { get; }
        public Dictionary<int, int> Counters { get; }
        public ITracker ThisTracker { get; }
        public ITrackingSystem TrackingSystem { get; }

        public int CountersSum()
        {
            int sum = 0;
            foreach (var counter in Counters)
                sum += counter.Value;

            return sum;
        }
        private void SimpleAddToCounter(int counterKey) => Counters[counterKey] += 1;
        public void AddToCounter(int counterKey, bool createNewIfIsntFound = true)
        {
            if (Counters.ContainsKey(counterKey))
                SimpleAddToCounter(counterKey);
            else
            {
                if (createNewIfIsntFound)
                    Counters.Add(counterKey, 1);
                else
                    throw new System.Exception("Лічильника з keyCounter = " + counterKey + "немає у списку.");
            }
        }
        public void Instantiate(ITrackingSystem trackingSystem) => trackingSystem.Trackers.Add(this);
        public void Reset()
        {
            var counterKeys = new int[Counters.Count];
            int i = 0;
            foreach (var counter in Counters)
                counterKeys[i++] = counter.Key;

            for (int j = 0; j < Counters.Count; j++)
                Counters[counterKeys[j]] = 0;
        }
    }
}