using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using YamlDotNet.RepresentationModel;

namespace YogurtTheHorse.Messenger.LayoutGenerator {
    public class DynamicYaml : DynamicObject, IEnumerable {
        private YamlNode _node;
        private bool _nullIfNotExist;

        public DynamicYaml(YamlNode node) : this(node, false) { }

        public DynamicYaml(YamlNode node, bool nullIfNotExist) {
            _node = node;
            _nullIfNotExist = nullIfNotExist;
        }

        private object NodeToObject(YamlNode node) {
            switch (node.NodeType) {
                case YamlNodeType.Scalar:
                    var strValue = (node as YamlScalarNode).Value;
                    if (int.TryParse(strValue, out int intValue)) {
                        return intValue;
                    }

                    if (bool.TryParse(strValue, out bool boolValue)) {
                        return boolValue;
                    }

                    return strValue;

                case YamlNodeType.Mapping:
                case YamlNodeType.Sequence:
                    return new DynamicYaml(node, _nullIfNotExist);

                default:
                    return null;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            switch (_node.NodeType) {
                case YamlNodeType.Mapping:
                    foreach (var nodeKeyPair in ((YamlMappingNode)_node).Children) {
                        if (((YamlScalarNode)nodeKeyPair.Key).Value == binder.Name) {
                            result = NodeToObject(nodeKeyPair.Value);
                            return true;
                        }
                    }
                    result = null;
                    break;

                case YamlNodeType.Scalar:
                    result = (_node as YamlScalarNode).Value;
                    return true;
            }

            result = null;
            return _nullIfNotExist;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
            if (_node.NodeType == YamlNodeType.Sequence) {
                var seq = _node as YamlSequenceNode;
                result = NodeToObject(seq[(int)indexes[0]]);
                return true;
            }

            result = null;
            return _nullIfNotExist;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            if (_node.NodeType == YamlNodeType.Sequence) {
                foreach (var node in (_node as YamlSequenceNode).Children) {
                    yield return NodeToObject(node);
                }
            } else if (_node.NodeType == YamlNodeType.Mapping) {
                foreach (var node in (_node as YamlMappingNode).Children) {
                    yield return new KeyValuePair<string, object>(NodeToObject(node.Key).ToString(), NodeToObject(node.Value));
                }
            } else {
                throw new InvalidOperationException();
            }
        }
    }
}
