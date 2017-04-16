using System;
using LearningEngine;
using UnityEngine;

public class EntityVisualizer : MonoBehaviour
{
    // Entities (as GameObjects)
    public GameObject Rabbit1;
    public GameObject Rabbit2;
    public GameObject Chair1;
    public GameObject Chair2;
    public GameObject Duck1;
    public GameObject Duck2;
    public GameObject Frog1;
    public GameObject Frog2;

    // Array for managing GameObjects at once
    private GameObject[] _gameObjects;

    private void Start()
    {
        // Bundle entities in array
        _gameObjects = new[] { Rabbit1, Rabbit2, Chair1, Chair2, Duck1, Duck2, Frog1, Frog2};
    }

    // Visualizaton method
    private void Visualize(WorldModel world)
    {
        ResetGameObjects();
        var gameObject1 = GetObject1(world.Entity1);
        var gameObject2 = GetObject2(world.Entity2);
        ProcessObject(gameObject1, world.Entity1);
        ProcessObject(gameObject2, world.Entity2);
    }

    // Reset game objects
    private void ResetGameObjects()
    {
        foreach (var obj in _gameObjects)
        {
            // Set to inactive
            obj.SetActive(false);

            // Reset location
            obj.transform.position = new Vector3(0, 3, 0);

            // Reset rotation
            obj.transform.GetChild(0).rotation = Quaternion.identity;
        }
    }

    private static void ProcessObject(GameObject gameObject, EntityModel entity)
    {
        // Activate
        gameObject.SetActive(true);

        // Set location
        SetLocation(gameObject, entity);

        // Set orientation
        SetOrientation(gameObject, entity);
    }

    private static void SetLocation(GameObject gameObject, EntityModel entity)
    {
        // Add extra vertical distance for entities north of river
        var skip = entity.Latitude > WorldModel.RiverLatitude ? 2 : 0;

        // Translate coordinates from abstract representation to Unity world
        var x = (entity.Longitude + skip) * 20;
        var z = (entity.Latitude + skip) * 10;

        // Set location of GameObject
        gameObject.transform.Translate(new Vector3(x, 0, z));
    }

    private static void SetOrientation(GameObject gameObject, EntityModel entity)
    {
        gameObject.transform.GetChild(0).Rotate(new Vector3(0, GetRotation(entity), 0));
    }

    // Get rotation values based on entity orientation
    private static int GetRotation(EntityModel entity)
    {
        switch (entity.Orientation)
        {
            case CardinalDirection.North:
                return 0;
            case CardinalDirection.South:
                return 180;
            case CardinalDirection.East:
                return 90;
            case CardinalDirection.West:
                return 270;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Get reference to GameObject corresopnding to entity1
    private GameObject GetObject1(EntityModel entity)
    {
        switch (entity.Species)
        {
            case EntitySpecies.Rabbit:
                return Rabbit1;
            case EntitySpecies.Chair:
                return Chair1;
            case EntitySpecies.Duck:
                return Duck1;
            case EntitySpecies.Frog:
                return Frog1;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Get reference to GameObject corresopnding to entity2
    private GameObject GetObject2(EntityModel entity)
    {
        switch (entity.Species)
        {
            case EntitySpecies.Rabbit:
                return Rabbit2;
            case EntitySpecies.Chair:
                return Chair2;
            case EntitySpecies.Duck:
                return Duck2;
            case EntitySpecies.Frog:
                return Frog2;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}
