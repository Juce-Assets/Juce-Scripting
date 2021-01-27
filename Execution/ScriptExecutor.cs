using System.Collections.Generic;

namespace Juce.Scripting.Execution
{
    public class ScriptExecutor : IScriptExecutor
    {
        private readonly Script script;

        public ScriptExecutor(Script script)
        {
            this.script = script;
        }

        public void Execute()
        {
            if (script.ScriptInstructions.Count == 0)
            {
                return;
            }

            bool found = script.TryGetScriptInstruction(out StartFlowInstruction startFlowInstruction);

            if(!found)
            {
                return;
            }

            ResetAllInstructions();

            ExecuteFlow(startFlowInstruction);
        }

        public void ExecuteFlow(FlowInstruction flow)
        {
            FlowInstruction currentFlow = flow;

            while (currentFlow != null)
            {
                ExecuteScriptInstruction(currentFlow);

                bool found = script.TryGetScriptInstruction(currentFlow.OutputFlowInstructionIndex, 
                    out ScriptInstruction scriptInstruction);

                if (!found)
                {
                    return;
                }

                currentFlow = scriptInstruction as FlowInstruction;
            }
        }

        private void ExecuteScriptInstruction(ScriptInstruction scriptInstruction)
        {
            if(!scriptInstruction.CanExecute)
            {
                return;
            }

            foreach(Port inputPort in scriptInstruction.InputPorts)
            {
                GatherInputPortValue(inputPort);
            }

            scriptInstruction.TryExecute(script);
        }

        private void GatherInputPortValue(Port inputPort)
        {
            if(inputPort.PortConnections.Count == 0)
            {
                inputPort.Value = inputPort.FallbackValue;

                return;
            }

            PortConnection currentPortConnection = inputPort.PortConnections[0];

            bool instructionFound = script.TryGetScriptInstruction(currentPortConnection.ConnectedPortScriptInstructionIndex, 
                out ScriptInstruction scriptInstruction);

            if(!instructionFound)
            {
                inputPort.Value = inputPort.FallbackValue;

                return;
            }

            bool outputPortFound = scriptInstruction.TryGetOutputPort(currentPortConnection.ConnectedPortIndex, out Port outputPort);

            if(!outputPortFound)
            {
                inputPort.Value = inputPort.FallbackValue;

                return;
            }

            ExecuteScriptInstruction(scriptInstruction);

            inputPort.Value = outputPort.Value;
        }

        public void ResetAllInstructions()
        {
            foreach(ScriptInstruction instruction in script.ScriptInstructions)
            {
                instruction.ResetInstruction(script);
            }
        }

        public void ResetFlow(FlowInstruction flow)
        {
            FlowInstruction currentFlow = flow;

            while (currentFlow != null)
            {
                if(!currentFlow.Executed)
                {
                    return;
                }

                currentFlow.ResetInstruction(script);

                bool found = script.TryGetScriptInstruction(currentFlow.OutputFlowInstructionIndex,
                    out ScriptInstruction scriptInstruction);

                if (!found)
                {
                    return;
                }

                currentFlow = scriptInstruction as FlowInstruction;
            }
        }
    }
}
