using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MyApp
{
    internal class Tasks
    {
        public static void LoadTasks()
        {
            ConsoleEx.WriteLine("Wybierz plik .csv do wczytania:", ConsoleColor.Magenta);
            foreach (var file in Directory.GetFiles("."))
            {
                ConsoleEx.WriteLine(Path.GetFileName(file), ConsoleColor.Yellow);
            }
            ConsoleEx.Write("", ConsoleColor.Blue);
            string myPath = Console.ReadLine();
            if (myPath != null && myPath.Contains('.'))
            {
                if (myPath.Split('.').Length == 2 && myPath.Split('.')[1].ToLower() == "csv")
                {
                    if (File.Exists(myPath))
                    {
                        Program.ListOfTasks.Clear();

                        using (StreamReader sr = new StreamReader(myPath))
                        {
                            List<string[]> param = new List<string[]>();

                            string fileLine;
                            while ((fileLine = sr.ReadLine()) != null)
                            {
                                param.Add(fileLine.Split(','));
                            }

                            foreach (var line in param)
                            {
                                if (line != null && line.Length == 5)
                                {
                                    Program.ListOfTasks.Add(new TaskModel(line[0], line[1], line[2], line[4]));
                                }
                                else
                                {
                                    ConsoleEx.WriteLine(
                                        $"Linnia [{line}] zawiera niepoprawny format danych.",
                                        ConsoleColor.Red);
                                }
                            }

                        }
                    }
                    else
                    {
                        ConsoleEx.WriteLine($"Plik {myPath} nie istnieje.", ConsoleColor.Red);
                    }
                }
                else
                {
                    ConsoleEx.WriteLine("Nieprawidłowy format - brak formatu csv", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleEx.WriteLine("Nieprawidłowy format - brak kropki", ConsoleColor.Red);
            }
        }

        public static void SaveTasks()
        {
            ConsoleEx.Write("Podaj nazwę pliku do zapisu:", ConsoleColor.Blue);
            string str = Console.ReadLine();
            if (File.Exists(str + ".csv"))
            {
                ConsoleEx.WriteLine("Plik o takiej nazwie już istnieje.", ConsoleColor.Red);
            }
            else
            {
                if (!str.Contains('.') && !str.Contains(' '))
                {
                    using (StreamWriter sw = new StreamWriter(str + ".csv"))
                    {
                        foreach (var task in Program.ListOfTasks)
                        {
                            sw.WriteLine(task.Export());
                        }
                        ConsoleEx.WriteLine($"Pomyślnie zapisano zadania w pliku {str}.csv", ConsoleColor.Green);
                        Process.Start("explorer.exe", ".");
                    }
                }
                else
                {
                    ConsoleEx.WriteLine("Plik nie może zawierać znaków specjalnych", ConsoleColor.Red);
                }
            }
        }

        public static void Sort()
        {
            Program.ListOfTasks.Sort((x, y) => DateTime.Parse(x.StartDate).CompareTo(DateTime.Parse(y.StartDate)));

            List<TaskModel> listOfImportant = new List<TaskModel>();
            List<TaskModel> listOfNotImportant = new List<TaskModel>();
            List<TaskModel> listOfNotImportantDefault = new List<TaskModel>();

            foreach (var task in Program.ListOfTasks)
            {
                if (task.ActivityRank == "Ważne")
                {
                    listOfImportant.Add(task);
                }
                else if (task.ActivityRank == "Nieważne")
                {
                    listOfNotImportant.Add(task);
                }
                else
                {
                    listOfNotImportantDefault.Add(task);
                }
            }

            Program.ListOfTasks = listOfImportant.Concat(listOfNotImportant).Concat(listOfNotImportantDefault).ToList();
        }

        public static void ShowTasks(bool showIndex)
        {
            if (showIndex)
            {
                GetLongestDescription();
                ConsoleEx.WriteLine("ID|".PadLeft(5) + "Opis|".PadLeft(Program.longestText + 5) + "Data Rozpoczęcia|".PadLeft(20) + "Data Zakończenia|".PadLeft(20) + "Zadanie Całodniowe|".PadLeft(20) + "Ważność|".PadLeft(10), ConsoleColor.DarkYellow);

                ConsoleEx.WriteLine(string.Empty.PadLeft(Program.longestText + 80, '▒'), ConsoleColor.Yellow);
                for (int x = 0; x < Program.ListOfTasks.Count; x++)
                {
                    ConsoleEx.WriteLine($"{(x + "|").PadLeft(5)}{(Program.ListOfTasks[x].Description + "|").PadLeft(Program.longestText + 5)}" +
                                        $"{(Program.ListOfTasks[x].StartDate + "|").PadLeft(20)}" +
                                        $"{(Program.ListOfTasks[x].EndDate + "|").PadLeft(20)}" +
                                        $"{(Program.ListOfTasks[x].AllDayAllDayActivity + "|").PadLeft(20)}" +
                                        $"{(Program.ListOfTasks[x].ActivityRank + "|").PadLeft(10)}", ConsoleColor.DarkYellow);
                }
            }
            else
            {
                GetLongestDescription();
                ConsoleEx.WriteLine("Opis|".PadLeft(Program.longestText + 5) + "Data Rozpoczęcia|".PadLeft(20) + "Data Zakończenia|".PadLeft(20) + "Zadanie Całodniowe|".PadLeft(20) + "Ważność|".PadLeft(10), ConsoleColor.DarkYellow);

                for (int x = 0; x < Program.ListOfTasks.Count; x++)
                {
                    ConsoleEx.WriteLine($"{(Program.ListOfTasks[x].Description + "|").PadLeft(Program.longestText + 5)}" +
                                        $"{(Program.ListOfTasks[x].StartDate + "|").PadLeft(20)}" +
                                        $"{(Program.ListOfTasks[x].EndDate + "|").PadLeft(20)}" +
                                        $"{(Program.ListOfTasks[x].AllDayAllDayActivity + "|").PadLeft(20)}" +
                                        $"{(Program.ListOfTasks[x].ActivityRank + "|").PadLeft(10)}", ConsoleColor.DarkYellow);
                }
            }
        }

        public static void GetLongestDescription()
        {
            Program.longestText = 0;
            foreach (var task in Program.ListOfTasks)
            {
                if (task.Description.Length > Program.longestText)
                {
                    Program.longestText = task.Description.Length;
                }
            }
        }

        public static void RemoveTask(List<TaskModel> listOfTasks)
        {
            ShowTasks(true);
            ConsoleEx.WriteLine("", ConsoleColor.Blue);
            ConsoleEx.Write("Podaj numer indeksu do usunięcia:", ConsoleColor.Blue);
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
                        ConsoleEx.WriteLine($"Element: {{{listOfTasks[wrt]}}} został pomyślnie usunięty.", ConsoleColor.Green);
                        listOfTasks.Remove(listOfTasks.ElementAt(wrt));
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

        public static void AddTask()
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
                ConsoleEx.WriteLine($"Podany argument: \"{line[1]}\" jest nieprawidłowy.", ConsoleColor.Red);
                ConsoleEx.WriteLine($"Prawidłowy format daty: [dd/mm/yyyy], [dd-mm-yyyy], [dd.mm.yyyy]", ConsoleColor.Red);
            }
            else if (line.Length == 3)              // [Opis] ; [Data_rozpoczęcia] ; [{null => NieCałodobowe}/{true/false => całodobowe}]
            {
                if (string.IsNullOrEmpty(line[2]))                 // To nie całodniowe
                {
                    Program.ListOfTasks.Add(new TaskModel(line[0], line[1], null, null));
                }
                else if (Validate.MatchBool(line[2]))                //To zadanie całodniowe
                {
                    Program.ListOfTasks.Add(new TaskModel(line[0], line[1], null, line[2]));
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
                        Program.ListOfTasks.Add(new TaskModel(line[0], line[1], line[2], null));
                        ConsoleEx.WriteLine("Pomyślnie dodano.", ConsoleColor.Green);

                    }
                    else if (Validate.MatchBool(line[3]))
                    {
                        Program.ListOfTasks.Add(new TaskModel(line[0], line[1], line[2], line[3]));
                        ConsoleEx.WriteLine("Pomyślnie dodano.", ConsoleColor.Green);

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
