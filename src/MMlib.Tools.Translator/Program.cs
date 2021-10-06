using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Console = Colorful.Console;
using System.Drawing;

namespace MMlib.Tools.Translator
{
    class Program
    {
        static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Option<string>(
                    new string[] {"--source-language" ,"-s"},
                    () => "en",
                    null),
                new Option<string>(
                    new string[] {"--target-language" ,"-t"},
                    () => "sk",
                    "Target language."),
            };

            rootCommand.AddArgument(new Argument("text"));
            rootCommand.Description = "Text translation tool";


            rootCommand.Handler = CommandHandler.Create<string, string, string>(Translate);

            return rootCommand.InvokeAsync(args).Result;
        }

        private static async Task Translate(string text, string sourceLanguage, string targetLanguage)
        {
            Console.WriteLine("Translating ...", Color.AliceBlue);
            var translator = new GTranslate.Translators.Translator();
            var result = await translator.TranslateAsync(text, targetLanguage, sourceLanguage);
            ClearLine();
            Console.WriteLine(
                $"Translated text from {result.SourceLanguage.Name} to {result.TargetLanguage.Name}:", Color.AliceBlue);
            Console.WriteLine($"{result.Result}");
        }

        public static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
}
