
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MyApp
{
    internal class Program
    {
        public static List<TaskModel> ListOfTasks = new List<TaskModel>();
        public static int longestText = 0;


        private static void Main(string[] args)
        {
            string[] command ;
            do
            {
                //TODO: Komenda i parametry w jednej linni
                ConsoleEx.Write($"Dostępne komendy: [{string.Join("/", Enum.GetNames(typeof(ConsoleCommand)))}]: ", ConsoleColor.Blue);
                command = Console.ReadLine().Split(' ');
                if (command.Length == 1 || command.Length == 2)
                {
                    command[0].ToLower();

                    if (Enum.GetNames(typeof(ConsoleCommand)).Contains(command[0]))
                    {
                        if (command[0] == "addtask")
                        {
                            AddTask();
                        }
                        else if (command[0] == "removetask")
                        {
                            RemoveTask(ListOfTasks);
                        }
                        else if (command[0] == "show")
                        {
                            Sort();
                            ShowTasks(false);
                        }
                        else if (command[0] == "save")
                        {
                            Sort();
                            SaveTasks();
                        }
                        else if (command[0] == "load")
                        {
                            if (File.Exists("Data.csv"))
                            {
                                ListOfTasks.Clear();

                                using (StreamReader sr = new StreamReader("Data.csv"))
                                {
                                    string[] lines = sr.ReadLine().Split(',');
                                    foreach (string line in lines)
                                    {
                                        if (line != null && line.Length == 4)
                                        {
                                            ListOfTasks.Add(new TaskModel(Validate.HasValue(line[0]),Validate.HasValue(line[1]), Validate.HasValue(line[2]), Validate.HasValue(line[3]));
                                        }
                                    }
                                   
                                }
                            }
                            else
                            {
                                ConsoleEx.WriteLine("Plik \"Dane.csv\" nie istnieje.", ConsoleColor.Red);
                            }
                        }
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Nieznana komenda.", ConsoleColor.Red);
                    }
                }
                else
                {
                    ConsoleEx.WriteLine("Podano za dużo parametrów", ConsoleColor.Red);
                }

            } while (command[0] != "exit");
        }

        private static void SaveTasks()
        {
            using (StreamWriter sw = new StreamWriter("Data.csv"))
            {
                foreach (var task in ListOfTasks)
                {
                    sw.WriteLine(task.Export());
                }
                ConsoleEx.WriteLine("Pomyślnie zapisano zadania w pliku \"Data.csv\"", ConsoleColor.Green);
                Process.Start("explorer.exe", ".");
            }
        }

        private static void Sort()
        {
            ListOfTasks.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));

            List<TaskModel> listOfImportant = new List<TaskModel>();
            List<TaskModel> listOfNotImportant = new List<TaskModel>();

            foreach (var task in ListOfTasks)
            {
                if (task.ActivityRank == true)
                {
                    listOfImportant.Add(task);
                }
                else
                {
                    listOfNotImportant.Add(task);
                }
            }

            ListOfTasks = listOfImportant.Concat(listOfNotImportant).ToList();
        }

        private static void ShowTasks(bool showIndex)
        {
            if (showIndex)
            {
                GetLongestDescription();
                ConsoleEx.WriteLine("ID|".PadLeft(5) + "Opis|".PadLeft(longestText + 5) +"Data Rozpoczęcia|".PadLeft(20)+"Data Zakończenia|".PadLeft(20)+"Zadanie Całodniowe|".PadLeft(20)+"Ważność|".PadLeft(10), ConsoleColor.DarkYellow);

                ConsoleEx.WriteLine(string.Empty.PadLeft(longestText + 80, '▒'), ConsoleColor.Yellow);
                for (int x = 0; x < ListOfTasks.Count; x++)
                {

                    ConsoleEx.WriteLine($"{x+"|".PadLeft(5)}{(ListOfTasks[x].Description+"|").PadLeft(longestText + 5)}{(ListOfTasks[x].StartDate.ToString("dd/MM/yyyy") + "|").PadLeft(20)}{(ListOfTasks[x].EndDate.Value.ToString("dd/MM/yyyy") + "|").PadLeft(20)}{(ListOfTasks[x].AllDayAllDayActivity + "|").PadLeft(20)}{(ListOfTasks[x].ActivityRank + "|").PadLeft(10)}", ConsoleColor.DarkYellow);
                }
            }
            else
            {
                GetLongestDescription();
                ConsoleEx.WriteLine("Opis|".PadLeft(longestText + 5) + "Data Rozpoczęcia|".PadLeft(20) + "Data Zakończenia|".PadLeft(20) + "Zadanie Całodniowe|".PadLeft(20) + "Ważność|".PadLeft(10), ConsoleColor.DarkYellow);

                for (int x = 0; x < ListOfTasks.Count; x++)
                {
                    ConsoleEx.WriteLine($"{(ListOfTasks[x].Description + "|").PadLeft(longestText + 5)}{(ListOfTasks[x].StartDate.ToString("dd/MM/yyyy") + "|").PadLeft(20)}{(ListOfTasks[x].EndDate.Value.ToString("dd/MM/yyyy") + "|").PadLeft(20)}{(ListOfTasks[x].AllDayAllDayActivity + "|").PadLeft(20)}{(ListOfTasks[x].ActivityRank + "|").PadLeft(10)}", ConsoleColor.DarkYellow);
                }
            }
        }

        private static void GetLongestDescription()
        {
            longestText = 0;
            foreach (var task in ListOfTasks)
            {
                if (task.Description.Length > longestText)
                {
                    longestText = task.Description.Length;
                }
            }
        }

        private static void RemoveTask(List<TaskModel> listOfTasks)
        {
            ShowTasks(true);
            ConsoleEx.WriteLine("", ConsoleColor.Blue);
            ConsoleEx.Write("Podaj numer indeksu do usunięcia:",ConsoleColor.Blue);
            string line = Console.ReadLine();
            if (Validate.MatchInteger(line))
            {
                int wrt = int.Parse(line);
                if (wrt >= 0 && wrt < listOfTasks.Count)
                {
                    ConsoleEx.Write($"Czy na pewno chcesz usunąć: {{{listOfTasks[wrt]}}} [T/N]: ", ConsoleColor.Yellow);
                    string select = Console.ReadLine().ToLower();
                    if (select == "t")
                    {
                        listOfTasks.Remove(listOfTasks.ElementAt(wrt));
                        ConsoleEx.WriteLine($"Element: {{{listOfTasks[wrt]}}} został pomyślnie usunięty.", ConsoleColor.Green);
                    }
                    else if (select == "f")
                    {
                        ConsoleEx.WriteLine($"Anulowano.", ConsoleColor.Red);

                    }
                    else
                    {
                        ConsoleEx.WriteLine($"Podany argument: \"{select}\" jest nieprawidłowy.", ConsoleColor.Red);
                    }
                }
                else
                {
                    ConsoleEx.WriteLine($"Podany argument: \"{line}\" jest spoza zakresu.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleEx.WriteLine($"Podany argument: \"{line}\" jest nieprawidłowy.", ConsoleColor.Red);
            }

        }

        private static void AddTask()
        {
            string possibilitiesInfo = @"Podaj w formacie: 
- opis;data_rozpoczęcia;[ważność-opcjonalne] - dla zdarzenia całodniowego.
- opis;data_rozpoczęcia;data_zakończenia;[ważność-opcjonalne] - dla zdarzenia z konkretnymi datami.";

            ConsoleEx.Write(possibilitiesInfo, ConsoleColor.Blue);
            string[] line = Console.ReadLine().Split(';');
            if (line.Length < 2 || line.Length > 4)
            {
                ConsoleEx.WriteLine("Nieprawidłowy format.", ConsoleColor.Red);
            }
            else if (!Validate.MatchDate(line[1]))
            {
                ConsoleEx.WriteLine($"Podany argument: \"{line[2]}\" jest nieprawidłowy.", ConsoleColor.Red);
                ConsoleEx.WriteLine($"Prawidłowy format daty: [dd/mm/yyyy], [dd-mm-yyyy], [dd.mm.yyyy]", ConsoleColor.Red);
            }
            else if (line.Length == 3)              // [Opis] ; [Data_rozpoczęcia] ; [{null => NieCałodobowe}/{true/false => całodobowe}]
            {
                if (string.IsNullOrEmpty(line[2]))                 // To nie całodniowe
                {
                    ListOfTasks.Add(new TaskModel(line[0], line[1], null, null));
                }
                else if (Validate.MatchBool(line[2]))                //To zadanie całodniowe
                {
                    ListOfTasks.Add(new TaskModel(line[0], line[1], null, bool.Parse(line[2].ToLower())));
                    ConsoleEx.WriteLine("Pomyślnie dodano zadaie całodniowe.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleEx.WriteLine($"Podany argument: \"{line[2]}\" jest nieprawidłowy.", ConsoleColor.Red);
                    ConsoleEx.WriteLine($"Prawidłowy format: [true], [false], [].", ConsoleColor.Red);
                }
            }
            else if (line.Length == 4)
            {
                if (Validate.MatchDate(line[2]))                // gdy jako 3 parametr jest podana data
                {
                    if (string.IsNullOrEmpty(line[3]))                 // gdy jako 4 parametr pusty ciagznakow
                    {
                        ListOfTasks.Add(new TaskModel(line[0], line[1], line[2], null));
                    }
                    else if (Validate.MatchBool(line[3]))
                    {
                        ListOfTasks.Add(new TaskModel(line[0], line[1], line[2], bool.Parse(line[3])));
                    }
                    else
                    {
                        ConsoleEx.WriteLine($"Podany argument: \"{line[3]}\" jest nieprawidłowy.", ConsoleColor.Red);
                        ConsoleEx.WriteLine($"Prawidłowy format: [true], [false], [].", ConsoleColor.Red);
                    }
                }
                else               // Nie może być puste
                {
                    ConsoleEx.WriteLine($"Podany argument: \"{line[2]}\" jest nieprawidłowy.", ConsoleColor.Red);
                    ConsoleEx.WriteLine($"Prawidłowy format daty: [dd/mm/yyyy], [dd-mm-yyyy], [dd.mm.yyyy]", ConsoleColor.Red);
                }
            }
        }
    }
}
