using Juce.Scripting.Execution;

namespace Juce.Scripting.Instructions.SubScript
{
    public class SubScriptInstruction : FlowScriptInstruction
    {
        public Script SubScript { get; set; }
        public int SubscriptInInstructionIndex { get; set; }
        public int SubscriptOutInstructionIndex { get; set; }

        public override void RegisterPorts()
        {

        }

        protected override void Execute()
        {
            if(SubScript == null)
            {
                return;
            }

            SubScriptInInstruction subscriptInInstruction = null;
            SubScriptOutInstruction subscriptOutInstruction = null;

            bool inFound = SubScript.TryGetScriptInstruction(
                SubscriptInInstructionIndex, 
                out ScriptInstruction scriptInInstruction
                );

            bool outFound = SubScript.TryGetScriptInstruction(
                SubscriptOutInstructionIndex,
                out ScriptInstruction scriptOutInstruction
                );

            if (inFound && outFound)
            {
                subscriptInInstruction = scriptInInstruction as SubScriptInInstruction;
                subscriptOutInstruction = scriptOutInstruction as SubScriptOutInstruction;
            }

            if(subscriptInInstruction == null)
            {
                throw new System.Exception($"Subscript does not have input node at {nameof(SubScriptInstruction)} " +
                    $"with index {ScriptInstructionIndex}");
            }

            if(subscriptOutInstruction == null)
            {
                throw new System.Exception($"Subscript does not have output node at {nameof(SubScriptInstruction)} " +
                    $"with index {ScriptInstructionIndex}");
            }

            foreach(Port port in InputPorts)
            {
                subscriptInInstruction.SetOutputPortValue(port.PortId, port.Value);
            }

            new ScriptExecutor(SubScript).ExecuteFlow(subscriptInInstruction);

            foreach(Port port in subscriptOutInstruction.InputPorts)
            {
                SetOutputPortValue(port.PortId, port.Value);
            }
        }
    }
}
