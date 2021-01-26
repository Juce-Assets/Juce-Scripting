namespace Juce.Scripting.Instructions
{
    public class FloatSubstractionInstruction : ScriptInstruction
    {
        public const string ValueAIn = nameof(ValueAIn);
        public const string ValueBIn = nameof(ValueBIn);
        public const string ResultOut = nameof(ResultOut);

        public override void RegisterPorts()
        {
            AddInputPort<float>(ValueAIn);
            AddInputPort<float>(ValueBIn);
            AddOutputPort<float>(ResultOut);
        }

        protected override void Execute(Script script)
        {
            float valueA = GetInputPortValue<float>(ValueAIn);
            float valueB = GetInputPortValue<float>(ValueBIn);

            SetOutputPortValue(ResultOut, valueA - valueB);
        }
    }
}
