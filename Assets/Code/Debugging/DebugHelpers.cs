using System.IO;

namespace AlienDebug
{
    static class DebugHelpers
    {
        public static void WriteXMLFile(string text)
        {
            using (StreamWriter sw = new StreamWriter("AlienKnowledge.xml", false))
            {
                sw.WriteLine(text);
            }
        }

        public static void WriteCatNumbers(int raw, int generalized)
        {
            using (StreamWriter sw = new StreamWriter("CategoryCount.txt", true))
            {
                sw.WriteLine(raw + "\t" + generalized);
            }
        }
    }
}
