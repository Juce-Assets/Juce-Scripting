using System;
using System.Collections.Generic;

namespace Juce.Scripting
{
    public abstract class ScriptInstruction
    {
        private readonly List<Port> inputPorts = new List<Port>();
        private readonly List<Port> outputPorts = new List<Port>();

        private bool executed;

        public int ScriptInstructionIndex { get; set; } = -1;

        public IReadOnlyList<Port> InputPorts => inputPorts;
        public IReadOnlyList<Port> OutputPorts => outputPorts;


        public bool TryGetInputPort(int index, out Port port)
        {
            if (inputPorts.Count - 1 < index)
            {
                port = null;
                return false;
            }

            port = inputPorts[index];
            return true;
        }

        public bool TryGetOutputPort(int index, out Port port)
        {
            if (outputPorts.Count - 1 < index)
            {
                port = null;
                return false;
            }

            port = outputPorts[index];
            return true;
        }

        private bool TryGetInputPort(string id, out Port port)
        {
            foreach (Port inputPort in inputPorts)
            {
                if (string.Equals(id, inputPort.PortId))
                {
                    port = inputPort;
                    return true;
                }
            }

            port = null;
            return false;
        }

        private bool TryGetOutputPort(string id, out Port port)
        {
            foreach (Port outputPort in outputPorts)
            {
                if (string.Equals(id, outputPort.PortId))
                {
                    port = outputPort;
                    return true;
                }
            }

            port = null;
            return false;
        }

        public void AddInputPort<T>(string id)
        {
            AddInputPort(typeof(T), id);
        }

        public void AddOutputPort<T>(string id)
        {
            AddOutputPort(typeof(T), id);
        }

        public void AddInputPort(Type type, string id)
        {
            Port port = new Port(ScriptInstructionIndex, inputPorts.Count, id, type);

            inputPorts.Add(port);
        }

        public void AddOutputPort(Type type, string id)
        {
            Port port = new Port(ScriptInstructionIndex, outputPorts.Count, id, type);

            outputPorts.Add(port);
        }

        protected void SetInputPortValue(string id, object value)
        {
            foreach (Port port in InputPorts)
            {
                if (string.Equals(id, port.PortId))
                {
                    port.Value = value;

                    break;
                }
            }
        }

        public void SetOutputPortValue(string id, object value)
        {
            foreach(Port port in outputPorts)
            {
                if(string.Equals(id, port.PortId))
                {
                    port.Value = value;

                    break;
                }
            }
        }

        public void SetInputPortFallbackValue(string id, object value)
        {
            foreach (Port port in inputPorts)
            {
                if (string.Equals(id, port.PortId))
                {
                    port.FallbackValue = value;

                    break;
                }
            }
        }

        public T GetInputPortValue<T>(string id)
        {
            foreach (Port port in inputPorts)
            {
                if (string.Equals(id, port.PortId))
                {
                    T ret = default;

                    try
                    {
                        ret = (T)Convert.ChangeType(port.Value, typeof(T));
                    }
                    catch
                    {
                        throw new System.Exception($"Tried to get input port value with id {id} of type {typeof(T).Name}, but " +
                            $"the type of the value is {port.Value.GetType().Name}, at instruction {GetType().Name} " +
                            $"with index {ScriptInstructionIndex}");
                    }

                    return ret;
                }
            }

            return default;
        }

        public void ConnectOutputToInputPort(string outputPortId, ScriptInstruction toConnectInstruction, string toConnectInstructionInputPort)
        {
            if(toConnectInstruction == null)
            {
                return;
            }

            bool outputPortFound = TryGetOutputPort(outputPortId, out Port outputPort);

            if (!outputPortFound)
            {
                throw new System.Exception($"Tried to connect output port to input port [Instruction {GetType().Name} with index" +
                    $"{ScriptInstructionIndex}], but output port with id {outputPortId} could not be found");
            }

            bool inputPortFound = toConnectInstruction.TryGetInputPort(toConnectInstructionInputPort, out Port inputPort);

            if(!inputPortFound)
            {
                throw new System.Exception($"Tried to connect output port to input port [Instruction {GetType().Name} with index " +
                    $"{ScriptInstructionIndex}], but input port with id {toConnectInstructionInputPort} could not be found");
            }

            outputPort.Connect(inputPort);
        }

        public void ConnectInputToOutputPort(string inputPortId, ScriptInstruction toConnectInstruction, string toConnectInstructionOutputPort)
        {
            if (toConnectInstruction == null)
            {
                return;
            }

            bool inputPortFound = TryGetInputPort(inputPortId, out Port outputPort);

            if (!inputPortFound)
            {
                throw new System.Exception($"Tried to connect input port to output port [Instruction {GetType().Name} with index " +
                    $"{ScriptInstructionIndex}], but input port with id {inputPortId} could not be found");
            }

            bool outputPortFound = toConnectInstruction.TryGetOutputPort(toConnectInstructionOutputPort, out Port inputPort);

            if(!outputPortFound)
            {
                throw new System.Exception($"Tried to connect input port to output port [Instruction {GetType().Name} with index " +
                    $"{ScriptInstructionIndex}], but output port with id {toConnectInstructionOutputPort} could not be found");
            }

            inputPort.Connect(outputPort);
        }

        public void TryExecute(Script script)
        {
            if(executed)
            {
                return;
            }

            executed = true;

            Execute(script);
        }

        public void ResetInstruction()
        {
            foreach (Port inputPort in inputPorts)
            {
                inputPort.Value = default;
            }

            foreach (Port outputPort in outputPorts)
            {
                outputPort.Value = default;
            }

            executed = false;
        }

        public abstract void RegisterPorts();
        protected abstract void Execute(Script script);
    }
}
