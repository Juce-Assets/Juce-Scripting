using System;

namespace Juce.Scripting
{
    public class PortConnection
    {
        public int ConnectedPortIndex { get; set; } = -1;
        public int ConnectedPortScriptInstructionIndex { get; set; } = -1;

        public PortConnection(int connectedPortIndex, int connectedPortScriptInstructionIndex)
        {
            ConnectedPortIndex = connectedPortIndex;
            ConnectedPortScriptInstructionIndex = connectedPortScriptInstructionIndex;
        }
    }
}
