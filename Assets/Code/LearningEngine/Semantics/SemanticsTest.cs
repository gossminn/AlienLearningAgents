using UnityEngine;

namespace LearningEngine
{
    internal static class SemanticsTest
    {
        public static void PrintSpecies(this LogicalModel m)
        {
            var output = "SPECIES TEST\n" + "++++++++++++++\n";

            foreach (var meaning in AtomicMeanings.Species)
            {
                var evaluation = (TypeEtValue) meaning(m);
                var e1True = evaluation.Value(TypeEValue.Create(Entity.Entity1)).Value;
                var e2True = evaluation.Value(TypeEValue.Create(Entity.Entity2)).Value;
                output += "E1: " + e1True + " E2: " + e2True + "\n";
            }

            Debug.Log(output);
        }

        public static void PrintDirections(this LogicalModel m)
        {
            var output = "DIRECTIONS TEST\n" + "++++++++++++++\n";

            foreach (var direction in AtomicMeanings.Directions)
            {
                var evaluation = ((TypeEtEValue) direction(m)).Value;
                foreach (var species in AtomicMeanings.Species)
                {
                    var speciesEval = (TypeEtValue) species(m);
                    output += evaluation(speciesEval).Value;
                }
                output += "\n";
            }

            Debug.Log(output);
        }

        public static void PrintRelations(this LogicalModel m)
        {
            var output = "RELATIONS TEST\n" + "++++++++++++++\n";

            foreach (var relation in AtomicMeanings.SpatialRelations)
            {
                var evaluation = ((TypeEEtValue) relation(m)).Value;
                var entities = new[] {Entity.Entity1, Entity.Entity2};
                foreach (var entity1 in entities)
                {
                    foreach (var entity2 in entities)
                    {
                        var x = evaluation(TypeEValue.Create(entity1))
                            .Value(TypeEValue.Create(entity2));
                        if (x.Value)
                        {
                            output += entity1 + "," + entity2;
                        }
                    }
                }
                output += "\n";
            }

            Debug.Log(output);
        }
    }
}