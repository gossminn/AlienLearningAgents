using Code.LearningEngine.Agents;
using Code.LearningEngine.Languages;
using Code.LearningEngine.Reality;
using Code.LearningEngine.Semantics;
using Code.LearningEngine.Semantics.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Code.BehaviourScripts
{
    public class SimulationController : MonoBehaviour
    {
        // Agents
        private ChildAgent _childAgent;
        private ParentAgent _parentAgent;

        // Model
        private LogicalModel _model;

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

        // Visualizer
        public EntityVisualizer Visualizer;

        // Use this for initialization
        private void Start()
        {
            // Initialize agents
            _parentAgent = AlienLanguage.MakeParentAgent();
            _childAgent = ChildAgent.Initialize();

            // Add listeners for buttons
            ParentButton.onClick.AddListener(ParentTurn);
            ChildButton.onClick.AddListener(ChildTurn);
            EvaluateButton.onClick.AddListener(EvaluateSent);
            RepeatButton.onClick.AddListener(RunSimulation);

            // Initialize visualizer
            Visualizer = GetComponent<EntityVisualizer>();
        }

        private void ParentTurn()
        {
            // Generate and visualize new situation
            var situation = Situation.Generate();
            Visualizer.Visualize(situation);

            // Create model from situation
            _model = LogicalModel.Create(situation);

            // TODO: remove after testing
            _model.PrintSpecies();
            _model.PrintDirections();
            _model.PrintRelations();

            // Parent says something
            _parentAgent = _parentAgent.UpdateModel(_model).SaySomething();
            ParentText.text = _parentAgent.CurrentSentence;

            // TODO: remove after debugging
            Debug.Log(_parentAgent.EvaluateSentence(_parentAgent.CurrentSentence));

        }

        private void ChildTurn()
        {
            // Child processes input; will produce a sentence or remain silent
            _childAgent = _childAgent.ProcessInput(_parentAgent.CurrentSentence, _model);
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
                ParentTurn();
                ChildTurn();
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
}