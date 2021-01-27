using Juce.Scripting.Execution;

namespace Juce.Scripting.Instructions.SubScript
{
    public class SubScriptInstruction : FlowInstruction
    {
        public int SubScriptIndex { get; set; } = -1;
        public int SubscriptInInstructionIndex { get; set; } = -1;
        public int SubscriptOutInstructionIndex { get; set; } = -1;

        public override void RegisterPorts()
        {

        }

        protected override void Execute(Script script)
        {
            bool found = script.TryGetSubScript(SubScriptIndex, out Script subScript);

            if(!found)
            {
                return;
            }

            SubScriptInInstruction subscriptInInstruction = null;
            SubScriptOutInstruction subscriptOutInstruction = null;

            bool inFound = subScript.TryGetScriptInstruction(
                SubscriptInInstructionIndex, 
                out ScriptInstruction scriptInInstruction
                );

            bool outFound = subScript.TryGetScriptInstruction(
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

            new ScriptExecutor(subScript).ExecuteFlow(subscriptInInstruction);

            foreach(Port port in subscriptOutInstruction.InputPorts)
            {
                SetOutputPortValue(port.PortId, port.Value);
            }
        }

        protected override void OnResetInstruction(Script script)
        {
            bool found = script.TryGetSubScript(SubScriptIndex, out Script subScript);

            if (!found)
            {
                return;
            }

            SubScriptInInstruction subscriptInInstruction = null;

            bool inFound = subScript.TryGetScriptInstruction(
                SubscriptInInstructionIndex,
                out ScriptInstruction scriptInInstruction
                );

            if (inFound)
            {
                subscriptInInstruction = scriptInInstruction as SubScriptInInstruction;
            }

            if (subscriptInInstruction == null)
            {
                throw new System.Exception($"Subscript does not have input node at {nameof(SubScriptInstruction)} " +
                    $"with index {ScriptInstructionIndex}");
            }

            new ScriptExecutor(subScript).ResetAllInstructions();
        }
    }
}
