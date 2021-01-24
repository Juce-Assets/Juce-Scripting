﻿using System;
using System.Collections.Generic;

namespace Juce.Scripting
{
    public class Script
    {
        private readonly List<ScriptInstruction> scriptInstructions = new List<ScriptInstruction>();

        public IReadOnlyList<ScriptInstruction> ScriptInstructions => scriptInstructions;

        public bool TryGetScriptInstruction<T>(out T scriptInstruction) where T : ScriptInstruction
        {
            Type lookingType = typeof(T);

            foreach(ScriptInstruction instruction in scriptInstructions)
            {
                if(lookingType == instruction.GetType())
                {
                    scriptInstruction = instruction as T;
                    return true;
                }
            }

            scriptInstruction = null;
            return true;
        }

        public bool TryGetScriptInstruction(int index, out ScriptInstruction scriptInstruction)
        {
            if(scriptInstructions.Count - 1 < index || index < 0)
            {
                scriptInstruction = null;
                return false;
            }

            scriptInstruction = scriptInstructions[index];
            return true;
        }

        public T CreateScriptInstruction<T>() where T : ScriptInstruction
        {
            T instruction = Activator.CreateInstance<T>();

            instruction.Init(this, scriptInstructions.Count);

            scriptInstructions.Add(instruction);

            instruction.RegisterPorts();

            return instruction;
        }
    }
}
