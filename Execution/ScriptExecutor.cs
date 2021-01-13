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

            FlowScriptInstruction startFlow = script.ScriptInstructions[0] as FlowScriptInstruction;

            ExecuteFlow(startFlow);
        }

        public void ResetScript()
        {
            foreach(ScriptInstruction scriptInstruction in script.ScriptInstructions)
            {
                scriptInstruction.ResetInstruction();
            }
        }

        public void ExecuteFlow(FlowScriptInstruction flow)
        {
            FlowScriptInstruction currentFlow = flow;

            while (currentFlow != null)
            {
                bool found = script.TryGetScriptInstruction(currentFlow.OutputFlowScriptInstructionIndex, 
                    out ScriptInstruction scriptInstruction);

                if (!found)
                {
                    return;
                }

                ExecuteScriptInstruction(scriptInstruction);

                currentFlow = scriptInstruction as FlowScriptInstruction;
            }
        }

        private void ExecuteScriptInstruction(ScriptInstruction scriptInstruction)
        {
            foreach(Port inputPort in scriptInstruction.InputPorts)
            {
                GatherInputPortValue(inputPort);
            }

            scriptInstruction.TryExecute();
        }

        private void GatherInputPortValue(Port inputPort)
        {
            bool instructionFound = script.TryGetScriptInstruction(inputPort.ConnectedPortScriptInstructionIndex, 
                out ScriptInstruction scriptInstruction);

            if(!instructionFound)
            {
                return;
            }

            bool outputPortFound = scriptInstruction.TryGetOutputPort(inputPort.ConnectedPortIndex, out Port outputPort);

            if(!outputPortFound)
            {
                inputPort.Value = inputPort.FallbackValue;

                return;
            }

            ExecuteScriptInstruction(scriptInstruction);

            inputPort.Value = outputPort.Value;
        }
    }
}
