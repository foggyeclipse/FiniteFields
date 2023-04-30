namespace FiniteFields;

public class FiniteFieldElement
{
    public int[] Coefficients { get; set; }
    public FiniteField Field { get; set; }

    public FiniteFieldElement(int[] coefficients, FiniteField field) 
    {   
        if (field.N - coefficients.Length < 0)
            throw new Exception("The degree of the polynomial does not match the given field");
        Coefficients = coefficients;
        Field = field;
    }
    
    public static FiniteFieldElement operator -(FiniteFieldElement element)
    {
        var degree = element.Coefficients.Length;
        var result = new int[degree];
        for (var i = 0; i < degree; i++)
        {
            if ((-element.Coefficients[i]) % element.Field.P < 0)
                result[i] =
                    (-element.Coefficients[i])  %
                    element.Field.P + element.Field.P;
            else
                result[i] =
                    (-element.Coefficients[i])  %
                    element.Field.P;
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
        
        var result = new int[maxDegree];
        if (firstElement.Coefficients.Length == maxDegree)
            Array.Copy(firstElement.Coefficients, result, maxDegree);
        else 
            Array.Copy(secondElement.Coefficients, result, maxDegree);
        for (var i = 0; i < minDegree; i++)
        {
            result[i] = (firstElement.Coefficients[i] + secondElement.Coefficients[i]) % firstElement.Field.P;
        }
        while (result[^1] == 0)
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

        var result = new int[maxDegree];
        if (firstElement.Coefficients.Length == maxDegree)
            Array.Copy(firstElement.Coefficients, result, maxDegree);
        else 
            Array.Copy(secondElement.Coefficients, result, maxDegree);
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
        while (result[^1] == 0)
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
        return new FiniteFieldElement(DivisionRemainder
            (result, firstElement.Field.Q, firstElement.Field), firstElement.Field);
    }
    private static int[] DivisionRemainder(int[] firstElement,
        int[] secondElement, FiniteField field)
    {
        if (secondElement.Length > firstElement.Length)
            return firstElement;
        var firstDegree = firstElement.Length - 1;
        var secondDegree = secondElement.Length - 1;

        var quotient = new int[firstDegree - secondDegree + 1];
        var remainder = firstElement;
        for (var i = firstDegree; i >= secondDegree; i--)
        {
            if (firstElement[i] * (int) Math.Pow(secondElement[secondDegree], field.P - 2)
                 % field.P < 0)
                quotient[i - secondDegree] = firstElement[i] 
                                             * (int) Math.Pow(secondElement[secondDegree], field.P - 2)
                                             % field.P + field.P;
            else
                quotient[i - secondDegree] = firstElement[i] 
                                             * (int) Math.Pow(secondElement[secondDegree], field.P - 2)
                                             % field.P;
            for (var j = secondDegree; j >= 0; j--)
            {
                if ((remainder[i - secondDegree + j] - secondElement[j] * quotient[i - secondDegree]) 
                    % field.P < 0)
                    remainder[i - secondDegree + j] = (remainder[i - secondDegree + j] - secondElement[j] * quotient[i - secondDegree])
                        % field.P + field.P;
                else 
                    remainder[i - secondDegree + j] = (remainder[i - secondDegree + j] - secondElement[j] * quotient[i - secondDegree])
                                                      % field.P;
            }
        }
        Array.Resize(ref remainder, secondDegree);
        while (remainder[^1] == 0)
        {
            if (remainder.Length - 1 == 0)
                break;
            Array.Resize(ref remainder, remainder.Length - 1);
        }
        return remainder;
    }
    public static FiniteFieldElement operator /(FiniteFieldElement firstElement,
        FiniteFieldElement secondElement)
    {
        if (firstElement.Field.P != secondElement.Field.P)
            throw new Exception("Both elements should be in the same field");
        return firstElement * ~secondElement;
    }
}