﻿using Newtonsoft.Json.Linq;
using System.Collections.Immutable;
using System.Globalization;

namespace ChalitaLearning.Configurations.Amazon
{
    internal class JsonConfigurationParser
    {
        private readonly Stack<string> _context = new Stack<string>();
        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private string _currentPath;

        private JsonConfigurationParser()
        {
        }

        public static JsonConfigurationParser Instance => new JsonConfigurationParser();

        public static IDictionary<string, string> Parse(JObject input)
        {
            return new JsonConfigurationParser().ParseObject(input);
        }

        public IDictionary<string, string> ParseObject(JObject input)
        {
            _data.Clear();
            VisitJObject(input);

            return _data;
        }

        private void VisitJObject(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                EnterContext(property.Name);
                VisitProperty(property);
                ExitContext();
            }
        }

        private void VisitProperty(JProperty property)
        {
            VisitToken(property.Value);
        }

        private void VisitToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    VisitJObject(token.Value<JObject>());

                    break;
                case JTokenType.Array:
                    VisitArray(token.Value<JArray>());

                    break;
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Null:
                case JTokenType.Raw:
                case JTokenType.Bytes:
                    VisitPrimitive(token.Value<JValue>());

                    break;
                default:
                    throw new FormatException($"Unsupported json type: {token.Type} - path: {token.Path}");
            }
        }

        private void VisitArray(JArray array)
        {
            for (var index = 0; index < array.Count; ++index)
            {
                EnterContext(index.ToString());
                VisitToken(array[index]);
                ExitContext();
            }
        }

        private void VisitPrimitive(JValue data)
        {
            var currentPath = _currentPath;

            if (_data.ContainsKey(currentPath))
            {
                throw new FormatException($"Duplicated key: {currentPath}");
            }

            _data[currentPath] = data.ToString(CultureInfo.InvariantCulture);
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}
