using BowlingBall.Models;
using BowlingBall.Models.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingBall.Tests
{
    [TestClass]
    public class GameFixture
    {
        [TestMethod]
        public void Gutter_game_score_should_be_zero_test()
        {
            var game = new Game();
            Roll(game, 0, 20);
            Assert.AreEqual(0, game.GetScore());
        }

        [TestMethod]
        [DataRow(data1: 187, moreData: new[] { 10, 9, 1, 5, 5, 7, 2, 10, 10, 10, 9, 0, 8, 2, 9, 1, 10 }, DisplayName = "PROBLEM STATEMENT")]
        [DataRow(data1: 187, moreData: new[] { 10, 9, 1, 5, 5, 7, 2, 10, 10, 10, 9, 0, 8, 2, 9, 1, 10 }, DisplayName = "PROBLEM STATEMENT")]
        [DataRow(data1: 300, moreData: new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, DisplayName = "ALL STRIKES")]
        [DataRow(data1: 20, moreData: new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, DisplayName = "ALL OPEN")]
        [DataRow(data1: 148, moreData: new[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 3 }, DisplayName = "ALL SPARE")]
        [DataTestMethod]
        public void GivenBallScore_WhenPlaying_ThenShouldCalculateExpectedResult(int expectedSum, params int[] ballScores)
        {
            var game = new Game();

            foreach (var score in ballScores)
            {
                game.Roll(score);
            }


            Assert.AreEqual(expectedSum, game.GetScore());
        }

        [DataTestMethod]
        [DataRow(data1: true, moreData: new[] { 4 }, DisplayName = "After Single ball, next turn is allowed")]
        [DataRow(data1: false, moreData: new[] { 4, 2 }, DisplayName = "After Open, No Bonus turn is allowed")]
        [DataRow(data1: true, moreData: new[] { 5, 5 }, DisplayName = "After Spare, Bonus turn is allowed")]
        [DataRow(data1: true, moreData: new[] { 10, 10 }, DisplayName = "After Strike, Bonus turn is allowed")]
        [DataRow(data1: false, moreData: new[] { 10, 10, 4 }, DisplayName = "After Bonus, No turn is allowed")]
        public void GivenLastFrame_WhenCheckingClosed_ShouldReturnGivenResult(bool expectedResult, params int[] ballScores)
        {
            var lastFrame = new LastFrame();
            foreach (var score in ballScores)
            {
                lastFrame.Roll(score);
            }

            Assert.AreEqual(expectedResult, lastFrame.IsOpen());

        }


        [DataTestMethod]
        [DataRow(data1: true, moreData: new[] { 4 }, DisplayName = "After Single ball, next turn is allowed")]
        [DataRow(data1: false, moreData: new[] { 4, 2 }, DisplayName = "After Open, No turn is allowed")]
        [DataRow(data1: false, moreData: new[] { 5, 5 }, DisplayName = "After Spare, no turn is allowed")]
        [DataRow(data1: false, moreData: new[] { 10 }, DisplayName = "After Strike, no turn is allowed")]
        public void GivenFrame_WhenCheckingClosed_ShouldReturnGivenResult(bool expectedResult, params int[] ballScores)
        {
            var frame = new Frame();
            foreach (var score in ballScores)
            {
                frame.Roll(score);
            }
            Assert.AreEqual(expectedResult, frame.IsOpen());
        }

        [DataTestMethod]
        [DataRow(data1: FrameType.Open, moreData: new[] { 4 }, DisplayName = "Should Calculate Frame As Open")]
        [DataRow(data1: FrameType.Open, moreData: new[] { 4, 2 }, DisplayName = "Should Calculate Frame As Open")]
        [DataRow(data1: FrameType.Spare, moreData: new[] { 4, 6 }, DisplayName = "Should Calculate Frame As Spare")]
        [DataRow(data1: FrameType.Strike, moreData: new[] { 10 }, DisplayName = "Should Calculate Frame As Strike")]
        [DataRow(data1: FrameType.NotPlayed, moreData: new int[] { }, DisplayName = "Should Calculate Frame As NotPlayed")]
        public void GivenFrame_WhenCheckingFrameType_ThenShouldCalculateGivenResult(FrameType expectedResult, params int[] ballScores)
        {
            var frame = new Frame();
            foreach (var score in ballScores)
            {
                frame.Roll(score);
            }
            Assert.AreEqual(expectedResult, frame.FrameType());
        }

        [DataTestMethod]
        [DataRow(data1: FrameType.Open, moreData: new[] { 4 }, DisplayName = "Should Calculate Frame As Open")]
        [DataRow(data1: FrameType.Open, moreData: new[] { 4, 2 }, DisplayName = "Should Calculate Frame As Open")]
        [DataRow(data1: FrameType.Spare, moreData: new[] { 4, 6 }, DisplayName = "Should Calculate Frame As Spare")]
        [DataRow(data1: FrameType.Strike, moreData: new[] { 10 }, DisplayName = "Should Calculate Frame As Strike")]
        [DataRow(data1: FrameType.NotPlayed, moreData: new int[] { }, DisplayName = "Should Calculate Frame As NotPlayed")]
        public void GivenLastFrame_WhenCheckingFrameType_ThenShouldCalculateGivenResult(FrameType expectedResult, params int[] ballScores)
        {
            var frame = new LastFrame();
            foreach (var score in ballScores)
            {
                frame.Roll(score);
            }
            Assert.AreEqual(expectedResult, frame.FrameType());
        }

        [DataTestMethod]
        [DataRow(data1: 20, moreData: new[] { 10, 10, 10 }, DisplayName = "Should Calculate bonus 20")]
        [DataRow(data1: 10, moreData: new[] { 10, 5, 5 }, DisplayName = "Should Calculate bonus 10")]
        [DataRow(data1: 0, moreData: new[] { 10, 0, 0 }, DisplayName = "Should Calculate bonus 0")]
        public void GivenFrames_WhenCalculatingStrikeBonus_ThenShouldPopulateGivenResult(int expectedBonus, params int[] ballScores)
        {
            Game game = new Game();
            foreach (var score in ballScores)
            {
                game.Roll(score);
            }

            Assert.AreEqual(expectedBonus, game.GetBonus(1, 2));
        }

        [DataTestMethod]
        [DataRow(data1: 10, moreData: new[] { 10, 10, 10 }, DisplayName = "Should Calculate bonus 10")]
        [DataRow(data1: 5, moreData: new[] { 10, 5, 5 }, DisplayName = "Should Calculate bonus 5")]
        [DataRow(data1: 0, moreData: new[] { 10, 0, 0 }, DisplayName = "Should Calculate bonus 0")]
        public void GivenFrames_WhenCalculatingSpareBonus_ThenShouldPopulateGivenResult(int expectedBonus, params int[] ballScores)
        {
            Game game = new Game();
            foreach (var score in ballScores)
            {
                game.Roll(score);
            }

            Assert.AreEqual(expectedBonus, game.GetBonus(1, 1));
        }

        private void Roll(Game game, int pins, int times)
        {
            for (int i = 0; i < times; i++)
            {
                game.Roll(pins);
            }
        }
    }
}
