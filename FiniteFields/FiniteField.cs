namespace FiniteFields;

public class FiniteField
{
    public int P { get; set; } 
    public int N { get; } 
    public int[] Q { get; }

    public FiniteField(int p, int n, int[] q)
    {
        P = p;
        N = n;
        Q = q;
    }

    public FiniteFieldElements GetZero()
    {
        return new FiniteFieldElements(new int[1] {0}, this);
    }
    
    public FiniteFieldElements GetOne()
    {
        return new FiniteFieldElements(new int[1] {1}, this);
    }

    public int GetDecimalRepresent(FiniteFieldElements element)
    {
        if (P != 2) 
            throw new Exception("The field characteristic must be equal to 2");
        var elementString = string.Join("", element.Coefficients);
        var firstStep = Convert.ToInt32(elementString, 2);
        var secondStep= Convert.ToString(firstStep, 10);
        var thirdStep = Convert.ToInt32(secondStep);
        return thirdStep;
    }
    
    public FiniteFieldElements GetBinaryRepresent(int m)
    {
        if (P != 2) 
            throw new Exception("The field characteristic must be equal to 2");
        var convert = Convert.ToString(m, 2);
        var result = new List<int>();
        foreach (var c in convert)
        { 
            result.Add(Convert.ToInt32(c.ToString()));
        }
        result.Reverse();
        return new FiniteFieldElements(result.ToArray(), this);
    }
    
    public FiniteFieldElements GetFromBinaryRepresent(byte[] bytes)
    {
        if (P != 2) 
            throw new Exception("The field characteristic must be equal to 2");
        var m = BitConverter.ToInt32(bytes, 0);
        return GetBinaryRepresent(m);
    }
    
    public byte[] GetToBinary(FiniteFieldElements element)
    { 
        if (element.Field.P != 2) 
            throw new Exception("The field characteristic must be equal to 2");
        var degree = element.Coefficients.Length;
        
        var result = 0;
        for (var i = element.Coefficients.Length - 1; i >= 0; i--)
        {
            result += element.Coefficients[i] * (int)Math.Pow(element.Field.P, degree++);
        }
        return BitConverter.GetBytes(result);
    }
}