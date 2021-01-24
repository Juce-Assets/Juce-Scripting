using Juce.Scripting.Execution;

namespace Juce.Scripting.Instructions.Subscript
{
    public class SubscriptInstruction : FlowScriptInstruction
    {
        public Script Subscript { get; set; }
        public int SubscriptInInstructionIndex { get; set; }
        public int SubscriptOutInstructionIndex { get; set; }

        public override void RegisterPorts()
        {

        }

        protected override void Execute()
        {
            if(Subscript == null)
            {
                return;
            }

            SubscriptInInstruction subscriptInInstruction = null;
            SubscriptOutInstruction subscriptOutInstruction = null;

            bool inFound = Subscript.TryGetScriptInstruction(
                SubscriptInInstructionIndex, 
                out ScriptInstruction scriptInInstruction
                );

            bool outFound = Subscript.TryGetScriptInstruction(
                SubscriptOutInstructionIndex,
                out ScriptInstruction scriptOutInstruction
                );

            if (inFound && outFound)
            {
                subscriptInInstruction = scriptInInstruction as SubscriptInInstruction;
                subscriptOutInstruction = scriptOutInstruction as SubscriptOutInstruction;
            }

            if(subscriptInInstruction == null || subscriptOutInstruction == null)
            {
                return;
            }

            foreach(Port port in InputPorts)
            {
                subscriptInInstruction.SetOutputPortValue(port.PortId, port.Value);
            }

            new ScriptExecutor(Subscript).ExecuteFlow(subscriptInInstruction);

            foreach(Port port in subscriptOutInstruction.InputPorts)
            {
                SetOutputPortValue(port.PortId, port.Value);
            }
        }
    }
}
