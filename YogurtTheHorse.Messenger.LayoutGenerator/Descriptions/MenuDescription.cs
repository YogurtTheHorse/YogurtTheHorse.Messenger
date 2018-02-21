namespace YogurtTheHorse.Messenger.LayoutGenerator.Descriptions {
    public struct MenuDescription {
        public string Name { get; }
        public string StartMessage { get; }
        public string LayoutName { get; }

        public VariableDescription? VariableToAssign { get; }
        public string MenuToOpenAfterAssign { get; }

        public MenuDescription(string name, string startMessage, string layoutName, VariableDescription? variableToAssign, string menuToOpenAfterAssign) {
            Name = name;
            StartMessage = startMessage;
            LayoutName = layoutName;
            VariableToAssign = variableToAssign;
            MenuToOpenAfterAssign = menuToOpenAfterAssign;
        }
    }
}
