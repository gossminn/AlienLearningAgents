using System;
using System.IO;

namespace AlienDebug
{
    internal static class DebugHelpers
    {
        public static void WriteXmlFile(string text)
        {
            using (var sw = new StreamWriter("Output/AlienKnowledge.xml", false))
            {
                sw.WriteLine(text);
            }
        }

        public static void WriteCatNumbers(int raw, int generalized)
        {
            using (var sw = new StreamWriter("Output/CategoryCount.txt", true))
            {
                sw.WriteLine(raw + "\t" + generalized);
            }
        }

        public static void WriteWordsetChanged(bool changed)
        {
            using (var sw = new StreamWriter("Output/CategoryChanged.txt", true))
            {
                sw.WriteLine(Convert.ToInt32(changed));
            }
        }
    }
}