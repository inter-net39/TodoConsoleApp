using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp
{
    internal class Program
    {
        public static List<TaskModel> List = new List<TaskModel>();

        private static void Main(string[] args)
        {
            string command = "";
            do
            {
                ConsoleEx.Write($"Dostępne komendy: [{string.Join("/", Enum.GetNames(typeof(ConsoleCommand)))}]: ", ConsoleColor.Blue);
                command = Console.ReadLine();
                if (Enum.GetNames(typeof(ConsoleCommand)).Contains(command))
                {
                    if (command == "AddTask")
                    {
                        //TODO:Przebudować konstruktory tip Slack.
                        ConsoleEx.Write($"Podaj w formacie: [Description];[StartDate];[ActivitySpan?];[ActivityValue?]: ", ConsoleColor.Blue);
                        string[] line = Console.ReadLine().Split(';');
                        if (line.Length < 2 || line.Length > 4) 
                        {
                            ConsoleEx.WriteLine("Nieprawidłowy format.", ConsoleColor.Red);
                        }
                        else if (line.Length == 3)
                        {
                            if (line[2] != "AllDay")
                            {
                                ConsoleEx.WriteLine("Nieprawidłowy format. Jeśli flaga zadanie całodniowe jest ustawiona, data zakończenia nie jest wymagana", ConsoleColor.Red);
                            }
                            else
                            {
                                List.Add(new TaskModel(line[0],DateTime.Parse(line[1]), (TaskSpan) Enum.Parse(typeof(TaskSpan),"AllDay")));
                            }
                        }
                        else if (line.Length == 4)
                        {
                            List.Add(new TaskModel(line[0], DateTime.Parse(line[1]), (TaskSpan)Enum.Parse(typeof(TaskSpan), line[2]), line[3]));
                        }
                        else
                        {
                            List.Add(new TaskModel(line[0], DateTime.Parse(line[1]) ));
                        }
                    }
                    else if (command == "Show")
                    {
                        foreach (var element in List)
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
    }

    public class TaskModel
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public TaskSpan ActivitySpan { get; set; } = TaskSpan.NoAllDay;
        public string ActivityValue { get; set; } = "NotImportant";

        public TaskModel(string description, DateTime startDate, TaskSpan activitySpan, string activityValue)
        {
            Description = description;
            StartDate = startDate;
            ActivitySpan = activitySpan;
            ActivityValue = activityValue;
        }

        public TaskModel(string description, DateTime startDate, TaskSpan activitySpan)
        {
            Description = description;
            StartDate = startDate;
            ActivitySpan = activitySpan;
        }
        public TaskModel(string description, DateTime startDate)
        {
            Description = description;
            StartDate = startDate;
        }

        public override string ToString()
        {
            return $"{Description}, {StartDate.ToString("d")}, {ActivitySpan.ToString()}, {ActivityValue}";
        }
    }
}
