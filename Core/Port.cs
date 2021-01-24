using System;

namespace Juce.Scripting
{
    public class Port
    {
        public int ScriptInstructionIndex { get; set; } = -1;
        public int PortIndex { get; set; } = -1;
        public string PortId { get; set; }
        public Type PortConnectionType { get; set; }

        public int ConnectedPortIndex { get; set; } = -1;
        public int ConnectedPortScriptInstructionIndex { get; set; } = -1;

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

            ConnectedPortScriptInstructionIndex = port.ScriptInstructionIndex;
            ConnectedPortIndex = port.PortIndex;

            port.ConnectedPortScriptInstructionIndex = ScriptInstructionIndex;
            port.ConnectedPortIndex = PortIndex;
        }
    }
}
