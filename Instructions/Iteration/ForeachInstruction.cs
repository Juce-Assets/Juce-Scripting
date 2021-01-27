using Juce.Scripting.Execution;
using System.Collections.Generic;

namespace Juce.Scripting.Instructions
{
    public class ForeachInstruction<T> : FlowInstruction
    {
        public const string ValuesListIn = nameof(ValuesListIn);
        public const string IterationValueOut = nameof(IterationValueOut);
        public const string IterationIndexOut = nameof(IterationIndexOut);

        public int IterationOutputFlowInstructionIndex { get; set; } = -1;

        public void ConnectIterationFlow(FlowInstruction flowScriptInstruction)
        {
            IterationOutputFlowInstructionIndex = flowScriptInstruction.ScriptInstructionIndex;
        }

        public override void RegisterPorts()
        {
            AddInputPort<List<T>>(ValuesListIn);
            AddOutputPort<T>(IterationValueOut);
            AddOutputPort<int>(IterationIndexOut);
        }

        protected override void Execute(Script script)
        {
            SetOutputPortValue(IterationValueOut, (T)default);
            SetOutputPortValue(IterationIndexOut, 0);

            List<T> valuesList = GetInputPortValue<List<T>>(ValuesListIn);

            if(valuesList == null)
            {
                return;
            }

            if(valuesList.Count == 0)
            {
                return;
            }

            bool found = script.TryGetScriptInstruction(IterationOutputFlowInstructionIndex, out ScriptInstruction scriptInstruction);

            if(!found)
            {
                return;
            }

            FlowInstruction flowScriptInstruction = scriptInstruction as FlowInstruction;

            if(flowScriptInstruction == null)
            {
                return;
            }

            for(int i = 0; i < valuesList.Count; ++i)
            {
                SetOutputPortValue(IterationValueOut, valuesList[i]);
                SetOutputPortValue(IterationIndexOut, i);

                ScriptExecutor scriptExecutor = new ScriptExecutor(script);

                scriptExecutor.ResetFlow(flowScriptInstruction);
                scriptExecutor.ExecuteFlow(flowScriptInstruction);
            }
        }
    }
}
