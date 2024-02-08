namespace SavingSystem
{
    public class LevelData
    {
        public LevelData(int number, float passedDistance)
        {
            Number = number;
            PassedDistance = passedDistance;
        }

        public int NextAwardIndex { get; set; }
        public int Number { get; set; }
        public float PassedDistance { get; set; }
    }
}