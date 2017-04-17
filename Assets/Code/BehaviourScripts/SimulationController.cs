using LearningEngine;
using UnityEngine;
using UnityEngine.UI;

public class SimulationController : MonoBehaviour
{
    // Agents
    private ChildAgent _childAgent;
    private ParentAgent _parentAgent;

    // UI: buttons
    public Button ChildButton;
    public Button ParentButton;
    public Button EvaluateButton;
    public Button RepeatButton;

    // UI: text meshes
    public Text ChildText;
    public Text ParentText;

    // UI: text input
    public InputField RepeatInput;
    
    // Use this for initialization
    private void Start()
    {
        // Initialize agents
        _parentAgent = AlienLanguage.MakeParentAgent();
        _childAgent = ChildAgent.Initialize();

        // Add listeners for buttons
        ParentButton.onClick.AddListener(ParentSay);
        ChildButton.onClick.AddListener(ChildSay);
        EvaluateButton.onClick.AddListener(EvaluateSent);
        RepeatButton.onClick.AddListener(RunSimulation);
    }

    private void ParentSay()
    {
        _parentAgent = _parentAgent.SaySomething();
        ParentText.text = _parentAgent.CurrentSentence;
    }

    private void ChildSay()
    {
        _childAgent = _childAgent.Learn(_parentAgent.CurrentSentence);
        _childAgent = _childAgent.SaySomething();
        ChildText.text = _childAgent.Current;
    }

    private void EvaluateSent()
    {
        var sentence = _childAgent.Current;
        var feedback = _parentAgent.ProvideFeedback(sentence);
        ParentText.text = feedback.ToString();
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
        return int.TryParse(RepeatInput.text, out n) ? n : 0;
    }
}