using Juce.Scripting.Execution;

namespace Juce.Scripting.Instructions
{
    public class IfInstruction : FlowScriptInstruction
    {
        public const string ConditionIn = nameof(ConditionIn);

        public int TrueOutputFlowScriptInstructionIndex { get; set; } = -1;
        public int FalseOutputFlowScriptInstructionIndex { get; set; } = -1;

        public void ConnectTrueFlow(FlowScriptInstruction flowScriptInstruction)
        {
            TrueOutputFlowScriptInstructionIndex = flowScriptInstruction.ScriptInstructionIndex;
        }

        public void ConnectFalseFlow(FlowScriptInstruction flowScriptInstruction)
        {
            FalseOutputFlowScriptInstructionIndex = flowScriptInstruction.ScriptInstructionIndex;
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
                script.TryGetScriptInstruction(TrueOutputFlowScriptInstructionIndex, out ScriptInstruction scriptInstruction);

                FlowScriptInstruction flowScriptInstruction = scriptInstruction as FlowScriptInstruction;

                new ScriptExecutor(script).ExecuteFlow(flowScriptInstruction);
            }
            else
            {
                script.TryGetScriptInstruction(FalseOutputFlowScriptInstructionIndex, out ScriptInstruction scriptInstruction);

                FlowScriptInstruction flowScriptInstruction = scriptInstruction as FlowScriptInstruction;

                new ScriptExecutor(script).ExecuteFlow(flowScriptInstruction);
            }
        }
    }
}
