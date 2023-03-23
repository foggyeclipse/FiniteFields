using FiniteFields;
using NUnit.Framework;

namespace FiniteFieldsTests;

public class Tests
{
    [Test]
    public void Addition1()
    {
        var GF9 = new FiniteField(3, 2, new[] { 1, 1, 2 });
        var element1 = new FiniteFieldElement(new[] { 0, 2, 2 }, GF9);
        var element2 = new FiniteFieldElement(new[] { 1, 1 }, GF9);
        var addition = element1 + element2;
        Assert.That(addition.Coefficients, Is.EqualTo(new[] { 1, 0, 2 }));
    }

    [Test]
    public void Addition2()
    {
        var GF4 = new FiniteField(2, 2, new[] { 1, 1, 1 });
        var element1 = new FiniteFieldElement(new[] { 1, 1 }, GF4);
        var element2 = new FiniteFieldElement(new[] { 1 }, GF4);
        var addition = element1 + element2;
        Assert.That(addition.Coefficients, Is.EqualTo(new[] { 0, 1 }));
    }

    [Test]
    public void Subtract1()
    {
        var GF9 = new FiniteField(3, 2, new[] { 1, 1, 2 });
        var element1 = new FiniteFieldElement(new[] { 2, 2, 1 }, GF9);
        var element2 = new FiniteFieldElement(new[] { 1, 1 }, GF9);
        var subtract = element1 - element2;
        Assert.That(subtract.Coefficients, Is.EqualTo(new[] { 1, 1, 1 }));
    }

    [Test]
    public void Subtract2()
    {
        var GF4 = new FiniteField(2, 2, new[] { 1, 1, 1 });
        var element1 = new FiniteFieldElement(new[] { 1, 1, 1 }, GF4);
        var element2 = new FiniteFieldElement(new[] { 0, 1 }, GF4);
        var subtract = element1 - element2;
        Assert.That(subtract.Coefficients, Is.EqualTo(new[] { 1, 0, 1 }));
    }

    [Test]
    public void Multiplication1()
    {
        var GF16 = new FiniteField(2, 4, new[] { 1, 1, 1, 1, 1 });
        var element1 = new FiniteFieldElement(new[] { 1, 0, 1, 1 }, GF16);
        var element2 = new FiniteFieldElement(new[] { 1, 0, 0, 1 }, GF16);
        var multiplication = element1 * element2;
        Assert.That(multiplication.Coefficients, Is.EqualTo(new[] { 0, 1, 1 }));
    }

    [Test]
    public void Multiplication2()
    {
        var GF8 = new FiniteField(2, 3, new[] { 1, 1, 0, 1 });
        var element1 = new FiniteFieldElement(new[] { 1, 0, 1 }, GF8);
        var element2 = new FiniteFieldElement(new[] { 1, 1, 1 }, GF8);
        var subtract = element1 * element2;
        Assert.That(subtract.Coefficients, Is.EqualTo(new[] { 0, 1, 1 }));
    }

    [Test]
    public void Power1()
    {
        var GF16 = new FiniteField(2, 4, new[] { 1, 1, 0, 0, 1 });
        var element = new FiniteFieldElement(new[] { 0, 0, 0, 1 }, GF16);
        var power = element ^ 3;
        Assert.That(element.Coefficients, Is.EqualTo(new[] { 0, 0, 0, 1 }));
    }

    [Test]
    public void Power2()
    {
        var GF16 = new FiniteField(2, 4, new[] { 1, 1, 0, 0, 1 });
        var element = new FiniteFieldElement(new[] { 1, 1, 0, 1 }, GF16);
        var power = element ^ 7;
        Assert.That(element.Coefficients, Is.EqualTo(new[] { 1, 1, 0, 1 }));
    }

    [Test]
    public void ReverseMultiplication1()
    {
        var GF16 = new FiniteField(2, 4, new[] { 1, 1, 0, 0, 1 });
        var element = new FiniteFieldElement(new[] { 1, 1, 0, 1 }, GF16);
        var reverseMultiplication = ~element;
        Assert.That(reverseMultiplication.Coefficients, Is.EqualTo(new[] { 1, 0, 1 }));
    }
    
    [Test]
    public void ReverseMultiplication2()
    {
        var GF16807 = new FiniteField(7, 5, new[] { 1, 3, 0, 0, 0, 1 });
        var element = new FiniteFieldElement(new[] { 1, 4, 0, 3, 2 }, GF16807);
        var reverseMultiplication = ~element;
        Assert.That(reverseMultiplication.Coefficients, Is.EqualTo(new[] { 0, 6, 2, 4, 4 }));
    }

    [Test]
    public void Divide1()
    {
        var GF8 = new FiniteField(2, 3, new[] { 1, 1, 0, 1 });
        var element1 = new FiniteFieldElement(new[] { 1, 0, 0, 1 }, GF8);
        var element2 = new FiniteFieldElement(new[] { 1, 1 }, GF8);
        var divide = element1 / element2;
        Assert.That(divide.Coefficients, Is.EqualTo(new[] { 1, 1, 1 }));
    }

    [Test]
    public void Divide2()
    {
        var GF16 = new FiniteField(2, 4, new[] { 1, 1, 0, 0, 1 });
        var element1 = new FiniteFieldElement(new[] { 1, 1 }, GF16);
        var element2 = new FiniteFieldElement(new[] { 1, 1, 1, 1 }, GF16);
        var divide = element1 / element2;
        Assert.That(divide.Coefficients, Is.EqualTo(new[] { 1, 1, 0, 1 }));
    }

    [Test]
    public void ToFiniteField1()
    {
        var GF4 = new FiniteField(2, 3, new[] { 1, 1 });
        var element = GF4.GetFiniteFieldRepresent(new byte[] { 4, 0, 0, 0 });
        Assert.That(element.Coefficients, Is.EqualTo(new[] { 0, 0, 1, 0 }));
    }

    [Test]
    public void ToBinary1()
    {
        var GF4 = new FiniteField(2, 2, new int[] { 1, 1, 1 });
        var element = GF4.GetBinaryRepresent(new FiniteFieldElement(new int[] { 0, 0, 1 }, GF4));
        Assert.That(element, Is.EqualTo(new byte[] { 4, 0, 0, 0 }));
    }
}
