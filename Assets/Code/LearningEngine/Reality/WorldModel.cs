using System;

namespace LearningEngine
{

    internal class WorldModel
    {
        // Constant: location of the river (= north/south boundary)
        public const int RiverLatitude = 4;

        // Constant: east/west boundary
        public const int EastWestBoundary = 4;

        // Entities
        private readonly EntityModel _entity1;
        private readonly EntityModel _entity2;

        // Private constructor
        private WorldModel(EntityModel entity1, EntityModel entity2)
        {
            _entity1 = entity1;
            _entity2 = entity2;
        }

        public EntityModel Entity1
        {
            get { return _entity1; }
        }

        public EntityModel Entity2
        {
            get { return _entity2; }
        }

        // Generate random situation
        public static WorldModel Generate()
        {
            return new WorldModel(EntityModel.Generate(), EntityModel.Generate());
        }
    }
}