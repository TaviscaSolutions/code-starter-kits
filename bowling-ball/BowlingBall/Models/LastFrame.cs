using BowlingBall.Models.Contracts;

namespace BowlingBall.Models
{
    public class LastFrame : IFrame
    {
        const int MaxPins = 10;
        const int MaxPlusBonusPins = MaxPins * 2;
        public int? Attempt1 { get; set; }
        public int? Attempt2 { get; set; }
        public int? Bonus { get; set; }

        public bool IsOpen()
        {
            var attempt1Made = Attempt1.HasValue;
            var attempt2Made = Attempt2.HasValue;
            var bonusAttemptMade = Bonus.HasValue;

            if (attempt1Made && attempt2Made && bonusAttemptMade) //FRAME IS CLOSED WHEN BOTH+BONUS ATTEMPTS MADE
            {
                return false;
            }

            if (attempt1Made && attempt2Made)
            {
                var sum = Attempt1 + Attempt2;
                return sum == MaxPins || sum == MaxPlusBonusPins; //FRAME IS CLOSED WHEN BOTH NORMAL ATTEMPTS MADE
            }

            return true;
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
                return;
            }
            if (!Bonus.HasValue)
            {
                Bonus = pins;
            }
        }

        public int GetScore()
        {
            return Attempt1.GetValueOrDefault() + Attempt2.GetValueOrDefault();
        }
    }
}
