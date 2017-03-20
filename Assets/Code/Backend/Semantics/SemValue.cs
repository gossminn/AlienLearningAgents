using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace LearningEngine
{
    class SemValue
    {
        private readonly string _value;
        public string Value { get {return _value; } }

        public SemValue(string value)
        {
            _value = value;
        }

        // Apply argument to lambda string. Returns itself on failure.
        public SemValue LambdaApply(SemValue argument)
        {
            if (!_value.StartsWith("/"))
            {
                return this;
            }

            var split = Regex.Split(_value, @"([\[\]])")
                .Where(x => !String.IsNullOrEmpty(x)).ToImmutableList();
            var bound = split[0].Trim(new char[] { '/' });
            var resultList = split
                .RemoveAt(split.Count - 1)
                .RemoveRange(0, 2);
            var resultString = String.Join("", resultList.ToArray())
                .Replace(bound, argument.Value);
            return new SemValue(resultString);
        }
    }
}
