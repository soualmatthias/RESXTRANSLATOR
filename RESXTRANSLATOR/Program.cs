using System.Collections;
using System.Resources.NetStandard;
using DeepL;

class Program
{
    static async Task Main()
    {
        string resxFilePath = ""; // Source file path
        string newResxFilePath = ""; // New file Path
        const string authKey = ""; // Your DeepL API key
        const string baseLanguage = LanguageCode.English; // Select source Language
        const string targetLanguage = LanguageCode.PortugueseBrazilian; // Select target Language
        int i = 0;
        int size = 0;
        int total = 0;

        // get number of strings to translate
        using (ResXResourceReader resxReader = new ResXResourceReader(resxFilePath))
        using (ResXResourceWriter resxWriter = new ResXResourceWriter(newResxFilePath))
        {
            foreach (DictionaryEntry entry in resxReader)
            {
                size++;
            }
        }

        // translate strings
        using (ResXResourceReader resxReader = new ResXResourceReader(resxFilePath))
        using (ResXResourceWriter resxWriter = new ResXResourceWriter(newResxFilePath))
        {
            foreach (DictionaryEntry entry in resxReader)
            {
                string resourceName = entry.Key.ToString();
                string resourceValue = entry.Value.ToString();

                Console.WriteLine($"Original text: {resourceValue}");

                if (string.IsNullOrEmpty(resourceName))
                {
                    Console.WriteLine("ERROR");
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(resourceValue))
                    {
                        resxWriter.AddResource(resourceName, "");
                        Console.WriteLine($"Translated Value: ");
                    }
                    else
                    {
                        var translator = new Translator(authKey);
                        var translatedText = await translator.TranslateTextAsync(
                                  resourceValue,
                                  baseLanguage,
                                  targetLanguage);

                        string translatedValue = translatedText.Text;
                        Console.WriteLine($"Translated text: {translatedValue}");
                        total += translatedValue.Length;
                        resxWriter.AddResource(resourceName, translatedValue);
                    }
                }
                i++;
                Console.WriteLine("Progression : {0}/{1}", i, size);
                Console.WriteLine("Total characters translated : {0}", total);
            }
            Console.WriteLine("\n======================\n Tanslation complete.\n======================");
        }
    }
}
