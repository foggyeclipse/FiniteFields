using FiniteFields;
using NUnit.Framework;

namespace FiniteFieldsTests;

public class Tests
{
    [Test]
    public void Reverse1()
    {
        var GF4 = new FiniteField(2, 2, new int[] {1, 1});
        var element1 = new FiniteFieldElements(new int[] {1, 1}, GF4);
        var reverse = ~ element1;
        Assert.That(reverse.Coefficients, Is.EqualTo(new int[] {0}));
    }

    [Test]
    public void Sum1()
    {
        var GF4 = new FiniteField(2, 2, new int[] {1, 1, 1});
        var element1 = new FiniteFieldElements(new int[] {1, 1}, GF4);
        var element2 = new FiniteFieldElements(new int[] {0, 1}, GF4);
        var sum = element1 + element2;
        Assert.That(sum.Coefficients, Is.EqualTo(new int[] {1, 0}));
    }

    [Test]
    public void Subtract1()
    {
        var GF9 = new FiniteField(3, 2, new int[] {1, 1, 2});
        var element1 = new FiniteFieldElements(new int[] {1, 2}, GF9);
        var element2 = new FiniteFieldElements(new int[] {2, 0}, GF9);
        var subtract = element1 - element2;
        Assert.That(subtract.Coefficients, Is.EqualTo(new int[] {2, 2}));
    }

    [Test]
    public void Power1()
    {
        var GF9 = new FiniteField(3, 2, new int[] {1, 2, 2});
        var element1 = new FiniteFieldElements(new int[] {2, 1}, GF9);
        var power = element1 ^ 9;
        Assert.That(element1.Coefficients, Is.EqualTo(power.Coefficients));
    }

    [Test]
    public void Multiplication1()
    {
        var GF4 = new FiniteField(2, 2, new int[] {1, 1, 1});
        var element1 = new FiniteFieldElements(new int[] {1, 0, 1}, GF4);
        var element2 = GF4.GetZero();
        var multiplication = element1 * element2;
        multiplication += element1;
        Assert.That(multiplication.Coefficients, Is.EqualTo(element1.Coefficients));
    }


    [Test]
    public void Divide1()
    {
        var GF4 = new FiniteField(2, 2, new int[] {1, 1});
        var element1 = new FiniteFieldElements(new int[] {1, 1}, GF4);
        var element2 = new FiniteFieldElements(new int[] {1}, GF4);
        var divide = element1 / element2;
        Assert.That(divide.Coefficients, Is.EqualTo(new int[] {0}));
    }
}