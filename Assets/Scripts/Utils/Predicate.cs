namespace Utils
{
    public static class Predicate
    {
        public static bool Between(float number, float min, float max)
        {
            return number >= min && number < max;
        }
    }
}