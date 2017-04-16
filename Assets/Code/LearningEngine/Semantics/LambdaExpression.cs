using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace LearningEngine
{
    internal class LambdaExpression
    {
        private readonly string _value;

        public LambdaExpression(string value)
        {
            _value = value;
        }

        public string Value
        {
            get
            {
                return _value == "" ? "NONE" : _value;
            }
        }

        // Apply argument to lambda string. Returns itself on failure.
        public LambdaExpression LambdaApply(LambdaExpression argument)
        {
            if (!_value.StartsWith("/"))
                return this;

            var split = Regex.Split(_value, @"([\[\]])")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToImmutableList();
            var bound = split[0].Trim('/');
            var resultList = split
                .RemoveAt(split.Count - 1)
                .RemoveRange(0, 2);
            var resultString = string.Join("", resultList.ToArray())
                .Replace(bound, argument.Value);
            return new LambdaExpression(resultString);
        }
    }
}