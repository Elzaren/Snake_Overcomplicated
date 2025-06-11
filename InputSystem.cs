using System.Collections.Generic;
class InputSystem { 
    private HashSet<ConsoleKey> pressedKeys = new HashSet<ConsoleKey>();

    public bool isKeyPressed(ConsoleKey key) {
        lock (pressedKeys) {
            return pressedKeys.Contains(key);
        }
    }

    public ConsoleKey? getKeyPress() {
        lock (pressedKeys) {
            if (pressedKeys.Count > 0) {
                var key = pressedKeys.First();
                pressedKeys.Remove(key);
                return key;
            }
        }
        return null;
    }

    public void ClearKeyBuffer() {
        lock (pressedKeys) {
            pressedKeys.Clear();
        }
    }

    // Método que se ejecuta en un hilo separado para leer las teclas presionadas
    private void Update() {
        while (true) {
            if (Console.KeyAvailable) {
                var key = Console.ReadKey(true).Key;
                lock (pressedKeys) {
                    pressedKeys.Add(key);
                }
            }
            Thread.Sleep(1); //Pequeño delay para no saturar la CPU
        }
    }

    // Constructor que inicia el proceso de lectura de teclas en un hilo separado
    public InputSystem() {
        Thread inputThread = new Thread(Update);
        inputThread.IsBackground = true;
        inputThread.Start();
    }
}