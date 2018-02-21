using System;
using System.Collections.Generic;

namespace YogurtTheHorse.Messenger.LayoutGenerator.Descriptions {
    public struct VariableDescription {
        public string VariableType { get; set; }
        public string VariableNamespace { get; set; }
        public string Convertion { get; set; }
        public string Name { get; set; }

        public VariableDescription(string name, dynamic d) {
            Name = name;
            VariableNamespace = d.type_namespace;
            VariableType = d.type;


            if (d.convert_type != null && d.method == null) {
                throw new KeyNotFoundException($"No method property for {name} variable");
            } else {
                switch (d.convert_type as string) {
                    case "static_method":
                        Convertion = d.method.ToString() + "($value$)";
                        break;

                    case "constructor":
                        Convertion = $"new {VariableType}($value$)";
                        break;

                    case "other":
                        Convertion = d.method;
                        break;

                    case null:
                        Convertion = "$value";
                        break;

                    default:
                        throw new InvalidOperationException("Unknown convertion method");
                }
            }
        }
    }
}
