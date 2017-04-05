using System.IO;

namespace AlienDebug
{
    static class DebugHelpers
    {
        public static void WriteToFile(string text)
        {
            using (StreamWriter sw = new StreamWriter("AlienOutput.txt", false))
            {
                sw.WriteLine(text);
            }
        }
    }
}
