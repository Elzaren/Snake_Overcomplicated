class Program
{
    static void Main(string[] args)
    {

        //Variables del programa y sistemas

        bool isAppRunning = true; //Flag para parar el programa (sale del bucle principal)
        bool isGameRunning = true;//Flag para pausar el juego (salta el update de los objetos)

        UserSystems gameSystems;//Define la logica del juego
        Scene currentScene;//Define la escena actual
        InputSystem inputSystem;//Escucha el input del usuario
        Renderer renderer;//Dibuja los objetos en la consola

        double deltaTime;//Almacena el tiempo entre frames (en milisegundos)
        DateTime previousTime;//Almacena el tiempo al comenzar el frame para calcular deltaTime


        //Incialización
        inputSystem = new InputSystem();//Instancia el sistema de input
        renderer = new Renderer();//Instancia el sistema de renderizado
        gameSystems = new UserSystems();//instancia el sistema de juego
        currentScene = gameSystems.mainScene();//Instancia la escena principal en base a la logica del juego

        renderer.clear();//Limpia la consola y la prepara para el juego 
        gameSystems.onInitialize(currentScene);//Llama a la funcion de inicializacion del juego
        previousTime = DateTime.Now;//Guarda el tiempo actual para calcular el deltaTime

        //Main Loop
        while (isAppRunning)
        {

            //Calcular deltaTime
            DateTime currentTime = DateTime.Now;
            deltaTime = (currentTime - previousTime).TotalMilliseconds;
            previousTime = currentTime;
            //Duerme el hilo para mantener el framerate deseado y prevenir el uso excesivo de CPU
            Thread.Sleep(gameSystems.frameTime);

            //Actualiza la escena y objetos del juego si no esta pausado
            if (isGameRunning)
            {

                //Actualiza el juego en base su logica
                gameSystems.onUpdate(currentScene, inputSystem, renderer, deltaTime);

                //Recorre todos los objetos de la escena y los actualiza si estan activos
                for (int i = 0; i < currentScene.size(); i++)
                {

                    if (currentScene.gameObjects[i] != null && currentScene.gameObjects[i].isActive)
                    {
                        currentScene.gameObjects[i].update(currentScene, inputSystem, deltaTime, gameSystems, renderer);

                        //Comprueba si el objeto esta dentro de la escena y lo dibuja
                        if (currentScene.gameObjects[i].transform.x >= 0 && currentScene.gameObjects[i].transform.x < currentScene.width &&
                            currentScene.gameObjects[i].transform.y >= 0 && currentScene.gameObjects[i].transform.y < currentScene.height)
                        {
                            renderer.draw(currentScene.gameObjects[i]);
                        }
                    }
                }

                //Elimina los objetos que que se han programado para ser destruidos
                currentScene.DespawnObjects();
            }

            //Cierra la aplicación si el usuario presiona ESCAPE
            isAppRunning = !inputSystem.isKeyPressed(ConsoleKey.Escape);

            //Pausa el juego si el usuario presiona SPACEBAR
            if (inputSystem.isKeyPressed(ConsoleKey.Spacebar))
            {
                isGameRunning = !isGameRunning;
                Thread.Sleep(200); //Añade un pequeño delay para evitar spamming de la tecla
            }

            //Imprime información de depuración en la consola
            int debugX = currentScene.width + 1;
            int debugY = currentScene.height / 2 + 1;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(debugX, debugY);
            Console.Write($"DT: {deltaTime} ms      ");
            Console.SetCursorPosition(debugX, debugY + 1);
            Console.Write($"Induced DT: {gameSystems.frameTime} ms      ");
            Console.SetCursorPosition(debugX, debugY + 2);
            Console.Write($"Real DT: {deltaTime - gameSystems.frameTime} ms      ");
            Console.SetCursorPosition(debugX, debugY + 3);
            Console.Write($"FPS: {(int)(1000 / deltaTime)}      ");
            Console.SetCursorPosition(debugX, debugY + 4);
            Console.Write($"Paused: {!isGameRunning}      ");

        }

        //Cleanup, no es necesario realizar ninguna corrutina especial ya que el GC se encarga de liberar la memoria
        renderer.clear();
    }
}