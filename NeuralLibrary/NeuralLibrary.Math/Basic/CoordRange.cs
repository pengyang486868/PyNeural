namespace NeuralLibrary.Math.Basic
{
    public static class CoordRange
    {
        public static double DistanceOfRange(double min1, double len1, double min2, double len2)
        {

            var min = min1;
            var max = min1 + len1;

            if (min1 > min2)
                min = min2;

            if (min1 + len1 < min2 + len2)   // <
                max = min2 + len2;

            return max - min - len1 - len2;
        }
    }
}
