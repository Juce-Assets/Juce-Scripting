namespace Juce.Scripting
{
    public abstract class FlowInstruction : ScriptInstruction
    {
        public int OutputFlowInstructionIndex { get; set; } = -1;

        protected override bool CanReExecute => false; 

        public void ConnectFlow(FlowInstruction flowInstruction)
        {
            OutputFlowInstructionIndex = flowInstruction.ScriptInstructionIndex;
        }
    }
}
