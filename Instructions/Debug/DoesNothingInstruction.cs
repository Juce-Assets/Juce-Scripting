namespace Juce.Scripting.Instructions
{
    public class DoesNothingInstruction : FlowInstruction
    {
        public const string ValueIntIn = nameof(ValueIntIn);
        public const string ValueFloatIn = nameof(ValueFloatIn);
        public const string ValueStringIn = nameof(ValueStringIn);

        public const string ValueIntOut = nameof(ValueIntOut);
        public const string ValueFloatOut = nameof(ValueFloatOut);
        public const string ValueStringOut = nameof(ValueStringOut);

        public override void RegisterPorts()
        {
            AddInputPort<int>(ValueIntIn);
            AddInputPort<float>(ValueFloatIn);
            AddInputPort<string>(ValueStringIn);

            AddOutputPort<int>(ValueIntOut);
            AddOutputPort<float>(ValueFloatOut);
            AddOutputPort<string>(ValueStringOut);
        }

        protected override void Execute(Script script)
        {
            int intValue = GetInputPortValue<int>(ValueIntIn);
            float floatValue = GetInputPortValue<float>(ValueFloatIn);
            string stringValue = GetInputPortValue<string>(ValueStringIn);

            SetOutputPortValue(ValueIntOut, intValue);
            SetOutputPortValue(ValueFloatOut, floatValue);
            SetOutputPortValue(ValueStringOut, stringValue);
        }
    }
}
