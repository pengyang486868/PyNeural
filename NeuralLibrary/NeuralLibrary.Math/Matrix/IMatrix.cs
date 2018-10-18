namespace NeuralLibrary.Math.Matrix
{
    public interface IMatrix
    {
        int Row { get; set; }
        int Column { get; set; }
        double this[int row, int column] { get; set; }
    }
}
