using System;
using System.Collections.Generic;
namespace KJakub.Octave.Data
{
    [Serializable]
    public class BindingOverrideData
    {
        public string action;
        public int bindingIndex;
        public string overridePath;
    }

    [Serializable]
    public class BindingOverrideWrapper
    {
        public List<BindingOverrideData> list;
    }
    [Serializable]
    public class InputBindingSet
    {
        private string keyboardJson;
        private string gamepadJson;
        public string KeyboardJSON { get { return keyboardJson; } set { keyboardJson = value; } }
        public string GamepadJSON { get { return gamepadJson; } set { gamepadJson = value; } }
        public InputBindingSet() { }
        public InputBindingSet(string keyboardJson, string gamepadJson)
        {
            (this.keyboardJson, this.gamepadJson) = (keyboardJson, gamepadJson);
        }
    }
}