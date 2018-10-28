
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp
{
    internal class Program
    {
        public static List<TaskModel> ListOfTasks = new List<TaskModel>();

        private static void Main(string[] args)
        {
            string command = "";
            do
            {
                ConsoleEx.Write($"Dostępne komendy: [{string.Join("/", Enum.GetNames(typeof(ConsoleCommand)))}]: ", ConsoleColor.Blue);
                command = Console.ReadLine().ToLower();
                if (Enum.GetNames(typeof(ConsoleCommand)).Contains(command.ToLower()))
                {
                    if (command == "addtask")
                    {
                        AddTask();
                    }
                    else if (command == "show")
                    {
                        //TODO: Przerobić na statyczą maybe albo użyc czegoś łądniejszego
                        foreach (var element in ListOfTasks)
                        {
                            ConsoleEx.WriteLine(element.ToString(), ConsoleColor.DarkYellow);
                        }
                    }
                }
                else
                {
                    ConsoleEx.WriteLine("Nieznana komenda.", ConsoleColor.Red);
                }

            } while (command != "exit");
        }

        private static void AddTask()
        {
            string possibilitiesInfo = @"Podaj w formacie: 
- opis;data_rozpoczęcia;[ważność-opcjonalne] - dla zdarzenia całodniowego.
- opis;data_rozpoczęcia;data_zakończenia;[ważność-opcjonalne] - dla zdarzenia z konkretnymi datami.";

            ConsoleEx.Write(possibilitiesInfo,ConsoleColor.Blue);
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
                    ListOfTasks.Add(new TaskModel(line[0], line[1], null, false, null));
                }
                else if (Validate.MatchBool(line[2]))                //To zadanie całodniowe
                {
                    ListOfTasks.Add(new TaskModel(line[0], line[1], null, true, bool.Parse(line[2].ToLower())));
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
                        ListOfTasks.Add(new TaskModel(line[0], line[1], line[2], false, null));
                    }
                    else if (Validate.MatchBool(line[3]))
                    {
                        ListOfTasks.Add(new TaskModel(line[0], line[1], line[2], false, bool.Parse(line[3])));
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


    public class TaskModel
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? AllDayAllDayActivity { get; set; } = false;
        public bool? ActivityRank { get; set; } = false;

        public TaskModel(string description, string startDate, string endDate, bool? allDayAllDayActivity, bool? activityRank)
        {
            Description = description;
            StartDate = DateTime.Parse(startDate);
            if (!string.IsNullOrEmpty(endDate))
            {
                EndDate = DateTime.Parse(endDate);
            }
            AllDayAllDayActivity = allDayAllDayActivity;
            ActivityRank = activityRank;
        }

        public override string ToString()
        {
            //TODO: Dodać hasValue
            return $"{Description}, {StartDate.ToString("dd/MM/yyyy")}, {EndDate.Value.ToString("dd/MM/yyy")}, {AllDayAllDayActivity.ToString()}, {ActivityRank}";
        }
    }
}
