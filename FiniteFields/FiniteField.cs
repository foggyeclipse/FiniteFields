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

    public FiniteFieldElements GetZero()
    {
        return new FiniteFieldElements(new int[1] {0}, this);
    }

    public FiniteFieldElements GetOne()
    {
        return new FiniteFieldElements(new int[1] {1}, this);
    }
}