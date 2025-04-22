using System;
using elli_assist;
using NAudio.Wave;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    { //
        string configDir = "config";
        if (!Directory.Exists(configDir))
        {
            Directory.CreateDirectory(configDir);
        }
        
        string commandsPath = Path.Combine(configDir, "commands.json");
        if (!File.Exists(commandsPath))
        {
            var defaultCommands = new CommandList
            {
                Commands = new List<Command>
                {
                    new Command
                    {
                        Keywords = new List<string> { "время", "скажи время" },
                        Action = "show_time"
                    },
                    new Command
                    {
                        Keywords = new List<string> { "выключи", "выключи пк", "пока" },
                        Action = "shutdown"
                    }
                }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(defaultCommands, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(commandsPath, json);
        }

        Console.Title = "elli assistant, made with love<3";
        CommandManager.LoadCommands("config/commands.json");
        string configPath = "config/config.json";
        Config config = Config.Load(configPath);
        AnsiConsole.MarkupLine("[bold yellow]Конфигурация загружена.[/]");
        
        Recognizer recognizer = new Recognizer();
        AnsiConsole.Clear();
        
        ShowApplicationInfo();

        using (var waveIn = new WaveInEvent())
        {
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new WaveFormat(16000, 1);

            waveIn.DataAvailable += (sender, e) =>
            {
                string text = recognizer.ProcessAudio(e.Buffer, e.BytesRecorded);

                if (!string.IsNullOrEmpty(text))
                {
                    if (config.UseActivationWord)
                    {
                        if (text.StartsWith(config.ActivationWord, StringComparison.OrdinalIgnoreCase))
                        {
                            string commandText = text.Substring(config.ActivationWord.Length).Trim();
                            try
                            {
                                CommandManager.ExecuteCommand(commandText);
                            }
                            catch (Exception ex)
                            {
                                PrintError($"Ошибка выполнения команды: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            CommandManager.ExecuteCommand(text);
                        }
                        catch (Exception ex)
                        {
                            PrintError($"Ошибка выполнения команды: {ex.Message}");
                        }
                    }
                }
            };

            waveIn.StartRecording();
            Console.ReadLine(); // asdasddddsssssssssssssasd
            waveIn.StopRecording();
        }
    }

    private static readonly string version = "1.0.0";
    private static readonly string giturl = "https://github.com/ImAngelOfDead/elli-assist";
    private static void ShowApplicationInfo(){
        AnsiConsole.Write(
            new Panel(
                    $"[yellow]Author:[/] [cyan][link=https://github.com/ImAngelOfDead]ImAngelOfDead[/][/]\n" +
                    $"[yellow]Version:[/] [green]{version}[/]\n" +
                    $"[yellow]lzt url:[/] [link={giturl}]{giturl}[/]")
                .BorderColor(new Spectre.Console.Color(0, 255, 255))
                .Header("for open link, ctrl+lkm")
        );
    }
    private static void PrintError(string message)
    {
        var rule = new Rule("[bold red]Ошибка[/]");
        AnsiConsole.Write(rule);
        AnsiConsole.MarkupLine($"[bold red]{message}[/]");
    }
}
