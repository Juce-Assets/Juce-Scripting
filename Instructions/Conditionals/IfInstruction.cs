using Juce.Scripting.Execution;

namespace Juce.Scripting.Instructions
{
    public class IfInstruction : FlowInstruction
    {
        public const string ConditionIn = nameof(ConditionIn);

        public int TrueOutputFlowInstructionIndex { get; set; } = -1;
        public int FalseOutputFlowInstructionIndex { get; set; } = -1;

        public void ConnectTrueFlow(FlowInstruction flowScriptInstruction)
        {
            TrueOutputFlowInstructionIndex = flowScriptInstruction.ScriptInstructionIndex;
        }

        public void ConnectFalseFlow(FlowInstruction flowScriptInstruction)
        {
            FalseOutputFlowInstructionIndex = flowScriptInstruction.ScriptInstructionIndex;
        }

        public override void RegisterPorts()
        {
            AddInputPort<bool>(ConditionIn);
        }

        protected override void Execute(Script script)
        {
            bool conditionResult = GetInputPortValue<bool>(ConditionIn);

            if(conditionResult)
            {
                script.TryGetScriptInstruction(TrueOutputFlowInstructionIndex, out ScriptInstruction scriptInstruction);

                FlowInstruction flowScriptInstruction = scriptInstruction as FlowInstruction;

                new ScriptExecutor(script).ExecuteFlow(flowScriptInstruction);
            }
            else
            {
                script.TryGetScriptInstruction(FalseOutputFlowInstructionIndex, out ScriptInstruction scriptInstruction);

                FlowInstruction flowScriptInstruction = scriptInstruction as FlowInstruction;

                new ScriptExecutor(script).ExecuteFlow(flowScriptInstruction);
            }
        }
    }
}
