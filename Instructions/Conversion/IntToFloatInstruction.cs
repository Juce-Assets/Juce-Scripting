namespace Juce.Scripting.Instructions
{
    public class IntToFloatInstruction : ScriptInstruction
    {
        public const string ValueIn = nameof(ValueIn);
        public const string ValueOut = nameof(ValueOut);

        public override void RegisterPorts()
        {
            AddInputPort<int>(ValueIn);
            AddOutputPort<float>(ValueOut);
        }

        protected override void Execute(Script script)
        {
            int value = GetInputPortValue<int>(ValueIn);

            SetOutputPortValue(ValueOut, (float)value);
        }
    }
}
