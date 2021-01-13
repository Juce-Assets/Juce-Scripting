namespace Juce.Scripting.Instructions
{
    public class IntToStringInstruction : FlowScriptInstruction
    {
        public const string ValueIn = nameof(ValueIn);
        public const string ValueOut = nameof(ValueOut);

        public override void RegisterPorts()
        {
            AddInputPort<int>(ValueIn);
            AddOutputPort<string>(ValueOut);
        }

        protected override void Execute()
        {
            int value = GetInputPortValue<int>(ValueIn);

            SetOutputPortValue(ValueOut, value.ToString());
        }
    }
}
