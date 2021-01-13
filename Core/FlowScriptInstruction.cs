using System;

namespace Juce.Scripting
{
    public abstract class FlowScriptInstruction : ScriptInstruction
    {
        public int OutputFlowScriptInstructionIndex { get; set; } = -1;

        public void ConnectFlow(FlowScriptInstruction flowScriptInstruction)
        {
            OutputFlowScriptInstructionIndex = flowScriptInstruction.ScriptInstructionIndex;
        }
    }
}
