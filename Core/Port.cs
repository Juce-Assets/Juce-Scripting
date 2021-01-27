using System;
using System.Collections.Generic;

namespace Juce.Scripting
{
    public class Port
    {
        public int ScriptInstructionIndex { get; set; } = -1;
        public int PortIndex { get; set; } = -1;
        public string PortId { get; set; }
        public Type PortConnectionType { get; set; }

        public List<PortConnection> PortConnections { get; set; } = new List<PortConnection>();

        public object FallbackValue { get; set; }
        public object Value { get; set; }

        public Port(int scriptInstructionIndex, int portIndex, string portId, Type portConnectionType)
        {
            ScriptInstructionIndex = scriptInstructionIndex;
            PortIndex = portIndex;
            PortId = portId;
            PortConnectionType = portConnectionType;
        }

        public void Connect(Port port)
        {
            if(ScriptInstructionIndex == port.ScriptInstructionIndex)
            {
                return;
            }

            if(this == port)
            {
                return;
            }

            foreach (PortConnection portConnection in PortConnections)
            {
                if(portConnection.ConnectedPortScriptInstructionIndex == port.ScriptInstructionIndex)
                {
                    return;
                }
            }

            PortConnections.Add(new PortConnection(port.PortIndex, port.ScriptInstructionIndex));
            port.PortConnections.Add(new PortConnection(PortIndex, ScriptInstructionIndex));
        }
    }
}
