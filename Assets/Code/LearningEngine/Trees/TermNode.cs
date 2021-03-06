﻿using System;
using Code.LearningEngine.Semantics.Model;
using Code.LearningEngine.Semantics.Types;
using Code.LearningEngine.Syntax;

namespace Code.LearningEngine.Trees
{
    internal class TermNode : ITreeNode
    {
        // Semantics
        private readonly Func<LogicalModel,ISemanticValue> _evalFunction;

        // Syntactic category
        private readonly CategoryLabel _synCat;

        // Written form
        private readonly string _writtenForm;

        // Private constructor
        private TermNode(CategoryLabel synCat, string writtenForm, Func<LogicalModel, ISemanticValue> evalFunction)
        {
            _synCat = synCat;
            _writtenForm = writtenForm;
            _evalFunction = evalFunction;
        }

        public CategoryLabel Category
        {
            get { return _synCat; }
        }


        public ISemanticValue GetSemantics(LogicalModel model)
        {
            return _evalFunction(model);
        }

        public bool GetTruthValue()
        {
            return false;
        }

        // Factory method for creating new instances
        public static TermNode Create(CategoryLabel synCat, string writtenForm,
            Func<LogicalModel, ISemanticValue> evalFunction)
        {
            return new TermNode(synCat, writtenForm, evalFunction);
        }

        public TermNode UpdateMeaning(Func<LogicalModel, ISemanticValue> evalFunction)
        {
            return new TermNode(_synCat, _writtenForm, evalFunction);
        }

        // Get as flat string
        public string GetFlatString()
        {
            return _writtenForm;
        }

        // Generate XML representation
        public string GetXmlString()
        {
            return "<" + _synCat + " value=" + _evalFunction.GetHashCode()
                   + ">" + _writtenForm + "</" + _synCat + ">";
        }

    }
}