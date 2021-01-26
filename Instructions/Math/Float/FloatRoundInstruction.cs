using System;

namespace Juce.Scripting.Instructions
{
    public class FloatRoundInstruction : ScriptInstruction
    {
        public const string ValueIn = nameof(ValueIn);
        public const string DecimalsIn = nameof(DecimalsIn);
        public const string ResultOut = nameof(ResultOut);

        public override void RegisterPorts()
        {
            AddInputPort<float>(ValueIn);
            AddInputPort<int>(DecimalsIn);
            AddOutputPort<float>(ResultOut);
        }

        protected override void Execute(Script script)
        {
            float value = GetInputPortValue<float>(ValueIn);
            int decimals = GetInputPortValue<int>(DecimalsIn);

            SetOutputPortValue(ResultOut, Math.Round(value, decimals));
        }
    }
}
