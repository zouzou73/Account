using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;

namespace AccountValueCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Chemin du fichier CSV
            string filePath = "Account.csv";

            // Date pour laquelle on souhaite obtenir la valeur du compte
            DateTime targetDate = new DateTime(2023, 2, 15); // Mettez la date souhaitée ici

            // Charger les transactions et le solde actuel depuis le fichier CSV
            var transactions = LoadTransactions(filePath);
            decimal currentBalance = GetCurrentBalance(filePath);

            // Calculer la valeur du compte pour la date cible
            decimal accountValue = CalculateAccountValue(transactions, currentBalance, targetDate);

            // Afficher le résultat
            Console.WriteLine($"La valeur du compte le {targetDate.ToShortDateString()} est de {accountValue} EUR.");
        }

        static List<Transaction> LoadTransactions(string filePath)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (var reader = new StreamReader(filePath))
            {
                // Ignorer la première ligne (en-tête)
                reader.ReadLine();

                // Lire les lignes restantes
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    DateTime date = DateTime.ParseExact(values[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    decimal amount = decimal.Parse(values[1], CultureInfo.InvariantCulture);
                    string currency = values[2];
                    string category = values[3];

                    transactions.Add(new Transaction(date, amount, currency, category));
                }
            }

            return transactions;
        }

        static decimal GetCurrentBalance(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                // Lire la première ligne
                var firstLine = reader.ReadLine();
                var balanceStr = firstLine.Split(':')[1].Trim().Split(' ')[0];
                return decimal.Parse(balanceStr, CultureInfo.InvariantCulture);
            }
        }

        static decimal CalculateAccountValue(List<Transaction> transactions, decimal currentBalance, DateTime targetDate)
        {
            decimal accountValue = currentBalance;

            foreach (var transaction in transactions)
            {
                if (transaction.Date <= targetDate)
                {
                    if (transaction.Currency == "EUR")
                    {
                        accountValue += transaction.Amount;
                    }
                    else if (transaction.Currency == "JPY")
                    {
                        // Conversion de JPY en EUR
                        accountValue += transaction.Amount * 0.482m; // Taux de change EUR/JPY
                    }
                    else if (transaction.Currency == "USD")
                    {
                        // Conversion de USD en EUR
                        accountValue += transaction.Amount * 1.445m; // Taux de change EUR/USD
                    }
                }
                else
                {
                    break; // Les transactions sont triées par date, donc on peut sortir de la boucle
                }
            }

            return accountValue;
        }
    }
    internal class Transaction
    {
        public DateTime Date { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public string Category { get; }

        public Transaction(DateTime date, decimal amount, string currency, string category)
        {
            Date = date;
            Amount = amount;
            Currency = currency;
            Category = category;
        }
    }

}