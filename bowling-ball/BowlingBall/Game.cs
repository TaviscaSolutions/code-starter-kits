using BowlingBall.Models;
using BowlingBall.Models.Contracts;
using System;
using System.Collections.Generic;

namespace BowlingBall
{
    public class Game
    {
        private const int LastFrameIndex = 10;
        private const int StrikeBonusFrames = 2;
        private const int SpareBonusFrames = 1;
        private readonly List<IFrame> _frames = new List<IFrame>(10);

        private IFrame _currentContext;

        public void Roll(int pins)
        {
            InitializeContext();

            ProcessContext(pins);

            ResetContext();

            if (_frames.Count >= LastFrameIndex + 1)
            {
                throw new ArgumentOutOfRangeException("Maximum limit reached!");
            }
        }

        public int GetScore()
        {
            var score = 0;

            for (var currentIndex = 0; currentIndex < LastFrameIndex; currentIndex++)
            {
                var frame = _frames[currentIndex];

                var currentFrameScore = frame.GetScore();

                if (frame.FrameType() == FrameType.Strike)
                {
                    score += currentFrameScore + GetStrikeBonus(currentIndex); //STRIKE FORMULA = CurrentThrows Sum + next 2 attempts
                }
                else if (frame.FrameType() == FrameType.Spare)
                {
                    score += currentFrameScore + GetSpareBonus(currentIndex); //Spare FORMULA = CurrentThrows Sum + next 1 attempts
                }
                else
                {
                    score += currentFrameScore;
                }
            }

            return score;
        }

        /// <summary>
        ///     Generate Bonus starting from given frame index till next number of frames to consider
        /// </summary>
        /// <param name="bonusStartIndex"></param>
        /// <param name="numberOfFrames"></param>
        /// <returns></returns>
        public int GetBonus(int bonusStartIndex, int numberOfFrames)
        {
            var sum = 0;

            var previousFrame = _frames[bonusStartIndex - 1];

            if (previousFrame.Bonus.HasValue)
            {
                sum += previousFrame.Bonus.Value;
                --numberOfFrames;
            }

            if (bonusStartIndex == _frames.Count)
            {
                return sum;
            }

            var currentFrame = _frames[bonusStartIndex];

            var isOpenOrSpare = currentFrame.FrameType() == FrameType.Open || currentFrame.FrameType() == FrameType.Spare;
            var bothAttemptsWasStrike = currentFrame.Attempt1.GetValueOrDefault() + currentFrame.Attempt2.GetValueOrDefault() == 20;

            if (isOpenOrSpare || bothAttemptsWasStrike)
            {
                if (numberOfFrames >= 1)
                {
                    sum += currentFrame.Attempt1.GetValueOrDefault();
                    --numberOfFrames;
                }

                if (numberOfFrames >= 1)
                {
                    sum += currentFrame.Attempt2.GetValueOrDefault();
                    --numberOfFrames;
                }

                ++bonusStartIndex;
            }
            else
            {
                sum = currentFrame.GetScore();
                --numberOfFrames;
                ++bonusStartIndex;
            }

            if (numberOfFrames != 0)
            {
                sum += GetBonus(bonusStartIndex, numberOfFrames);
            }

            return sum;
        }

        private void ProcessContext(int pins)
        {
            if (!_currentContext.IsOpen())
            {
                return;
            }

            _currentContext.Roll(pins);
        }

        private void InitializeContext()
        {
            var secondLastFrame = LastFrameIndex - 1;
            if (_frames.Count == secondLastFrame && _currentContext == null)
            {
                _currentContext = new LastFrame();
            }

            if (_currentContext == null)
            {
                _currentContext = new Frame();
            }
        }

        private void ResetContext()
        {
            if (_currentContext.IsOpen())
            {
                return;
            }

            _frames.Add(_currentContext);
            _currentContext = null;
        }

        /// <summary>
        ///     Calculates Strike Bonus of current Frame
        /// </summary>
        /// <param name="currentFrameIndex"></param>
        /// <returns></returns>
        private int GetStrikeBonus(int currentFrameIndex)
        {
            var bonusStartIndex = currentFrameIndex + 1;
            return GetBonus(bonusStartIndex, StrikeBonusFrames);
        }

        /// <summary>
        ///     Calculates Spare Bonus of current Frame
        /// </summary>
        /// <param name="currentFrameIndex"></param>
        /// <returns></returns>
        private int GetSpareBonus(int currentFrameIndex)
        {
            var bonusStartIndex = currentFrameIndex + 1;
            return GetBonus(bonusStartIndex, SpareBonusFrames);
        }


    }
}
