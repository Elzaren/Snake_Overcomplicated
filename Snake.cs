//---------------- Engine Systems ------------------//
class UserSystems : GameSystems{

    public UserSystems() {
        frameTime = 350;
    }

    public override Scene mainScene(){
        return(new Scene(10, 10, 50));
    }


    Random rnd = new Random();
    public bool gameOver = false;
    
    void paintBorders(Scene scene, Renderer renderer){
        for (int i = 0; i < scene.width; i++){
            renderer.drawTile(i, 0, ConsoleColor.White);
            renderer.drawTile(i, scene.height, ConsoleColor.White);
        }
        for (int i = 0; i < scene.height; i++){
            renderer.drawTile(0, i, ConsoleColor.White);
            renderer.drawTile(scene.width, i, ConsoleColor.White);
        }
    }

    public void spawnNewApple(Scene scene){
        int x, y;
        //Buscar una posición vacia para la manzana
        do{
            x = rnd.Next(1, scene.width-1);
            y = rnd.Next(1, scene.height-1);
        }while (scene.gameObjects.Any(obj => obj != null && obj.transform.x == x && obj.transform.y == y));
        //Spawn la manzana en la posición vacia
        scene.Spawn(new Apple(x, y));
    }

    public override void onInitialize(Scene scene){
        scene.Spawn(new Snake(scene.width/2, scene.height/2, scene.width*scene.height));
        spawnNewApple(scene); //Generar la primera manzana
    }

    public override void onUpdate(Scene scene, InputSystem inputSystem, Renderer renderer, double deltaTime){
        paintBorders(scene, new Renderer());

        if(gameOver){
            Console.SetCursorPosition(scene.width+1, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game Over!");
        }    
    }

}

//---------------- Game Objects ------------------//
class Snake : GameObject{   
    public Snake(int x, int y, int sceneSize){
        this.material = new Material{color = ConsoleColor.Green, symbol = 'S'};
        this.transform = new Transform{x = x, y = y};
        body = new GameObject[sceneSize];
    }

    //Almacena la dirección hacia la que se mueve la serpiente
    int direction = 0; // 0 = up, 1 = down, 2 = left, 3 = right
    int length = 0; //Longitud de la serpiente
    GameObject[] body;
    
    public override void update(Scene scene, InputSystem inputSystem, double deltaTime, UserSystems userSystems, Renderer renderer){
        //Movimiento del cuerpo de la serpiente
        if (length == 0){
            // Si la serpiente no tiene cuerpo, solo se borra la cabeza
            renderer.clearTile(transform.x, transform.y);
        }else{
            // Si la serpiente tiene cuerpo, se borra la última parte del cuerpo
            renderer.clearTile(body[length-1].transform.x, body[length-1].transform.y);

            // Actualiza las posiciones de las partes del cuerpo para que sigan a la parte anterior
            for (int i = length - 1; i > 0; i--) {
                body[i].transform.x = body[i - 1].transform.x;
                body[i].transform.y = body[i - 1].transform.y;
            }

            // Actualiza la primera parte del cuerpo para que siga a la cabeza
            body[0].transform.x = transform.x;
            body[0].transform.y = transform.y;
        }
        
        // Actualiza la dirección de la serpiente según la tecla presionada
        switch (inputSystem.getKeyPress()){
            case ConsoleKey.UpArrow:
                direction = direction == 1 ? 1 : 0;
                break;
            case ConsoleKey.DownArrow:
                direction = direction == 0 ? 0 : 1;
                break;
            case ConsoleKey.LeftArrow:
                direction = direction == 3 ? 3 : 2;
                break;
            case ConsoleKey.RightArrow:
                direction = direction == 2 ? 2 : 3;
                break;
        }

        //Mueve la serpiente según la dirección actual
        transform.y += direction == 0 ? -1 : direction == 1 ? 1 : 0;
        transform.x += direction == 2 ? -1 : direction == 3 ? 1 : 0;

        
        //Comprobar si hay colisión con el cuerpo de la serpiente
        if(length > 0){
            for (int i = 0; i < length; i++){
                if (transform.x == body[i].transform.x && transform.y == body[i].transform.y){
                    scene.despawn(this); //Borrar la cabeza de la serpiente de la escena
                    userSystems.gameOver = true; // Cambia el estado del juego a "Game Over"    
                }
            }
        }

        // Verifica si la serpiente ha chocado con los bordes de la escena
        if (transform.x < 1 || transform.x >= scene.width || transform.y < 1 || transform.y >= scene.height){
            scene.despawn(this);
            userSystems.gameOver = true; // Cambia el estado del juego a "Game Over"
        }

        //Comprobar si hay colisión con una manzana
        foreach (var obj in scene.gameObjects){
            if (obj is Apple apple && transform.x == apple.transform.x && transform.y == apple.transform.y){
                scene.despawn(apple); //Despawnear la manzana

                userSystems.spawnNewApple(scene); //Generar una nueva manzana

                body[length] = scene.Spawn(new SnakeBody(transform.x,transform.y)); //Crecer la serpiente
                length++; //Aumentar la longitud de la serpiente
            }
        }
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(scene.width+1, 1);
        Console.Write($"Score: {length}");

    }
}

class SnakeBody : GameObject{
    public SnakeBody(int x, int y){
        this.material = new Material{color = ConsoleColor.Green, symbol = 'H'};
        this.transform = new Transform{x = x, y = y};
    }
}

class Apple : GameObject{
    public Apple(int x, int y){
        this.material = new Material{color = ConsoleColor.Red, symbol = 'A'};
        this.transform = new Transform{x = x, y = y};
    }
}