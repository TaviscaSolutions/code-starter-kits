using System.Collections.Generic;

namespace BowlingBall
{
    internal class Frame
    {
        /// <summary>
        /// How many pins have been knocked down in each roll
        /// </summary>
        public List<int> RollResults { get; } = new List<int>();

        /// <summary>
        /// No more rolls can be registered on a closed Frame
        /// </summary>
        public bool IsClosed => !isLastFrame && standingPins == 0 ||
                                !isLastFrame && RollResults.Count == 2 ||
                                RollResults.Count == 3;

        private int standingPins;
        private readonly int startingPinCount;
        private readonly bool isLastFrame;

        /// <summary>
        /// Create a new Frame
        /// </summary>
        /// <param name="startingPinCount">Number of pins that the Frame should start with</param>
        /// <param name="isLastFrame">Special rules apply on the last frame</param>
        public Frame(int startingPinCount, bool isLastFrame = false)
        {
            this.startingPinCount = startingPinCount;
            standingPins = startingPinCount;
            this.isLastFrame = isLastFrame;
        }

        /// <summary>
        /// Register the result of a roll
        /// </summary>
        /// <param name="pins">How many pins have been knocked down</param>
        public void RegisterRoll(int pins)
        {
            RollResults.Add(pins);
            standingPins -= pins;
        }
    }
}
