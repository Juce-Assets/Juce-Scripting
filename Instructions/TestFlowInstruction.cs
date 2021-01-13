using System;

namespace Juce.Scripting
{
    public class TestFlowInstruction : FlowScriptInstruction
    {
        public const string IntValueOut = nameof(IntValueOut);

        public override void RegisterPorts()
        {
            AddOutputPort<int>(IntValueOut);
        }

        protected override void Execute()
        {
            SetOutputPortValue(IntValueOut, 32);
        }
    }
}
