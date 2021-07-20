namespace BowlingBall.Models.Contracts
{
    public interface IFrame
    {
        int? Attempt1 { get; set; }
        int? Attempt2 { get; set; }
        int? Bonus { get; set; }
        bool IsOpen();
        FrameType FrameType();
        void Roll(int pins);
        int GetScore();

    }
}
