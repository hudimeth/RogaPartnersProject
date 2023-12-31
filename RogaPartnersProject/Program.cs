using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Faker;

namespace RogaPartnersProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Person> people = GeneratePeopleList();
            string csv = BuildCsv(people);
            File.WriteAllText("people.csv", csv);

            string csvFromFile = File.ReadAllText("people.csv");
            List<Person> peopleFromCsv = ReadFromCsv(csvFromFile);

            Console.WriteLine();
            Console.WriteLine($"Average Age: {AverageAge(peopleFromCsv)}");
            Console.WriteLine($"People weighing between 120 and 140 lbs: {PeopleBtwn120And140Lbs(peopleFromCsv)}");
            Console.WriteLine($"Average age of people weighing between 120 and 140 lbs: {AverageAgeofPeopleWeighingBtwn120And140Lbs(peopleFromCsv)}");

            Console.ReadKey(true);
        }

        private static List<Person> GeneratePeopleList()
        {
            return Enumerable.Range(1, 1000).Select(_ => new Person
            {
                FirstName = Name.First(),
                LastName = Name.Last(),
                Age = RandomNumber.Next(18, 70),
                Weight = RandomNumber.Next(85,300),
                Gender = Faker.Enum.Random<Gender>()
            }).ToList();
        }
        private static string BuildCsv(List<Person> people)
        {
            StringBuilder builder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(builder);
            CsvWriter csv = new CsvWriter(stringWriter, CultureInfo.InvariantCulture);
            csv.WriteRecords(people);
            return builder.ToString();
        }
        private static List<Person> ReadFromCsv(string csv)
        {
            StringReader stringReader = new StringReader(csv); CsvReader reader = new CsvReader(stringReader, CultureInfo.InvariantCulture);
            return reader.GetRecords<Person>().ToList();
        }
        private static int AverageAge(List<Person> people)
        {
            int sum = 0;
            for(int i = 0; i< people.Count; i++)
            {
                sum += people[i].Age;
            }
            return sum / people.Count;
        }

        private static int PeopleBtwn120And140Lbs(List<Person> people)
        {
            return people.Where(person => person.Weight >= 120 && person.Weight <= 140).Count();
        }

        private static int AverageAgeofPeopleWeighingBtwn120And140Lbs(List<Person> people)
        {
            List<Person> peopleBtwn120And140Lbs = people.Where(person => person.Weight >= 120 && person.Weight <= 140).ToList();
            return AverageAge(peopleBtwn120And140Lbs);
        }
    }
}
