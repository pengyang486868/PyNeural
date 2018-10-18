namespace NeuralLibrary.Core
{
    public class TrainingParam
    {
        public int Id { get; set; }
        public string WorkingDir { get; set; }
        public double Speed { get; set; }
        public int MaxTime { get; set; }
        public double Tolerence { get; set; }

        public TrainingParam()
        {
            Id = 99;
            WorkingDir = @"D:\";
            Speed = 0.05;
            MaxTime = 10000;
            Tolerence = 0.001;
        }
    }
}
