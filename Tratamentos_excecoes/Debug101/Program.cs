string? readResult;
int startIndex = 0;
bool goodEntry = false;

int[] numbers = { 1, 2, 3, 4, 5 };

// Exibe o array no console.
Console.Clear();
Console.Write("\n\rO array 'numbers' contém: {");
foreach (int number in numbers)
{
    Console.Write($"{numbers}");
}

// Para calcular a soma dos elementos do array,
// solicita ao usuário o número do elemento inicial.
Console.WriteLine($"\n\r\n\rPara somar os valores de 'n' até 5, insira um valor para 'n':");
while (goodEntry == false)
{
    readResult = Console.ReadLine();
    goodEntry = int.TryParse(readResult, out startIndex);

    if (startIndex > 5)
    {
        goodEntry = false;
        Console.WriteLine("\n\rInsira um valor inteiro entre 1 e 5");
    }

}

// Exibe a soma e depois pausa.
Console.WriteLine($"\n\rA soma dos números de {startIndex} até {numbers.Length} é: {sumValues(numbers, startIndex)}");

Console.WriteLine("Pressione Enter para sair");
readResult = Console.ReadLine();

// Este método retorna a soma dos elementos de n até 5
static int sumValues(int[] numbers, int n)
{
    int sum = 0;
    for (int i = n; i < numbers.Length; i++)
    {
        sum += numbers[i];
    }
    return sum;
}