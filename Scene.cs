using System.Collections.Concurrent;
class Scene{

    //Tamaño de la escena
    public int width;
    public int height;
    
    int objectCapacity;//Capacidad de la escena
    int objectCount; //Contador de objetos en la escena


    public GameObject?[] gameObjects; //Lista de objetos en la escena

    //Lista de objetos a eliminar de la escena
    private ConcurrentQueue<GameObject> despawnQueue = new ConcurrentQueue<GameObject>();

    //Constructor de la escena
    public Scene(int width= 25, int height=25, int objectCapacity=100){
        this.width = width;
        this.height = height;
        this.objectCapacity = objectCapacity;
        gameObjects = new GameObject[objectCapacity];
        for (int i = 0; i < gameObjects.Length; i++){
            gameObjects[i] = null;
        }
    }

    public int size(){ //Devuelve la capacidad de la escena
        return objectCapacity;
    }

    //Busca un hueco en la escena y crea el objeto si es posible
    public GameObject Spawn(GameObject gameObject){
        if (objectCount != objectCapacity){
            for (int i = 0; i < gameObjects.Length; i++){
                if (gameObjects[i] == null){
                    gameObjects[i] = gameObject;
                    gameObject.index = i;
                    objectCount++;
                    return gameObject;
                }
            }
        } else {
            Console.WriteLine("Scene is full, cannot add more game objects.");
        }
        return null;
    }

    //Programa objetos para ser eliminados
    public void despawn(GameObject gameObject){
        gameObject.isActive = false;
        despawnQueue.Enqueue(gameObject);
        return;
    }

    //Elimina objetos programados para ser eliminados
    public void DespawnObjects(){
        while (despawnQueue.TryDequeue(out GameObject gameObject)){
            gameObjects[gameObject.index] = null;
            objectCount--;
        }
        return;
    }

}