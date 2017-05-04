using Code.LearningEngine.Reality;

namespace Code.LearningEngine.Semantics.Model
{
    internal class LogicalModel
    {
        private readonly OrientationSet _orientations;
        private readonly SpatialRelationSet _spatialRelations;
        private readonly SpeciesSet _species;

        private LogicalModel(OrientationSet orientations, SpatialRelationSet spatialRelations,
            SpeciesSet species)
        {
            _orientations = orientations;
            _spatialRelations = spatialRelations;
            _species = species;
        }

        public OrientationSet Orientations
        {
            get { return _orientations; }
        }

        public SpatialRelationSet SpatialRelations
        {
            get { return _spatialRelations; }
        }

        public SpeciesSet Species
        {
            get { return _species; }
        }

        // Create logical model from situation
        public static LogicalModel Create(Situation situation)
        {
            // Add information about individual animals
            var animal1 = situation.Animal1;
            var animal2 = situation.Animal2;
            var orientations = OrientationSet
                .CreateEmtpy()
                .Add(animal1.RiverOrientation, Entity.Entity1)
                .Add(animal2.RiverOrientation, Entity.Entity2);
            var species = SpeciesSet
                .CreateEmpty()
                .Add(animal1.Species, Entity.Entity1)
                .Add(animal2.Species, Entity.Entity2);

            // Are animals on the same side?
            var sameSide = animal1.LatHemisphere == animal2.LatHemisphere;

            // Same distance to river?
            var equalRiverDistance = animal1.RiverDistance == animal2.RiverDistance;

            // Which animal is closer to the river?
            var closestToRiver = animal1.RiverDistance < animal2.RiverDistance
                ? Entity.Entity1
                : Entity.Entity2;
            var furthestFromRiver = closestToRiver == Entity.Entity1
                ? Entity.Entity2
                : Entity.Entity1;

            // Same longitude?
            var equalLongitude = animal1.Longitude == animal2.Longitude;

            // Which animal is furthest upstream/downstream?
            var furthestUpstream = animal1.Longitude > animal2.Longitude
                ? Entity.Entity1
                : Entity.Entity2;
            var furthestDownstream = furthestUpstream == Entity.Entity1
                ? Entity.Entity2
                : Entity.Entity1;

            // Emtpy set
            var spatialRelations0 = SpatialRelationSet.CreateEmpty();

            // Define river distance predicates
            var spatialRelations1 = equalRiverDistance
                // If same distance: don't add predicates
                ? spatialRelations0

                // Else: define sets for Towards and Away
                : spatialRelations0
                    .Add(RiverDirection.Towards, sameSide, closestToRiver, furthestFromRiver)
                    .Add(RiverDirection.Away, sameSide, furthestFromRiver, closestToRiver);

            // Define longitude predicates
            var spatialRelations2 = equalLongitude
                // If equal longitude: don't add predicates
                ? spatialRelations1

                // Else: define sets for Upstream and Downstream
                : spatialRelations1
                    .Add(RiverDirection.Upstream, sameSide, furthestUpstream, furthestDownstream)
                    .Add(RiverDirection.Downstream, sameSide, furthestDownstream, furthestUpstream);

            return new LogicalModel(orientations, spatialRelations2, species);
        }

        public static LogicalModel CreateEmpty()
        {
            return new LogicalModel(
                OrientationSet.CreateEmtpy(),
                SpatialRelationSet.CreateEmpty(),
                SpeciesSet.CreateEmpty());
        }
    }
}