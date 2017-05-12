using System.Collections;
using System.Diagnostics;
using System.IO;
using Code.Debugging;
using Code.LearningEngine.Agents;
using Code.LearningEngine.Languages;
using Code.LearningEngine.Reality;
using Code.LearningEngine.Semantics;
using Code.LearningEngine.Semantics.Model;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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

                // TODO: remove after debugging
                var sw = new Stopwatch();

                // Parent turn
                sw.Start();
                ParentTurn();
                sw.Stop();
                Debug.Log("Parent turn: " + sw.ElapsedMilliseconds);
                yield return new WaitForSeconds(delay / 10f);

                // Child turn
                sw.Reset();
                sw.Start();
                ChildTurn();
                sw.Stop();
                Debug.Log("Child turn: " + sw.ElapsedMilliseconds);
                yield return new WaitForSeconds(delay / 10f);

                // Evaluation turn
                sw.Reset();
                sw.Start();
                EvaluateSent();
                sw.Stop();
                Debug.Log("Evaluation turn: " + sw.ElapsedMilliseconds);
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

            // TODO: remove after testing
            _model.PrintSpecies();
            _model.PrintDirections();
            _model.PrintRelations();

            // Parent says something
            _parentAgent = _parentAgent.UpdateModel(_model).SaySomething();
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
                var feedback = _parentAgent.ProvideFeedback(sentence);
                FeedbackText.text = feedback.ToString();
                DebugHelpers.WriteFeedback(feedback);
                _childAgent = _childAgent.EvaluateFeedback(feedback);
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