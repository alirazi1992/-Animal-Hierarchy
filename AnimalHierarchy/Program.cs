using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalHierarchy
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Animal Hierarchy - Day 7";
            var animals = new List<Animal>();

            // Seed a few examples
            animals.Add(new Dog("Rex", 3, breed: "Shepherd"));
            animals.Add(new Cat("Milo", 2, isIndoor: true));
            animals.Add(new Bird("Kiwi", 1, wingspanCm: 28));

            while (true)
            {
                ShowMenu();
                switch ((Console.ReadLine() ?? "").Trim())
                {
                    case "1": AddAnimal(animals); break;
                    case "2": ListAnimals(animals); break;
                    case "3": MakeAllSpeak(animals); break;
                    case "4": ShowMovements(animals); break;
                    case "5": ShowFlyers(animals); break;
                    case "0": Info("Bye 👋"); return;
                    default: Warn("Invalid choice."); break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== Animal Hierarchy ===");
            Console.WriteLine("1) Add animal");
            Console.WriteLine("2) List animals");
            Console.WriteLine("3) Make all speak");
            Console.WriteLine("4) Show movements");
            Console.WriteLine("5) Show flyers (IFly)");
            Console.WriteLine("0) Exit");
            Console.Write("Choose: ");
        }

        static void AddAnimal(List<Animal> animals)
        {
            Console.WriteLine("\nChoose type: 1) Dog  2) Cat  3) Bird");
            var type = (Console.ReadLine() ?? "").Trim();

            Console.Write("Name: ");
            var name = (Console.ReadLine() ?? "").Trim();

            int age = ReadInt("Age (years): ", 0, 100);

            switch (type)
            {
                case "1":
                    Console.Write("Breed (optional): ");
                    var breed = Console.ReadLine() ?? "";
                    animals.Add(new Dog(name, age, breed));
                    break;

                case "2":
                    Console.Write("Indoor cat? (y/n): ");
                    bool isIndoor = ReadYesNo();
                    animals.Add(new Cat(name, age, isIndoor));
                    break;

                case "3":
                    int wingspan = ReadInt("Wingspan (cm): ", 5, 300);
                    animals.Add(new Bird(name, age, wingspan));
                    break;

                default:
                    Warn("Unknown type. Aborted.");
                    return;
            }

            Notify("Added ✅");
        }

        static void ListAnimals(List<Animal> animals)
        {
            if (animals.Count == 0) { Info("No animals yet."); return; }

            Console.WriteLine("\n#  Type  Name                Age  Extra");
            Console.WriteLine("----------------------------------------------");
            for (int i = 0; i < animals.Count; i++)
            {
                var a = animals[i];
                string extra = a switch
                {
                    Dog d => $"Breed={d.Breed ?? "—"}",
                    Cat c => $"Indoor={c.IsIndoor}",
                    Bird b => $"Wingspan={b.WingspanCm}cm",
                    _ => "-"
                };
                Console.WriteLine($"{i + 1,2}  {a.GetType().Name,-5} {a.Name,-18} {a.Age,3}  {extra}");
            }
        }

        static void MakeAllSpeak(List<Animal> animals)
        {
            if (animals.Count == 0) { Info("No animals yet."); return; }
            Console.WriteLine();
            foreach (var a in animals)
            {
                Console.Write($"{a.Name} the {a.GetType().Name}: ");
                Console.WriteLine(a.Speak());
            }
        }

        static void ShowMovements(List<Animal> animals)
        {
            if (animals.Count == 0) { Info("No animals yet."); return; }
            Console.WriteLine();
            foreach (var a in animals)
            {
                Console.WriteLine($"{a.Name} -> {a.Move()}");
            }
        }

        static void ShowFlyers(List<Animal> animals)
        {
            var flyers = animals.OfType<IFly>().ToList();
            if (flyers.Count == 0) { Info("No flyers here."); return; }

            Console.WriteLine();
            foreach (var f in flyers)
            {
                Console.WriteLine(f.Fly());
            }
        }

        // Helpers
        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int v) && v >= min && v <= max)
                    return v;
                Warn($"Enter a number between {min} and {max}.");
            }
        }

        static bool ReadYesNo()
        {
            while (true)
            {
                var s = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
                if (s is "y" or "yes") return true;
                if (s is "n" or "no") return false;
                Warn("Please enter y/n.");
            }
        }

        static void Warn(string msg) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(msg); Console.ResetColor(); }
        static void Notify(string msg) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine(msg); Console.ResetColor(); }
        static void Info(string msg) { Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine(msg); Console.ResetColor(); }
    }

    // ===== OOP Core =====
    interface IFly
    {
        string Fly();
    }

    abstract class Animal
    {
        protected Animal(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; set; }
        public int Age { get; set; }

        public abstract string Speak();   // force derived classes to implement
        public virtual string Move() => "moves around"; // can be overridden
        public override string ToString() => $"{GetType().Name} {Name}, Age {Age}";
    }

    class Dog : Animal
    {
        public string? Breed { get; set; }
        public Dog(string name, int age, string? breed = null) : base(name, age) => Breed = breed;

        public override string Speak() => "Woof!";
        public override string Move() => "runs on four legs";
    }

    class Cat : Animal
    {
        public bool IsIndoor { get; set; }
        public Cat(string name, int age, bool isIndoor) : base(name, age) => IsIndoor = isIndoor;

        public override string Speak() => "Meow~";
        public override string Move() => "sneaks gracefully";
    }

    class Bird : Animal, IFly
    {
        public int WingspanCm { get; set; }
        public Bird(string name, int age, int wingspanCm) : base(name, age) => WingspanCm = wingspanCm;

        public override string Speak() => "Chirp!";
        public override string Move() => "hops and flutters";
        public string Fly() => $"{Name} spreads {WingspanCm}cm wings and takes off! 🕊️";
    }
}
