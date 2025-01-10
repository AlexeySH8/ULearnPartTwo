using System.Numerics;

namespace Tickets;

public class TicketsTask
{
    public static BigInteger Solve(int halfLen, int totalSum)
    {
        if (totalSum % 2 != 0) return 0;

        var halfSum = totalSum / 2;
        var opt = new BigInteger[halfLen + 1, halfSum + 1];
        for (int i = 1; i <= halfLen; i++) opt[i, 0] = 1;
        for (int i = 0; i <= halfSum; i++) opt[0, i] = 0;

        for (int i = 1; i <= halfLen; i++)
            for (int j = 1; j <= halfSum; j++)
            {
                if (j > i * 9)
                    opt[i, j] = 0;
                else
                {
                    opt[i, j] = opt[i - 1, j]
                              + opt[i, j - 1]
                              - (j > 9 ? opt[i - 1, j - 10] : 0);
                }
            }
        return opt[halfLen, halfSum] * opt[halfLen, halfSum];
    }
}
