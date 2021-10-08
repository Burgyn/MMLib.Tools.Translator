using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Console = Colorful.Console;
using System.Drawing;
using System;

namespace MMlib.Tools.Translator
{
    class Program
    {
        private const string _envPrefix = "MMLIB_TRANSLATOR_";
        private const string _sourceLngKey = "SOURCE_LNG";
        private const string _targetLngKey = "TARGET_LNG";

        static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Option<string>(
                    new string[] {"--source-language" ,"-s"},
                    () => GetDefault(_sourceLngKey, "en"),
                    "Source langulate"),
                new Option<string>(
                    new string[] {"--target-language" ,"-t"},
                    () => GetDefault(_targetLngKey, "sk"),
                    "Target language"),
                new Option<bool>(
                    new string[] { "--reverse", "-re" },
                    () => false,
                    "Reverse source language with target language"),
                new Option<bool>(
                    new string[] { "--copy-to-clipboard", "-co" },
                    () => false,
                    "Copy result to clipboard")
            };

            rootCommand.AddArgument(new Argument("text"));
            rootCommand.Description = "Text translation tool";
            rootCommand.AddCommand(CreateInfoCommand());
            rootCommand.AddCommand(CreateSetDefaultLanguagesCommand());


            rootCommand.Handler = CommandHandler.Create<string, string, string, bool, bool>(Translate);

            return rootCommand.InvokeAsync(args).Result;
        }

        private static string GetDefault(string key, string defaultValue)
            => Environment.GetEnvironmentVariable($"{_envPrefix}{key}", EnvironmentVariableTarget.User) ?? defaultValue;

        private static void SetDefault(string key, string value)
            => Environment.SetEnvironmentVariable($"{_envPrefix}{key}", value, EnvironmentVariableTarget.User);

        private static async Task Translate(
            string text,
            string sourceLanguage,
            string targetLanguage,
            bool reverse,
            bool copyToClipBoard)
        {
            Console.WriteLine("Translating ...", Color.Yellow);

            if (reverse)
            {
                var tmp = sourceLanguage;
                sourceLanguage = targetLanguage;
                targetLanguage = tmp;
            }

            var translator = new GTranslate.Translators.GoogleTranslator();
            var result = await translator.TranslateAsync(text, targetLanguage, sourceLanguage);
            ClearLine();
            Console.WriteLine(
                $"Translated text from {result.SourceLanguage.Name} to {result.TargetLanguage.Name}:", Color.Yellow);
            Console.WriteLine($"{result.Result}");

            if (copyToClipBoard)
            {
                TextCopy.ClipboardService.SetText(result.Result);
            }
        }

        private static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        private static Command CreateSetDefaultLanguagesCommand()
        {
            var command = new Command("set-defaults") {
                 new Option<string>(
                    new string[] {"--source-language" ,"-s"},
                    "Source langulate"),
                new Option<string>(
                    new string[] {"--target-language" ,"-t"},
                    "Target language")
            };
            command.Handler = CommandHandler.Create<string, string>((sourceLanguage, targetLanguage) =>
            {
                SetDefault(_sourceLngKey, sourceLanguage);
                SetDefault(_targetLngKey, targetLanguage);
                Console.WriteLine("Defaults was set", Color.Yellow);
                Console.WriteLine($"Source language: {sourceLanguage}", Color.Yellow);
                Console.WriteLine($"Target language: {targetLanguage}", Color.Yellow);

            });

            return command;
        }

        private static Command CreateInfoCommand()
        {
            var command = new Command("info");
            command.Handler = CommandHandler.Create(() =>
            {
                Console.WriteLine(@"
  __  __  __  __  _       _  _     
 |  \/  ||  \/  || |     (_)| |    
 | \  / || \  / || |      _ | |__  
 | |\/| || |\/| || |     | || '_ \ 
 | |  | || |  | || |____ | || |_) |
 |_|  |_||_|  |_||______||_||_.__/ 
                                   
                                   
");
                Console.WriteLine(@"
 _____  ____  ____  _      ____  _     ____  _____  ____  ____ 
/__ __\/  __\/  _ \/ \  /|/ ___\/ \   /  _ \/__ __\/  _ \/  __\
  / \  |  \/|| / \|| |\ |||    \| |   | / \|  / \  | / \||  \/|
  | |  |    /| |-||| | \||\___ || |_/\| |-||  | |  | \_/||    /
  \_/  \_/\_\\_/ \|\_/  \|\____/\____/\_/ \|  \_/  \____/\_/\_\
                                                               
");
            });

            return command;
        }
    }
}
