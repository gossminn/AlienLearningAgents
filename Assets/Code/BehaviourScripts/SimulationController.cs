using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Code.Debugging;
using Code.LearningEngine.Agents;
using Code.LearningEngine.Languages;
using Code.LearningEngine.Reality;
using Code.LearningEngine.Semantics.Model;

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
        public Button RunButton;
        public Button StopButton;

        // UI: texts
        public InputField ChildText;
        public InputField ParentText;
        public InputField FeedbackText;

        // UI: counter
        public Text CountText;
        private int _counter;

        // UI: text input
        public InputField RepeatInput;
        public InputField DelayInput;

        // Continue or not?
        private bool _continue;

        // Visualizer
        public EntityVisualizer Visualizer;

        // Use this for initialization
        private void Start()
        {
            // Directory for output files
            Directory.CreateDirectory("Output");

            // Initialize agents
            _parentAgent = AlienLanguage.MakeParentAgent();
            _childAgent = ChildAgent.Initialize();

            // Add listeners for buttons
            RunButton.onClick.AddListener(() => StartCoroutine("RunSimulation"));
            StopButton.onClick.AddListener(() => StopCoroutine("RunSimulation"));

            // Initialize visualizer
            Visualizer = GetComponent<EntityVisualizer>();
        }

        // Run the simulation automatically (repeat n times)
        private IEnumerator RunSimulation()
        {
            var delay = GetDelay();
            var repetitions = GetRepetitions();
            for (var i = 0; i < repetitions; i++)
            {
                // Display counter
                _counter++;
                CountText.text = _counter.ToString();

                // Set texts to empty
                ParentText.text = "";
                ChildText.text = "";
                FeedbackText.text = "";

                // Parent turn
                ParentTurn();
                yield return new WaitForSeconds(delay / 10f);

                // Child turn
                ChildTurn();
                yield return new WaitForSeconds(delay / 10f);

                // Evaluation turn
                EvaluateSent();
                yield return new WaitForSeconds(delay / 10f);
            }
        }

        private void ParentTurn()
        {
            // Generate and visualize new situation
            var situation = Situation.Generate();
            Visualizer.Visualize(situation);

            // Create model from situation
            _model = LogicalModel.Create(situation);

            // Parent says something
            _parentAgent = _parentAgent.SaySomething(_model);
            ParentText.text = _parentAgent.CurrentSentence;
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
            if (sentence != "")
            {
                var feedback = _parentAgent.ProvideFeedback(sentence, _model);
                FeedbackText.text = feedback.ToString();
                _childAgent = _childAgent.EvaluateFeedback(feedback);

                DebugHelpers.WriteFeedback(feedback);
            }
        }

        // Get delay
        private int GetDelay()
        {
            return ParseInputNumber(DelayInput);
        }

        // Get repetitions
        private float GetRepetitions()
        {
            return ParseInputNumber(RepeatInput);
        }

        // Parse int from textfield
        private static int ParseInputNumber(InputField input)
        {
            int n;
            return int.TryParse(input.text, out n) ? n : 0;
        }
    }
}