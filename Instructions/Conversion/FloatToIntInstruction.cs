namespace Juce.Scripting.Instructions
{
    public class FloatToIntInstruction : ScriptInstruction
    {
        public const string ValueIn = nameof(ValueIn);
        public const string ValueOut = nameof(ValueOut);

        public override void RegisterPorts()
        {
            AddInputPort<float>(ValueIn);
            AddOutputPort<int>(ValueOut);
        }

        protected override void Execute()
        {
            float value = GetInputPortValue<float>(ValueIn);

            SetOutputPortValue(ValueOut, (int)value);
        }
    }
}
