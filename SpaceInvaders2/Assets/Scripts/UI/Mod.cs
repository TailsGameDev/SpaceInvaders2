public class Mod
{
    public static int mod(int dividend, int divisor)
    {
        return (dividend % divisor + divisor) % divisor;
    }
}