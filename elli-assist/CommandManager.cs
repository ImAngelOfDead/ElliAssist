using System.Diagnostics;
using Newtonsoft.Json;

namespace elli_assist;

public static class CommandManager
{
    private static List<Command> Commands;

    public static void LoadCommands(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл команд не найден!");
            Environment.Exit(1);
        }

        string json = File.ReadAllText(filePath);
        Commands = JsonConvert.DeserializeObject<CommandList>(json)?.Commands;
        if (Commands == null || Commands.Count == 0)
        {
            Console.WriteLine("Файл команд пуст или некорректен!");
            Environment.Exit(1);
        }
    }

    public static void ExecuteCommand(string inputText)
    {
        foreach (var command in Commands)
        {
            if (command.Keywords.Any(keyword => inputText.Contains(keyword)))
            {
                PerformAction(command);
                return;
            }
        }
        Console.WriteLine("Команда не распознана.");
    }

    private static void PerformAction(Command command)
    {
        switch (command.Action)
        {
            case "shutdown":
                Console.WriteLine("Выключаю компьютер...");
                Process.Start("shutdown", "/s /t 0");
                break;

            case "show_time":
                Console.WriteLine($"Текущее время: {DateTime.Now}");
                break;

            case "custom":
                if (!string.IsNullOrEmpty(command.Path))
                {
                    Console.WriteLine($"Запускаю: {command.Path}");
                    try
                    {
                        Process.Start(command.Path);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при запуске: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Не указан путь для команды custom.");
                }
                break;

            default:
                Console.WriteLine($"Неизвестное действие: {command.Action}");
                break;
        }
    }
}