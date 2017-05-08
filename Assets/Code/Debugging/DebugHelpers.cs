using System;
using System.IO;
using Code.LearningEngine.Agents;

namespace Code.Debugging
{
    internal static class DebugHelpers
    {
        private static int _counter = 1;

        public static void WriteXmlFile(string text)
        {
            // New file for each print; print only even numbers (removes duplicates)
            _counter++;
            if (_counter % 2 != 0)
            {
                return;
            }

            // Pad string for filename
            var counter = (_counter / 2).ToString().PadLeft(3, '0');

            using (var sw = new StreamWriter("Output/AlienKnowledge" + counter + ".xml", false))
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

        public static void LogSemanticTest(string testResults)
        {
            using (var sw = new StreamWriter("Output/SemanticTests.txt", true))
            {
                var counter = _counter / 2;
                sw.WriteLine(counter + "\n" + testResults);
            }
        }

        public static void WriteFeedback(Feedback feedback)
        {
            using (var sw = new StreamWriter("Output/Feedback.txt", true))
            {
                sw.WriteLine(feedback);
            }
        }
    }
}