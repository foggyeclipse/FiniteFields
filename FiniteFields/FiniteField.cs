using System.Runtime.CompilerServices;

namespace FiniteFields;

public class FiniteField
{
    public int P { get; } 
    public int N { get; } 
    public int[] Q { get; }

    public FiniteField(int p, int n, int[] q)
    {
        P = p;
        N = n;
        Q = q;
    }

    public FiniteFieldElement GetZero()
    {
        return new FiniteFieldElement(new[] {0}, this);
    }
    public FiniteFieldElement GetOne()
    {
        return new FiniteFieldElement(new[] {1}, this);
    }

    public FiniteFieldElement GetFiniteFieldRepresent(byte[] bytes)
    {
        if (P != 2) 
            throw new Exception("The field characteristic must be equal to 2");
        var bytesInt = BitConverter.ToInt32(bytes);
        var result = new List<int>();
        for (var i = bytesInt; i > 0; i--)
        {
            result.Add(bytesInt % 2);
            bytesInt /= 2;
        }
        while (result[result.Count - 1] == 0)
        {
            if (result.Count - 1 == 0)
                break;
            result = result.SkipLast(1).ToList();
        }
        return new FiniteFieldElement(result.ToArray(), this);
    }

    public static byte[] GetBinaryRepresent(FiniteFieldElement element)
    {
        if (element.Field.P != 2) 
            throw new Exception("The field characteristic must be equal to 2");
        var result = BitConverter.GetBytes(SchemeHorner(2, element.Coefficients));
        return result;
    }
    public static int SchemeHorner(int x, int[] coefficients)
    {
        var result = coefficients[coefficients.Length-1];
        for (var i = coefficients.Length - 2; i >= 0; i--)
        {
            result = result * x + coefficients[i];
        }
        return result;
    }
}