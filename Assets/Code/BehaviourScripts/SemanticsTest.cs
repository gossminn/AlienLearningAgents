using LearningEngine;
using UnityEngine;

public class SemanticsTest : MonoBehaviour
{

	// Use this for initialization
    private void Start ()
    {
//        var i = 0;
//	    foreach (var entity in Entity.GenerateAll())
//	    {
//	        Debug.Log("ID: " + i++ +
//	                  ", Species: " + entity.Species +
//	                  ", Lat.: " + entity.Latitude +
//	                  ", Long.: " + entity.Longitude +
//	                  ", Abs.: " + entity.AbsoluteOrientation +
//	                  ", Riv.: " + entity.RiverOrientation);
//	    }

        var world = Situation.Generate();
        Debug.Log("E1: " + world.Animal1.Species + " Lat.: " + world.Animal1.Latitude +
                  " Long.: " + world.Animal1.Longitude + " Hem.: " + world.Animal1.LatHemisphere);
        Debug.Log("E2: " + world.Animal2.Species + " Lat.: " + world.Animal2.Latitude +
                  " Long.: " + world.Animal2.Longitude + " Hem.: " + world.Animal2.LatHemisphere);
        var visualizer = GetComponent<EntityVisualizer>();
        visualizer.Visualize(world);
    }
}
