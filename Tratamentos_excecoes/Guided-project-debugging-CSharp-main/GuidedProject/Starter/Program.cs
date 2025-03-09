/*
Este aplicativo gerencia transações em uma linha de check-out de loja. A
linha de check-out tem um caixa registradora, e a registradora tem um caixa
que é preparado com um número de notas todas as manhãs. O caixa inclui
notas de quatro denominações: $1, $5, $10 e $20. O caixa é usado
para fornecer troco ao cliente durante a transação. O custo do item
é um número gerado aleatoriamente entre 2 e 49. O cliente
oferece pagamento com base em um algoritmo que determina um número de notas
em cada denominação.

Cada dia, o caixa é carregado no início do dia. À medida que as transações
ocorrem, o caixa é gerenciado em um método chamado MakeChange (pagamentos do cliente
entram e o troco devolvido ao cliente sai). Um
cálculo de "verificação de segurança" separado que é usado para verificar a quantidade de
dinheiro no caixa é realizado no "programa principal". Esta verificação de segurança
é usada para garantir que a lógica no método MakeChange esteja funcionando como
esperado.
*/

string? readResult = null;
bool useTestData = false;

Console.Clear();

int[] cashTill = new int[] { 0, 0, 0, 0 };
int registerCheckTillTotal = 0;

// registerDailyStartingCash: $1 x 50, $5 x 20, $10 x 10, $20 x 5 => ($350 total)
int[,] registerDailyStartingCash = new int[,] { { 1, 50 }, { 5, 20 }, { 10, 10 }, { 20, 5 } };

int[] testData = new int[] { 6, 10, 17, 20, 31, 36, 40, 41 };
int testCounter = 0;

LoadTillEachMorning(registerDailyStartingCash, cashTill);

registerCheckTillTotal = registerDailyStartingCash[0, 0] * registerDailyStartingCash[0, 1] + registerDailyStartingCash[1, 0] * registerDailyStartingCash[1, 1] + registerDailyStartingCash[2, 0] * registerDailyStartingCash[2, 1] + registerDailyStartingCash[3, 0] * registerDailyStartingCash[3, 1];

// exibir o número de notas de cada denominação atualmente no caixa
LogTillStatus(cashTill);

// exibir uma mensagem mostrando a quantidade de dinheiro no caixa
Console.WriteLine(TillAmountSummary(cashTill));

// exibir o valor esperado do registerDailyStartingCash
Console.WriteLine($"Valor esperado no caixa: {registerCheckTillTotal}\n\r");

var valueGenerator = new Random((int)DateTime.Now.Ticks);

int transactions = 10;

if (useTestData)
{
    transactions = testData.Length;
}

while (transactions > 0)
{
    transactions -= 1;
    int itemCost = valueGenerator.Next(2, 20);

    if (useTestData)
    {
        itemCost = testData[testCounter];
        testCounter += 1;
    }

    int paymentOnes = itemCost % 2;                 // valor é 1 quando itemCost é ímpar, valor é 0 quando itemCost é par
    int paymentFives = (itemCost % 10 > 7) ? 1 : 0; // valor é 1 quando itemCost termina com 8 ou 9, caso contrário, valor é 0
    int paymentTens = (itemCost % 20 > 13) ? 1 : 0; // valor é 1 quando 13 < itemCost < 20 OU 33 < itemCost < 40, caso contrário, valor é 0
    int paymentTwenties = (itemCost < 20) ? 1 : 2;  // valor é 1 quando itemCost < 20, caso contrário, valor é 2

    // exibir mensagens descrevendo a transação atual
    Console.WriteLine($"Cliente está fazendo uma compra de ${itemCost}");
    Console.WriteLine($"\t Usando {paymentTwenties} notas de vinte dólares");
    Console.WriteLine($"\t Usando {paymentTens} notas de dez dólares");
    Console.WriteLine($"\t Usando {paymentFives} notas de cinco dólares");
    Console.WriteLine($"\t Usando {paymentOnes} notas de um dólar");

    // MakeChange gerencia a transação e atualiza o caixa
    string transactionMessage = MakeChange(itemCost, cashTill, paymentTwenties, paymentTens, paymentFives, paymentOnes);

    // Cálculo de Backup - cada transação adiciona o "itemCost" atual ao caixa
    if (transactionMessage == "transaction succeeded")
    {
        Console.WriteLine($"Transação concluída com sucesso.");
        registerCheckTillTotal += itemCost;
    }
    else
    {
        Console.WriteLine($"Transação malsucedida: {transactionMessage}");
    }

    Console.WriteLine(TillAmountSummary(cashTill));
    Console.WriteLine($"Valor esperado no caixa: {registerCheckTillTotal}\n\r");
    Console.WriteLine();
}

Console.WriteLine("Pressione a tecla Enter para sair");
do
{
    readResult = Console.ReadLine();

} while (readResult == null);


static void LoadTillEachMorning(int[,] registerDailyStartingCash, int[] cashTill)
{
    cashTill[0] = registerDailyStartingCash[0, 1];
    cashTill[1] = registerDailyStartingCash[1, 1];
    cashTill[2] = registerDailyStartingCash[2, 1];
    cashTill[3] = registerDailyStartingCash[3, 1];
}


static string MakeChange(int cost, int[] cashTill, int twenties, int tens = 0, int fives = 0, int ones = 0)
{
    string transactionMessage = "";

    cashTill[3] += twenties;
    cashTill[2] += tens;
    cashTill[1] += fives;
    cashTill[0] += ones;

    int amountPaid = twenties * 20 + tens * 10 + fives * 5 + ones;
    int changeNeeded = amountPaid - cost;

    if (changeNeeded < 0)
        transactionMessage = "Dinheiro insuficiente fornecido.";

    Console.WriteLine("Caixa devolve:");

    while ((changeNeeded > 19) && (cashTill[3] > 0))
    {
        cashTill[3]--;
        changeNeeded -= 20;
        Console.WriteLine("\t Uma nota de vinte");
    }

    while ((changeNeeded > 9) && (cashTill[2] > 0))
    {
        cashTill[2]--;
        changeNeeded -= 10;
        Console.WriteLine("\t Uma nota de dez");
    }

    while ((changeNeeded > 4) && (cashTill[1] > 0))
    {
        cashTill[2]--;
        changeNeeded -= 5;
        Console.WriteLine("\t Uma nota de cinco");
    }

    while ((changeNeeded > 0) && (cashTill[0] > 0))
    {
        cashTill[0]--;
        changeNeeded--;
        Console.WriteLine("\t Uma nota de um");
    }

    if (changeNeeded > 0)
        transactionMessage = "Não é possível dar troco. Você tem algo menor?";

    if (transactionMessage == "")
        transactionMessage = "transação bem-sucedida";

    return transactionMessage;
}

static void LogTillStatus(int[] cashTill)
{
    Console.WriteLine("O caixa atualmente tem:");
    Console.WriteLine($"{cashTill[3] * 20} em notas de vinte");
    Console.WriteLine($"{cashTill[2] * 10} em notas de dez");
    Console.WriteLine($"{cashTill[1] * 5} em notas de cinco");
    Console.WriteLine($"{cashTill[0]} em notas de um");
    Console.WriteLine();
}

static string TillAmountSummary(int[] cashTill)
{
    return $"O caixa tem {cashTill[3] * 20 + cashTill[2] * 10 + cashTill[1] * 5 + cashTill[0]} dólares";
}
