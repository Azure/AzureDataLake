using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace SDKSampleHelpers
{
    public class ConsolePrompts
    {
        public static string MenuPrompt(string promptText, IDictionary<string, string> dict, bool responseRequired = false, bool mustBeInt = false)
        {
            var keys = dict.Keys.ToList();
            keys.Sort();
            var menuItems = keys.Select(a => String.Format("{0,25} ({1})", a, dict[a])).ToList();
            var input = MenuPrompt(promptText, menuItems, responseRequired, mustBeInt);
            if (input == null)
                return null;
            try
            {
                var inputInt = Convert.ToUInt16(input);
                return keys[inputInt];
            }
            catch (FormatException)
            {
                return input;
            }
        }

        public static string MenuPrompt(string promptText, IList<string> items, bool responseRequired = false, bool mustBeInt = false)
        {
            var menuPromptText = promptText;
            var itemsCount = items.Count();
            for (var i = 0; i < itemsCount; i++)
                menuPromptText += String.Format("\r\n{0,4} : {1}", i, items[i]);
            var input = Prompt(menuPromptText, responseRequired, mustBeInt, (items.Count()-1).ToString().Length);
            return input;
        }

        public static string Prompt(string promptText, bool responseRequired = false, bool mustBeInt = false, int expectedLength=0)
        {
            Console.WriteLine("\r\n" + promptText);
            var input = "";
            ushort testInt;

            do
            {
                Console.Write("> ");
                if (expectedLength != 0)
                {
                    input = "";
                    do
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey();
                        if (keyInfo.Key == ConsoleKey.Enter)
                            break;
                        input += keyInfo.KeyChar;
                        if (expectedLength != 0 && input.Length == expectedLength)
                            break;
                    } while (true);
                    Console.WriteLine();
                }
                else
                {
                    input = Console.ReadLine();
                }
            } while ((responseRequired && string.IsNullOrWhiteSpace(input)) || (mustBeInt && !ushort.TryParse(input, out testInt)));

            if (string.IsNullOrWhiteSpace(input))
                input = null;

            return input;
        }

        public static SecureString SecurePrompt(string promptText, bool responseRequired = false)
        {
            SecureString secureInput;

            do
            {
                Console.WriteLine("\r\n"+promptText);
                Console.Write("> ");

                secureInput = new SecureString();
                while (true)
                {
                    var keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                        break;
                    secureInput.AppendChar(keyInfo.KeyChar);
                }
            } while (secureInput.Length == 0);

            return secureInput;
        }
    }
}
