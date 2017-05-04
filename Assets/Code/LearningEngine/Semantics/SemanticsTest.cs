using Code.LearningEngine.Semantics.Functions;
using Code.LearningEngine.Semantics.Model;
using UnityEngine;

namespace Code.LearningEngine.Semantics
{
    internal static class SemanticsTest
    {
        public static void PrintSpecies(this LogicalModel m)
        {
            var output = "SPECIES TEST\n" + "++++++++++++++\n";
            foreach (var meaning in AtomicMeanings.Species)
            {
                output += meaning(m).AppliesToModel(m);
            }
            Debug.Log(output);
        }

        public static void PrintDirections(this LogicalModel m)
        {
            var output = "DIRECTIONS TEST\n" + "++++++++++++++\n";
            foreach (var meaning in AtomicMeanings.Directions)
            {
                output += meaning(m).AppliesToModel(m);
            }
            Debug.Log(output);
        }

        public static void PrintRelations(this LogicalModel m)
        {
            var output = "RELATIONS TEST\n" + "++++++++++++++\n";
            foreach (var meaning in AtomicMeanings.SpatialRelations)
            {
                output += meaning(m).AppliesToModel(m);
            }
            Debug.Log(output);
        }
    }
}