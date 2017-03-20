using LearningEngine;
using UnityEngine;

public class AlienTester : MonoBehaviour
{
    private ParentAgent parentAgent;
    private TextMesh textMesh;

	// Use this for initialization
	void Start ()
    {
        parentAgent = AlienLanguage.MakeParentAgent();
        textMesh = GetComponent<TextMesh>();
	}

    // Update: called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            parentAgent = parentAgent.SaySomething();
            textMesh.text = parentAgent.CurrentSentence;
        }
    }
}
