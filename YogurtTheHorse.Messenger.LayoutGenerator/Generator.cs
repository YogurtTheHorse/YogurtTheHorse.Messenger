using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;

using YogurtTheHorse.Messenger.LayoutGenerator.Descriptions;
using YogurtTheHorse.Messenger.LayoutGenerator.Templates;

using static YogurtTheHorse.Messenger.LayoutGenerator.Descriptions.ButtonDescription;

namespace YogurtTheHorse.Messenger.LayoutGenerator {
    public class Generator {
        private dynamic _doc;
        private string _outputPath;

        private List<string> _defaultNamespaces;
        private Dictionary<string, ButtonDescription> _buttonsDescriptions;
        private Dictionary<string, LayoutDescription> _layoutsDescriptions;
        private Dictionary<string, MenuDescription> _menusDescriptions;
        private Dictionary<string, VariableDescription> _userVariablesDescriptions;


        public string Filepath { get; private set; }
        public string DefaultNamespace { get; private set; }
        public string UserDataTypeName { get; private set; }

        public Generator(string filepath) {
            Filepath = filepath;

            _defaultNamespaces = new List<string>();
            _userVariablesDescriptions = new Dictionary<string, VariableDescription>();
            _buttonsDescriptions = new Dictionary<string, ButtonDescription>();
            _layoutsDescriptions = new Dictionary<string, LayoutDescription>();
            _menusDescriptions = new Dictionary<string, MenuDescription>();
        }

        #region Generating methods
        public void Generate() {
            var input = new StreamReader(Filepath);
            var yamlReader = new YamlStream();
            yamlReader.Load(input);

            _doc = new DynamicYaml(yamlReader.Documents[0].RootNode, true);

            Register();
            GenerateFiles();
        }

        private void GenerateFiles() {
            var buttonsDirectory = _outputPath + @"\Buttons";
            var layoutsDirectory = _outputPath + @"\Layouts";
            var menusDirectory = _outputPath + @"\Menus";

            UserDataTemplate ud = new UserDataTemplate {
                Session = new Dictionary<string, object> {
                    ["generator"] = this
                }
            };
            ud.Initialize();

            if (!Directory.Exists(_outputPath)) {
                Directory.CreateDirectory(_outputPath);
            }

            File.WriteAllText($@"{_outputPath}\{UserDataTypeName}.cs", ud.TransformText());

            // TODO: Duplicate code??
            if (_buttonsDescriptions.Count > 0 && !Directory.Exists(buttonsDirectory)) {
                Directory.CreateDirectory(buttonsDirectory);
            }

            foreach (ButtonDescription buttonDescription in _buttonsDescriptions.Values) {
                CreateButtonFile(buttonsDirectory, buttonDescription);
            }

            if (_layoutsDescriptions.Count > 0 && !Directory.Exists(layoutsDirectory)) {
                Directory.CreateDirectory(layoutsDirectory);
            }

            foreach (LayoutDescription layoutDescription in _layoutsDescriptions.Values) {
                CreateLayoutFile(layoutsDirectory, layoutDescription);
            }

            if (_menusDescriptions.Count > 0 && !Directory.Exists(menusDirectory)) {
                Directory.CreateDirectory(menusDirectory);
            }

            foreach (MenuDescription menuDescription in _menusDescriptions.Values) {
                CreateMenuFile(menusDirectory, menuDescription);
            }
        }

        private void CreateButtonFile(string buttonsDirectory, ButtonDescription buttonDescription) {
            dynamic buttonTemplate;

            switch (buttonDescription.ButtonType) {
                case EButtonType.Navigate:
                    buttonTemplate = new NavigateButtonTemplate();
                    break;

                case EButtonType.Inline:
                    buttonTemplate = new InlineButtonTemplate();
                    break;

                case EButtonType.Input:
                    buttonTemplate = new InputButtonTemplate();
                    break;

                default:
                    throw new NotSupportedException($"Unsupported button type \"{buttonDescription.ButtonType}\" at {buttonDescription.Name}");
            }

            var session = new Dictionary<string, object> {
                ["generator"] = this,
                ["buttonDescription"] = buttonDescription
            };

            buttonTemplate.Session = session;
            buttonTemplate.Initialize();

            string textToWrite = buttonTemplate.TransformText();
            // Seems like bug in T4, so i need to substring
            textToWrite = textToWrite.Substring(2);

            File.WriteAllText($@"{buttonsDirectory}\{buttonDescription.Name}.cs", textToWrite);
        }

        private void CreateLayoutFile(string layoutsDirectory, LayoutDescription layoutDescription) {
            LayoutTemplate layoutTemplate = new LayoutTemplate() {
                Session = new Dictionary<string, object> {
                    ["generator"] = this,
                    ["layoutDescription"] = layoutDescription
                }
            };
            layoutTemplate.Initialize();

            File.WriteAllText($@"{layoutsDirectory}\{layoutDescription.Name}.cs", layoutTemplate.TransformText());
        }

