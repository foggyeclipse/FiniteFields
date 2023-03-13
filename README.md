# Библиотека для работы с конечными полями
Данная библиотека представляет собой два класса: FiniteField и FiniteFieldElements. 
> Все поля подразумеваются конечными. Поля имеют вид $F\_{p^n} \simeq F_p[X]/(q)$, где $p$ - простое число 

Первый из рассматриваемых классов хранит всю информацию о поле, в котором производятся операции. 
Класс FiniteField содержит в себе всего 3 элемента: 
$p$ - характеристика поля, 
$n$ - степень, 
$q$ - неприводимый многочлен над полем.

Второй класс представляет собой элементы заданного поля с реализованными операциями над ними.

## 1.1 Создания поля 
```C#
int characteristic = 2;
int degree = 2;

FiniteField GF4 = new FiniteField (characteristic, degree, new int[] { 1, 1, 1 }); 
```
## 1.2 Методы поля 
### Получение нулевого элемента
```C#
GF4.GetZero();
```
### Получение единичного элемента
```C#
GF4.GetOne();
```
## 2.1 Создание элемента поля 
```C#
var GF4 = new FiniteField(2,2,new int[]{1,1,1});
FiniteFieldElements element1 = new FiniteFieldElements(new int[] {2, 1}, GF4);
```
## 2.2 Операции над элементами поля 
### Сложение
```C#
FiniteFieldElements element2 = new FiniteFieldElements(new int[] {1, 1}, GF4);
var elementSum = element1 + element2;
```
### Вычитание
```C#
FiniteFieldElements element2 = new FiniteFieldElements(new int[] {1, 1}, GF4);
var elementSubtract = element1 - element2;
```
### Умножение
```C#
FiniteFieldElements element2 = new FiniteFieldElements(new int[] {1, 1}, GF4);
var elementMultiplication = element1 * element2;
```
### Деление
```C#
FiniteFieldElements element2 = new FiniteFieldElements(new int[] {1, 1}, GF4);
var elementDivide = element1 / element2;
```
### Нахождение элемента, обратного по сложению
```C#
var elementReverseAddition = (-element1);
```
### Нахождение элемента, обратного по умножению
```C#
var elementReverse = (~element1);
```
### Возведение в степень
```C#
var n = 2;
var elementPower = element1 ^ n;
```



