namespace ProjektZaliczzeniowy
{
    using System;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using BCrypt.Net;

    public class Program
    {
        private static List<User> _users = new List<User>();

        public class User
        {
            public int ID { get; private set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public User(int id, string name, string email, string password)
            {
                ID = id;
                Name = name;
                Email = email;
                Password = password;
            }
        }
        private static int _lastID = 0;

        public User Register()
        {
            Console.WriteLine("Wprowadź dane do rejestracji!");
            Console.WriteLine("Imię: ");
            string name = Console.ReadLine();

            Console.WriteLine("Email: ");
            string email = Console.ReadLine();

            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            foreach (User existingUser in _users)
            {
                if (existingUser.Email == email)
                {
                    Console.WriteLine("Użytkownik o podanym emailu już istnieje!");
                    return null;
                }
            }

            _lastID++;
            User newUser = new User(_lastID, name, email, password);
            _users.Add(newUser);

            SaveUsersToJson();

            Console.WriteLine($"Użytkownik {newUser.Name} został poprawnie zarejestrowany!");

            return newUser;
        }
        public User Login()
        {
            Console.WriteLine("Zaloguj się!");
            Console.WriteLine("Email: ");
            string email = Console.ReadLine();

            Console.WriteLine("Hasło: ");
            string password = Console.ReadLine();

            User user = null;

            foreach (User u in _users)
            {
                if (u.Email == email && u.Password == password)
                {
                    user = u;
                }
            }

            if (user != null)
            {
                Console.WriteLine($"Zalogowano jako {user.Name}!");
                return user;
            }

            Console.WriteLine("Błędne dane logowania. Spróbuj ponownie.");
            return null;
        }

        public void SaveUsersToJson()
        {
            string json = JsonConvert.SerializeObject(_users);
            File.WriteAllText("users.json", json);
        }

        public void LoadUsersFromJson()
        {
            if (File.Exists("users.json"))
            {
                string json = File.ReadAllText("users.json");
                _users = JsonConvert.DeserializeObject<List<User>>(json);
            }
        }
        public static void LoginMenu()
        {
            bool exit = false;
            Program program = new Program();

            while(!exit) 
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Zaloguj się");
                Console.WriteLine("2. Zarejestruj się");
                Console.WriteLine("3. Wyjdź");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        User loggedInUser = program.Login();
                        if (loggedInUser != null)
                        {
                            Console.WriteLine($"Zalogowano jako: {loggedInUser.Name}");
                            ShowMenu(loggedInUser);
                        }
                        break;

                    case "2":
                        program.Register();
                        break;

                    case "3":
                        Console.WriteLine("Dowidzenia!");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Zła opcja!");
                        break;
                }
            }
            
        }
        public static void ShowMenu(User loggedInUser)
        {
            bool exit = false;

            while (!exit) 
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Rejestracja czasu pracy");
                Console.WriteLine("2. Zarządzanie urlopami");
                Console.WriteLine("3. Wyloguj");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Opcja 1");
                        break;

                    case "2":
                        Console.WriteLine("Opcja 2");
                        break;

                    case "3":
                        Console.WriteLine("Opcja 3");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Zły wybór!");
                        break;
                }
            }
        }
        public static void Main(string[] args)
        {
            Program program = new Program();
            program.LoadUsersFromJson();
            LoginMenu();
        }
    }
}
