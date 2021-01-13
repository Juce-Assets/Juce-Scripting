using System;

namespace Juce.Scripting
{
    public class Test2FlowInstruction : FlowScriptInstruction
    {
        public const string IntValueIn = nameof(IntValueIn);
        public const string IntValueOut = nameof(IntValueOut);

        public override void RegisterPorts()
        {
            AddInputPort<int>(IntValueIn);
            AddOutputPort<int>(IntValueOut);
        }

        protected override void Execute()
        {
            int currentValue = GetInputPortValue<int>(IntValueIn);

            SetOutputPortValue(IntValueOut, currentValue + 10);
        }
    }
}
