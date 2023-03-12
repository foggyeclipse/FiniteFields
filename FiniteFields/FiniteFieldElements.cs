namespace FiniteFields;

public class FiniteFieldElements
{
    public int[] Coefficients { get; }
    private FiniteField Field { get; }

    public FiniteFieldElements(int[] coefficients, FiniteField field)
    {
        Coefficients = coefficients;
        Field = field;
    }

    public static FiniteFieldElements operator -(FiniteFieldElements elements)
    {
        var degree = elements.Coefficients.Length;
        var result = new int[degree + 1];
        for (var i = 0; i < degree; i++)
        {
            result[i] = -elements.Coefficients[i];
        }

        return new FiniteFieldElements(result, elements.Field);
    }

    public static FiniteFieldElements operator ~(FiniteFieldElements elements)
        => elements ^ (int) Math.Pow(elements.Field.P, elements.Field.N) - 2;

    public static FiniteFieldElements operator ^(FiniteFieldElements elements, int m)
    {
        var degree = m % ((int) Math.Pow(elements.Field.P, elements.Field.N) - 1);
        switch (m)
        {
            case 0:
                return elements.Field.GetOne();
            case 1:
                return elements;
            default:
                return (elements ^ (degree / 2)) * (elements ^ (degree / 2));
        }
    }

    public static FiniteFieldElements operator +(FiniteFieldElements firstElements,
        FiniteFieldElements secondElements)
    {
        if (firstElements.Field.P != secondElements.Field.P)
        {
            throw new Exception("Both elements should be in the same field");
        }

        var firstLength = firstElements.Coefficients.Length;
        var secondLength = secondElements.Coefficients.Length;
        var maxDegree = firstLength > secondLength ? firstLength : secondLength;

        var result = new int[maxDegree];
        for (var i = 0; i < maxDegree; i++)
        {
            if ((firstElements.Coefficients[i] + secondElements.Coefficients[i]) % firstElements.Field.P < 0)
                result[i] =
                    (firstElements.Coefficients[i] + secondElements.Coefficients[i]) %
                    firstElements.Field.P + firstElements.Field.P;
            else
                result[i] =
                    (firstElements.Coefficients[i] + secondElements.Coefficients[i]) %
                    firstElements.Field.P;
        }

        return new FiniteFieldElements(result, firstElements.Field);
    }

    public static FiniteFieldElements operator -(FiniteFieldElements firstElements,
        FiniteFieldElements secondElements)
    {
        if (firstElements.Field.P != secondElements.Field.P)
        {
            throw new Exception("Both elements should be in the same field");
        }

        var firstLength = firstElements.Coefficients.Length;
        var secondLength = secondElements.Coefficients.Length;
        var maxDegree = firstLength > secondLength ? firstLength : secondLength;

        var result = new int[maxDegree];
        for (var i = 0; i < maxDegree; i++)
        {
            if ((firstElements.Coefficients[i] - secondElements.Coefficients[i]) % firstElements.Field.P < 0)
                result[i] =
                    (firstElements.Coefficients[i] - secondElements.Coefficients[i]) %
                    firstElements.Field.P + firstElements.Field.P;
            else
                result[i] =
                    (firstElements.Coefficients[i] - secondElements.Coefficients[i]) %
                    firstElements.Field.P;
        }

        return new FiniteFieldElements(result, firstElements.Field);
    }

    public static FiniteFieldElements operator *(FiniteFieldElements firstElements,
        FiniteFieldElements secondElements)
    {
        if (firstElements.Field.P != secondElements.Field.P)
        {
            throw new Exception("Both elements should be in the same field");
        }

        var firstLength = firstElements.Coefficients.Length;
        var secondLength = secondElements.Coefficients.Length;
        var maxDegree = firstLength > secondLength ? firstLength : secondLength;
        var minDegree = firstLength < secondLength ? firstLength : secondLength;

        var result = new int[maxDegree + minDegree - 1];
        for (var i = 0; i < maxDegree; i++)
        {
            for (var j = 0; j < minDegree; j++)
            {
                result[i + j] = (result[i + j] + firstElements.Coefficients[i] * secondElements.Coefficients[j]) %
                                firstElements.Field.P;
            }
        }

        FiniteFieldElements qElements = new FiniteFieldElements(new int[firstElements.Field.Q.Length], firstElements.Field);
        FiniteFieldElements resultElements = new FiniteFieldElements(new int[result.Length], firstElements.Field);
        for (var i = 0; i < firstElements.Field.Q.Length; i++)
        {
            qElements.Coefficients[i] = firstElements.Field.Q[i];
        }

        for (var i = 0; i < result.Length; i++)
        {
            resultElements.Coefficients[i] = result[i];
        }

        FiniteFieldElements resultFinal = new FiniteFieldElements(resultElements.Coefficients, resultElements.Field)
                                         % qElements;
        return resultFinal;
    }

    public static FiniteFieldElements operator %(FiniteFieldElements firstElements,
        FiniteFieldElements secondElements)
    {
        if (firstElements.Field.P != secondElements.Field.P)
        {
            throw new Exception("Both elements should be in the same field");
        }

        var firstDegree = firstElements.Coefficients.Length - 1;
        var secondDegree = secondElements.Coefficients.Length - 1;

        var quotient = new int[firstDegree - secondDegree + 1];
        var remainder = firstElements.Coefficients;

        for (var i = firstDegree; i >= secondDegree; i--)
        {
            quotient[i - secondDegree] = firstElements.Coefficients[i] *
                                         (int) Math.Pow(secondElements.Coefficients[secondDegree],
                                             secondElements.Field.P - 2) %
                                         secondElements.Field.P;
            for (var j = secondDegree; j >= 0; j--)
            {
                remainder[i - secondDegree + j] -= secondElements.Coefficients[j] *
                    quotient[i - secondDegree] % secondElements.Field.P;
            }
        }

        Array.Resize(ref remainder, secondDegree);
        while (remainder[remainder.Length - 1] == 0)
        {
            Array.Resize(ref remainder, remainder.Length - 1);
        }

        return new FiniteFieldElements(remainder, firstElements.Field);
    }

    public static FiniteFieldElements operator /(FiniteFieldElements firstElements,
        FiniteFieldElements secondElements)
    {
        if (firstElements.Field.P != secondElements.Field.P)
        {
            throw new Exception("Both elements should be in the same field");
        }

        return firstElements * ~secondElements;
    }
}