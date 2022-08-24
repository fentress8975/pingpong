public class GameSettings
{
    public readonly float BallBooster;
    public readonly int PointsToWin;

    public GameSettings(float ballBooster, float pointsToWin)
    {
        BallBooster = ballBooster;
        PointsToWin = (int)pointsToWin;
    }
}
