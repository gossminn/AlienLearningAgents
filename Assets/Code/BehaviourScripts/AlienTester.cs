using LearningEngine;
using UnityEngine;
using UnityEngine.UI;

public class AlienTester : MonoBehaviour
{
    private ParentAgent parentAgent;
    private ChildAgent childAgent;
    public TextMesh parentText;
    public TextMesh childText;
    public Button parentButton;
    public Button childButton;
    public Button evaluateButton;

	// Use this for initialization
	void Start ()
    {
        parentAgent = AlienLanguage.MakeParentAgent();
        childAgent = ChildAgent.Initialize();
        parentButton.onClick.AddListener(ParentSay);
        childButton.onClick.AddListener(ChildSay);
        evaluateButton.onClick.AddListener(EvaluateSent);
	}

    private void ParentSay()
    {
        parentAgent = parentAgent.SaySomething();
        parentText.text = parentAgent.CurrentSentence;
    }

    private void ChildSay()
    {
        childAgent = childAgent.Learn(parentAgent.CurrentSentence);
        childAgent = childAgent.SaySomething();
        childText.text = childAgent.CurrentSentence;
    }

    private void EvaluateSent()
    {
        var sentence = childAgent.CurrentSentence;
        var feedback = parentAgent.ProvideFeedback(sentence);
        parentText.text = feedback.ToString();
        childAgent = childAgent.EvaluateFeedback(feedback);
    }
}
