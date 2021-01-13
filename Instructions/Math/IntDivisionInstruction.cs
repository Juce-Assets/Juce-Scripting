namespace Juce.Scripting.Instructions
{
    public class IntDivisionInstruction : ScriptInstruction
    {
        public const string ValueAIn = nameof(ValueAIn);
        public const string ValueBIn = nameof(ValueBIn);
        public const string ResultOut = nameof(ResultOut);

        public override void RegisterPorts()
        {
            AddInputPort<int>(ValueAIn);
            AddInputPort<int>(ValueBIn);
            AddOutputPort<int>(ResultOut);
        }

        protected override void Execute()
        {
            int valueA = GetInputPortValue<int>(ValueAIn);
            int valueB = GetInputPortValue<int>(ValueBIn);

            int finalValue = 0;

            if(valueB != 0)
            {
                finalValue = valueA / valueB;
            }

            SetOutputPortValue(ResultOut, finalValue);
        }
    }
}