        private void CreateMenuFile(string menusDirectory, MenuDescription menuDescription) {
            MenuTemplate menuTemplate = new MenuTemplate() {
                Session = new Dictionary<string, object> {
                    ["generator"] = this,
                    ["menuDescription"] = menuDescription
                }
            };
            menuTemplate.Initialize();

            File.WriteAllText($@"{menusDirectory}\{menuDescription.Name}.cs", menuTemplate.TransformText());
        }
        #endregion

        #region Getting variables methods
        public IEnumerable<VariableDescription> GetUserVariablesDescriptions() {
            return _userVariablesDescriptions.Values;
        }

        public ButtonDescription GetButtonDescription(dynamic btn) {
            if (btn is string buttonName) {
                return
                    _buttonsDescriptions.TryGetValue(buttonName, out ButtonDescription ButtonDescription) ?
                        ButtonDescription : throw new KeyNotFoundException($"No button {buttonName} found");
            } else {
                return new ButtonDescription(btn);
            }
        }

        public LayoutDescription GetLayout(string layoutName) {
            if (layoutName == null) { return new LayoutDescription(); }

            return _layoutsDescriptions.TryGetValue(layoutName, out LayoutDescription layout) ?
                layout :
                throw new KeyNotFoundException($"{layoutName} nayout not found.");
        }
        #endregion

        #region Namespaces formatting methods
        public string GetDefaultNamespacesString() {
            return FormatedNamespaces(_defaultNamespaces);
        }

        public string GetUserDataNamespacesString() {
            HashSet<string> namespaces = new HashSet<string>(_defaultNamespaces);

            foreach (var vd in _userVariablesDescriptions.Values) {
                namespaces.Add(vd.VariableNamespace);
            }

            return FormatedNamespaces(namespaces);
        }

        public static string FormatedNamespaces(IEnumerable<string> namespaces) {
            var strings = namespaces.Where(ns => !String.IsNullOrEmpty(ns)).Select(ns => $"using {ns};");
            return String.Join("\n", strings);
        }
        #endregion

        #region Registration methods
        private void Register() {
            _outputPath = _doc.output_path ?? throw new KeyNotFoundException("Output path is not specified");
            _outputPath = Path.GetDirectoryName(Filepath) + "\\" + _outputPath;

            DefaultNamespace = _doc.default_namespace ?? throw new KeyNotFoundException("Default namespace is not specified");
            UserDataTypeName = _doc.user_data?.name ?? throw new KeyNotFoundException("User data type is not specified");

            // TODO: seems like bad code. I'm not sure.
            foreach (var ns in _doc.default_namespaces ?? new object[] { }) {
                _defaultNamespaces.Add(ns);
            }

            foreach (var userVariable in _doc.user_data?.variables ?? new object[] { }) {
                RegisterVariable(userVariable.Key, userVariable.Value);
            }

            foreach (var button in _doc.buttons ?? new object[] { }) {
                RegisterButton(button.Key, button.Value);
            }

            foreach (var layout in _doc.layouts ?? new object[] { }) {
                RegisterLayout(layout.Key, layout.Value);
            }

            foreach (var menu in _doc.menus ?? new object[] { }) {
                RegisterMenu(menu.Key, menu.Value);
            }
        }

        private void RegisterVariable(string name, dynamic value) {
            _userVariablesDescriptions[name] = new VariableDescription(name, value);
        }

        private void RegisterButton(string name, dynamic btn) {
            _buttonsDescriptions[name] = new ButtonDescription(name, btn);
        }

        private void RegisterLayout(string name, dynamic value) {
            _layoutsDescriptions[name] = new LayoutDescription(this, name, value);
        }

        private void RegisterMenu(string name, dynamic value) {
            VariableDescription? variableToAssign = null;
            if (value.variable_name != null) {
                if (_userVariablesDescriptions.ContainsKey(value.variable_name)) {
                    variableToAssign = _userVariablesDescriptions[value.variable_name];

                    if (value.menu_to_open == null) {
                        throw new ArgumentNullException("menu_to_open", "menu_to_open can't be null if menu is for variable input");
                    }
                } else {
                    throw new KeyNotFoundException($"{value.variable_name} variable not found.");
                }
            }

            _menusDescriptions[name] = new MenuDescription(
                name: name,
                startMessage: value.start_message,
                layoutName: value.layout,
                variableToAssign: variableToAssign,
                menuToOpenAfterAssign: variableToAssign == null ? null : value.menu_to_open);
        }
        #endregion
    }
}
