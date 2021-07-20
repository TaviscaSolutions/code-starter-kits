using BowlingBall.Models.Contracts;

namespace BowlingBall.Models
{
    public class Frame : IFrame
    {
        const int MaxPins = 10;
        public int? Attempt1 { get; set; }
        public int? Attempt2 { get; set; }
        public int? Bonus { get; set; }

        public bool IsOpen()
        {
            var attempt1Made = Attempt1.HasValue;
            var attempt2Made = Attempt2.HasValue;

            var bothAttemptDone = attempt1Made && attempt2Made;  //FRAME IS CLOSED WHEN BOTH ATTEMPTS MADE
            var firstStrike = attempt1Made && Attempt1.Equals(MaxPins);  //FRAME IS CLOSED WHEN FIRST IS STRIKE

            return !(bothAttemptDone || firstStrike);
        }

        public FrameType FrameType()
        {
            if (!Attempt1.HasValue)
            {
                return Contracts.FrameType.NotPlayed;
            }

            if (Attempt1.Equals(MaxPins))
            {
                return Contracts.FrameType.Strike;
            }

            var attemptsSum = Attempt1.GetValueOrDefault() + Attempt2.GetValueOrDefault();

            if (attemptsSum == MaxPins)
            {
                return Contracts.FrameType.Spare;
            }

            return Contracts.FrameType.Open;
        }

        public void Roll(int pins)
        {
            if (!Attempt1.HasValue)
            {
                Attempt1 = pins;
                return;
            }
            if (!Attempt2.HasValue)
            {
                Attempt2 = pins;
            }
            //BONUS ATTEMPT IS NOT APPLICABLE FOR NORMAL FRAMES
        }

        public int GetScore()
        {
            return Attempt1.GetValueOrDefault() + Attempt2.GetValueOrDefault();
        }
    }
}
