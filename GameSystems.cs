class GameSystems{

    public int frameTime = 0; //Tiempo entre frames en milisegundos

    //Constructor de la escena principal
    public virtual Scene mainScene(){
        Scene scene = new Scene(25, 25, 100);
        return scene;
    }

    //Ejecuta las funciones al inicio del programa
    public virtual void onInitialize(Scene scene){
        
    }

    //Ejecuta las funciones en cada frame
    public virtual void onUpdate(Scene scene, InputSystem inputSystem, Renderer renderer, double deltaTime){
        
    }
}