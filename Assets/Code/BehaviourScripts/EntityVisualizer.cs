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
    internal void Visualize(Situation situation)
    {
        // Visualize scene
        ResetGameObjects();
        var gameObject1 = GetObject1(situation.Animal1);
        var gameObject2 = GetObject2(situation.Animal2);
        ProcessObject(gameObject1, situation.Animal1);
        ProcessObject(gameObject2, situation.Animal2);

        // Log
        Debug.Log("E1: " + situation.Animal1.Species + " Lat.: " + situation.Animal1.Latitude +
              " Long.: " + situation.Animal1.Longitude + " Hem.: " + situation.Animal1.LatHemisphere
              + " RiverDist.: " + situation.Animal1.RiverDistance + " Direction: "
              + situation.Animal1.RiverOrientation);
        Debug.Log("E2: " + situation.Animal2.Species + " Lat.: " + situation.Animal2.Latitude +
              " Long.: " + situation.Animal2.Longitude + " Hem.: " + situation.Animal2.LatHemisphere
              + " RiverDist.: " + situation.Animal2.RiverDistance + " Direction: "
              + situation.Animal2.RiverOrientation);


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

    private static void ProcessObject(GameObject gameObject, Animal animal)
    {
        // Activate
        gameObject.SetActive(true);

        // Set location
        SetLocation(gameObject, animal);

        // Set orientation
        SetOrientation(gameObject, animal);
    }

    private static void SetLocation(GameObject gameObject, Animal animal)
    {
        // Add extra vertical distance for entities north of river
        var skip = animal.Latitude > Situation.RiverLatitude ? 2 : 0;

        // Translate coordinates from abstract representation to Unity world
        var x = animal.Longitude * 10;
        var z = (animal.Latitude + skip) * 10;

        // Set location of GameObject
        gameObject.transform.Translate(new Vector3(x, 0, z));
    }

    private static void SetOrientation(GameObject gameObject, Animal animal)
    {
        gameObject.transform.GetChild(0).Rotate(new Vector3(0, GetRotation(animal), 0));
    }

    // Get rotation values based on entity orientation
    private static int GetRotation(Animal animal)
    {
        switch (animal.AbsoluteOrientation)
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
    private GameObject GetObject1(Animal animal)
    {
        switch (animal.Species)
        {
            case AnimalSpecies.Rabbit:
                return Rabbit1;
            case AnimalSpecies.Chair:
                return Chair1;
            case AnimalSpecies.Duck:
                return Duck1;
            case AnimalSpecies.Frog:
                return Frog1;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Get reference to GameObject corresopnding to entity2
    private GameObject GetObject2(Animal animal)
    {
        switch (animal.Species)
        {
            case AnimalSpecies.Rabbit:
                return Rabbit2;
            case AnimalSpecies.Chair:
                return Chair2;
            case AnimalSpecies.Duck:
                return Duck2;
            case AnimalSpecies.Frog:
                return Frog2;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}
