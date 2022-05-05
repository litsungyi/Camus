using Camus.Inputs;
using UnityEngine;

public partial class App
{
    private InputManager inputManager;
    public InputManager InputManager
    {
        get
        {
            if (inputManager == null)
            {
                inputManager = gameObject.AddComponent<InputManager>();
            }

            return inputManager;
        }
    }
}
