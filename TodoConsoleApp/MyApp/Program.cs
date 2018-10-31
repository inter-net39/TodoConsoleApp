
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

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
                ConsoleEx.Write($"Dostępne komendy: [{string.Join("/", Enum.GetNames(typeof(ConsoleCommand)))+"/exit"}]: ", ConsoleColor.Blue);
                command = Console.ReadLine().Split(' ');
                if (command.Length == 1 || command.Length == 2)
                {
                    command[0].ToLower();

                    if (Enum.GetNames(typeof(ConsoleCommand)).Contains(command[0]))
                    {
                        if (command[0] == "addtask")
                        {
                            Tasks.AddTask();
                        }
                        else if (command[0] == "removetask")
                        {
                            Tasks.RemoveTask(ListOfTasks);
                        }
                        else if (command[0] == "show")
                        {
                            Tasks.Sort();
                            Tasks.ShowTasks(false);
                        }
                        else if (command[0] == "save")
                        {
                            Tasks.Sort();
                            Tasks.SaveTasks();
                        }
                        else if (command[0] == "load")
                        {
                            Tasks.LoadTasks();
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
        
    }
}
