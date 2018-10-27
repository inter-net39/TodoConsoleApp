
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            //TODO:Przebudować konstruktory tip Slack.
            ConsoleEx.Write($"Podaj w formacie: [Description];[StartDate];[ActivitySpan?];[ActivityRank?]: ", ConsoleColor.Blue);
            string[] line = Console.ReadLine().Split(';');
            if (line.Length < 2 || line.Length > 4)
            {
                ConsoleEx.WriteLine("Nieprawidłowy format.", ConsoleColor.Red);
            }
            else if (line.Length == 2)  //Dla ciagów które zawirają dokladnie 2 parametry
            {
                if (Validate.MatchDate(line[1]))
                {
                    ListOfTasks.Add(new TaskModel(line[0], line[1]));
                    ConsoleEx.WriteLine("Pomyślnie dodano zadaie.",ConsoleColor.Green);
                }
                else
                {
                    ConsoleEx.WriteLine("Podana data jest nieprawidłowa.", ConsoleColor.Red);
                    ConsoleEx.WriteLine($"Prawidłowy format daty: [dd/mm/yyyy], [dd-mm-yyyy], [dd.mm.yyyy]", ConsoleColor.Red);
                }
            }
        }
    }
    public static class Validate
    {
        /// <summary>
        ///     dd/mm/yyyy,dd-mm-yyyy or dd.mm.yyyy
        /// </summary>
        private static Regex _regexDate = new Regex(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");

        public static bool MatchDate(string date)
        {
            Match match = _regexDate.Match(date);
            if (match.Success)
            {
                return true;
            }

            return false;
        }
    }


    public class TaskModel
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public bool AllDayAllDayActivity { get; set; }
        public string ActivityRank { get; set; } = "NotImportant";

        public TaskModel(string description, string startDate, bool allDayActivity, string activityRank)
        {
            Description = description;
            StartDate = DateTime.Parse(startDate);
            AllDayAllDayActivity = allDayActivity;
            ActivityRank = activityRank;
        }

        public TaskModel(string description, string startDate, bool allDayActivity)
        {
            Description = description;
            StartDate = DateTime.Parse(startDate);
            AllDayAllDayActivity = allDayActivity;
        }
        public TaskModel(string description, string startDate)
        {
            Description = description;
            StartDate = DateTime.Parse(startDate);
        }

        public override string ToString()
        {
            return $"{Description}, {StartDate.ToString("dd/MM/yyyy")}, {AllDayAllDayActivity.ToString()}, {ActivityRank}";
        }
    }
}
