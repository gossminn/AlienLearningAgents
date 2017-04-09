using LearningEngine;
using UnityEngine;
using UnityEngine.UI;

public class AlienTester : MonoBehaviour
{
    private ChildAgent _childAgent;
    public Button _childButton;
    public TextMesh _childText;
    public Button _evaluateButton;
    private ParentAgent _parentAgent;
    public Button _parentButton;
    public TextMesh _parentText;
    public InputField _repeatInput;
    public Button _repeatButton;


    // Use this for initialization
    private void Start()
    {
        // Initialize agents
        _parentAgent = AlienLanguage.MakeParentAgent();
        _childAgent = ChildAgent.Initialize();

        // Add listeners for buttons
        _parentButton.onClick.AddListener(ParentSay);
        _childButton.onClick.AddListener(ChildSay);
        _evaluateButton.onClick.AddListener(EvaluateSent);
        _repeatButton.onClick.AddListener(RunSimulation);
    }

    private void ParentSay()
    {
        _parentAgent = _parentAgent.SaySomething();
        _parentText.text = _parentAgent.CurrentSentence;
    }

    private void ChildSay()
    {
        _childAgent = _childAgent.Learn(_parentAgent.CurrentSentence);
        _childAgent = _childAgent.SaySomething();
        _childText.text = _childAgent.Current;
    }

    private void EvaluateSent()
    {
        var sentence = _childAgent.Current;
        var feedback = _parentAgent.ProvideFeedback(sentence);
        _parentText.text = feedback.ToString();
        _childAgent = _childAgent.EvaluateFeedback(feedback);
    }
    
    // Run the simulation automatically (repeat n times)
    private void RunSimulation()
    {
        var repetitions = ParseRepetitions();
        for (var i = 0; i < repetitions; i++)
        {
            ParentSay();
            ChildSay();
            //EvaluateSent(); TODO: enable when implemented
        }
    }

    // Get number of repetitions from text field
    private int ParseRepetitions()
    {
        int n;
        return int.TryParse(_repeatInput.text, out n) ? n : 0;
    }
}