namespace FiniteFields;

public class FiniteFieldElement
{
    public int[] Coefficients { get; private set; }
    public FiniteField Field { get; }

    public FiniteFieldElement(int[] coefficients, FiniteField field) 
    {   
        if (field.N + 1 - coefficients.Length < 0)
            throw new Exception("The degree of the polynomial does not match the given field");
        Coefficients = coefficients;
        Field = field;
    }
    
    public static FiniteFieldElement operator -(FiniteFieldElement element)
    {
        var degree = element.Coefficients.Length;
        var result = new int[degree + 1];
        for (var i = 0; i < degree; i++)
        {
            result[i] = -element.Coefficients[i];
        }
        return new FiniteFieldElement(result, element.Field);
    }
    public static FiniteFieldElement operator ~(FiniteFieldElement element)
        => element ^ ((int)Math.Pow(element.Field.P, element.Field.N) - 2);
    public static FiniteFieldElement operator ^(FiniteFieldElement element, int m)
    {
        var degree = m % ((int)Math.Pow(element.Field.P, element.Field.N) - 1);
        var flagParity= degree % 2 == 0;
        
        if (flagParity)
        {
            var degreeTemporary = element ^ (degree / 2);
            return degree switch
            {
                0 => element.Field.GetOne(),
                _ => degreeTemporary * degreeTemporary
            };
        }
        else
        {
            return degree switch
            {
                1 => element,
                _ => element * (element ^ (degree - 1))
            };
        }
    }
    public static FiniteFieldElement operator +(FiniteFieldElement firstElement,
        FiniteFieldElement secondElement)
    {
        if (firstElement.Field.P != secondElement.Field.P)
            throw new Exception("Both elements should be in the same field");
        var firstLength = firstElement.Coefficients.Length;
        var secondLength = secondElement.Coefficients.Length;
        var maxDegree = firstLength > secondLength ? firstLength : secondLength;
        var minDegree = firstLength < secondLength ? firstLength : secondLength;
        
        int[] result;
        _ = firstElement.Coefficients.Length == maxDegree
            ? result = firstElement.Coefficients
            : result = secondElement.Coefficients;
        for (var i = 0; i < minDegree; i++)
        {
            result[i] = (firstElement.Coefficients[i] + secondElement.Coefficients[i]) % firstElement.Field.P;
        }
        while (result[result.Length - 1] == 0)
        {
            if (result.Length - 1 == 0)
                break;
            Array.Resize(ref result, result.Length - 1);
        }
        return new FiniteFieldElement(result, firstElement.Field);
    }
    public static FiniteFieldElement operator -(FiniteFieldElement firstElement,
        FiniteFieldElement secondElement)
    {
        if (firstElement.Field.P != secondElement.Field.P)
            throw new Exception("Both elements should be in the same field");
        var firstLength = firstElement.Coefficients.Length;
        var secondLength = secondElement.Coefficients.Length;
        var maxDegree = firstLength > secondLength ? firstLength : secondLength;
        var minDegree = firstLength < secondLength ? firstLength : secondLength;

        int[] result;
        _ = firstElement.Coefficients.Length == maxDegree
            ? result = firstElement.Coefficients
            : result = secondElement.Coefficients;
        for (var i = 0; i < minDegree; i++)
        {
            if ((firstElement.Coefficients[i] - secondElement.Coefficients[i]) % firstElement.Field.P < 0)
                result[i] =
                    (firstElement.Coefficients[i] - secondElement.Coefficients[i]) %
                    firstElement.Field.P + firstElement.Field.P;
            else
                result[i] =
                    (firstElement.Coefficients[i] - secondElement.Coefficients[i]) %
                    firstElement.Field.P;
        }
        while (result[result.Length - 1] == 0)
        {
            if (result.Length - 1 == 0)
                break;
            Array.Resize(ref result, result.Length - 1);
        }
        return new FiniteFieldElement(result, firstElement.Field);
    }
    public static FiniteFieldElement operator *(FiniteFieldElement firstElement,
        FiniteFieldElement secondElement)
    {
        if (firstElement.Field.P != secondElement.Field.P)
            throw new Exception("Both elements should be in the same field");
        var firstLength = firstElement.Coefficients.Length;
        var secondLength = secondElement.Coefficients.Length;
        var maxDegree = firstLength > secondLength ? firstLength : secondLength;
        var minDegree = firstLength < secondLength ? firstLength : secondLength;
        
        var maxElement = new FiniteFieldElement(new int[maxDegree], firstElement.Field);
        var minElement = new FiniteFieldElement(new int[minDegree], firstElement.Field);
        if (firstLength >= secondLength)
        {
            maxElement.Coefficients = firstElement.Coefficients;
            minElement.Coefficients = secondElement.Coefficients;
        }
        else 
        {
            maxElement.Coefficients = secondElement.Coefficients;
            minElement.Coefficients = firstElement.Coefficients;
        }
        
        var result = new int[maxDegree + minDegree - 1];
        for (var i = maxDegree - 1; i >= 0; i--)
        {
            for (var j = minDegree - 1; j >= 0; j--)
            {
                if ((result[i + j] + maxElement.Coefficients[i] * minElement.Coefficients[j]) 
                     % firstElement.Field.P < 0)
                    result[i + j] = (result[i + j] + maxElement.Coefficients[i] * minElement.Coefficients[j]) 
                                    % maxElement.Field.P + maxElement.Field.P;
                else
                    result[i + j] = (result[i + j] + maxElement.Coefficients[i] * minElement.Coefficients[j]) 
                                    % maxElement.Field.P;
            }
        }
        return result % new FiniteFieldElement(firstElement.Field.Q, firstElement.Field);
    }
    public static FiniteFieldElement operator %(int[] firstElement,
        FiniteFieldElement secondElement)
    {
        if (secondElement.Coefficients.Length > firstElement.Length)
            return new FiniteFieldElement(firstElement, secondElement.Field);
        var firstDegree = firstElement.Length - 1;
        var secondDegree = secondElement.Coefficients.Length - 1;

        var quotient = new int[firstDegree - secondDegree + 1];
        var remainder = firstElement;
        for (var i = firstDegree; i >= secondDegree; i--)
        {
            if (firstElement[i] * (int) Math.Pow(secondElement.Coefficients[secondDegree], secondElement.Field.P - 2)
                 % secondElement.Field.P < 0)
                quotient[i - secondDegree] = firstElement[i] 
                                             * (int) Math.Pow(secondElement.Coefficients[secondDegree], secondElement.Field.P - 2)
                                             % secondElement.Field.P + secondElement.Field.P;
            else
                quotient[i - secondDegree] = firstElement[i] 
                                             * (int) Math.Pow(secondElement.Coefficients[secondDegree], secondElement.Field.P - 2)
                                             % secondElement.Field.P;
            for (var j = secondDegree; j >= 0; j--)
            {
                if ((remainder[i - secondDegree + j] - secondElement.Coefficients[j] * quotient[i - secondDegree]) 
                    % secondElement.Field.P < 0)
                    remainder[i - secondDegree + j] = (remainder[i - secondDegree + j] - secondElement.Coefficients[j] * quotient[i - secondDegree])
                        % secondElement.Field.P + secondElement.Field.P;
                else 
                    remainder[i - secondDegree + j] = (remainder[i - secondDegree + j] - secondElement.Coefficients[j] * quotient[i - secondDegree])
                                                      % secondElement.Field.P;
            }
        }
        Array.Resize(ref remainder, secondDegree);
        while (remainder[remainder.Length - 1] == 0)
        {
            if (remainder.Length - 1 == 0)
                break;
            Array.Resize(ref remainder, remainder.Length - 1);
        }
        return new FiniteFieldElement(remainder, secondElement.Field);
    }
    public static FiniteFieldElement operator /(FiniteFieldElement firstElement,
        FiniteFieldElement secondElement)
    {
        if (firstElement.Field.P != secondElement.Field.P)
            throw new Exception("Both elements should be in the same field");
        return firstElement * ~secondElement;
    }
}