public class GameSettings
{
    public readonly float BallBooster;
    public readonly int PointsToWin;
    public readonly int Difficulty;

    public GameSettings(float ballBooster, float pointsToWin, int difficulty)
    {
        BallBooster = ballBooster;
        PointsToWin = (int)pointsToWin;
        Difficulty = difficulty;
    }
}
