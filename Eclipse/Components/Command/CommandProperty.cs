namespace Eclipse.Components.Command
{
    [System.Serializable]
    public class CommandProperty
    {
        public enum VariableType
        {
            Static, Dynamic
        }

        [System.Serializable]
        public struct PropertyType
        {
            public VariableType variableType;
            public string variableName;
            public string variableValue;

            public PropertyType(VariableType variableType, string variableValue, string variableName)
            {
                this.variableType = variableType;
                this.variableName = variableName;
                this.variableValue = variableValue;
            }
        }

        public PropertyType property;

        public CommandProperty(PropertyType property)
        {
            this.property = property;
        }
    }
}
