namespace SavingSystem
{
    public class LevelData
    {
        public LevelData(string name, float passedDistance)
        {
            Name = name;
            PassedDistance = passedDistance;
        }

        public int NextAwardIndex { get; set; }
        public string Name { get; set; }
        public float PassedDistance { get; set; }
        public bool ComicWasShown { get; set; }
    }
}