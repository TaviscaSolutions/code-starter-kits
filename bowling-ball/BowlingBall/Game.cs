using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingBall
{
    public class Game
    {
        private readonly List<Frame> frames = new List<Frame>();
        private int score;

        /// <summary>
        /// Number of pins that each Frame should start with
        /// </summary>
        private const int StartingPinCount = 10;

        /// <summary>
        /// Maximum number of Frames allowed in this game
        /// </summary>
        private const int MaxFrameCount = 10;

        public void Roll(int pins)
        {
            // Add your logic here. Add classes as needed.
            if (frames.Count == MaxFrameCount && frames.Last().IsClosed)
            {
                throw new InvalidOperationException("You've played enough for today! Consider calling Score()");
            }

            if (!frames.Any() || frames.Last().IsClosed)
            {
                var isLastFrame = frames.Count == MaxFrameCount - 1;
                frames.Add(new Frame(StartingPinCount, isLastFrame));
            }

            frames.Last().RegisterRoll(pins);
        }

        public int GetScore()
        {
            // Returns the final score of the game.
            for (var frameIndex = 0; frameIndex < frames.Count; frameIndex++)
            {
                var frame = frames[frameIndex];
                var frameScore = 0;
                var bonusScore = 0;
                var isStrike = false;

                // cap the roll index to 2 to avoid over-counting points if the last frame has bonus rolls
                var maxRollIndex = frame.RollResults.Count < 2 ? frame.RollResults.Count : 2;

                for (var rollIndex = 0; rollIndex < maxRollIndex; rollIndex++)
                {
                    var result = frame.RollResults[rollIndex];
                    frameScore += result;

                    // calculate bonus score for a strike
                    if (result == StartingPinCount)
                    {
                        isStrike = true;

                        // look 2 rolls ahead
                        bonusScore += CalculateBonusScore(frameIndex, rollIndex, 2);
                        break;
                    }
                }

                // calculate bonus score for a spare
                if (!isStrike && frameScore == StartingPinCount)
                {
                    // look 1 roll ahead
                    bonusScore += CalculateBonusScore(frameIndex, maxRollIndex - 1, 1);
                }

                score += frameScore + bonusScore;
            }

            return score;
        }

        /// <summary>
        /// Recursive function to calculate the bonus score of the next X rolls
        /// </summary>
        /// <param name="frameIndex">Index of the current frame</param>
        /// <param name="rollIndex">Index of the current roll</param>
        /// <param name="rollCount">How many rolls to look ahead</param>
        /// <returns>The amount of bonus score calculated from the next X rolls</returns>
        private int CalculateBonusScore(int frameIndex, int rollIndex, int rollCount)
        {
            if (rollCount == 0)
            {
                return 0;
            }

            var bonusPoints = 0;

            // add the next roll in the same frame, if any
            if (frames[frameIndex].RollResults.Count > rollIndex + 1)
            {
                bonusPoints += frames[frameIndex].RollResults[rollIndex + 1];
                bonusPoints += CalculateBonusScore(frameIndex, rollIndex + 1, rollCount - 1);
            }
            else
            {
                // add the first roll of the next frame, if any
                if (frames.Count > frameIndex + 1)
                {
                    bonusPoints += frames[frameIndex + 1].RollResults[0];
                    bonusPoints += CalculateBonusScore(frameIndex + 1, 0, rollCount - 1);
                }
            }

            return bonusPoints;
        }
    }
}
