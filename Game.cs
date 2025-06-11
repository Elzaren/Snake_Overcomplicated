/*
//---------------- Engine Systems ------------------//
class UserSystems : GameSystems{

    // Constructor para definir variables del programa
    public UserSystems() {
        //frameTime = 100;
    }

    //Define la escena principal del juego
    public override Scene mainScene(){
        return(new Scene(10, 10, 50));
    }

    //Se ejecuta cuando se inicializa la aplicacion despues de crear la escena principal
    public override void onInitialize(Scene scene){

    }

    //Se ejecuta cada frame
    public override void onUpdate(Scene scene, InputSystem inputSystem, Renderer renderer, double deltaTime){

    }


}

//---------------- Game Objects ------------------//

class MyGameObject : GameObject{
    // Constructor del objeto del juego
    public MyGameObject(int x, int y){
        this.material = new Material{color = ConsoleColor.Red, symbol = 'A'};
        this.transform = new Transform{x = x, y = y};
    }

    // Se ejecuta cada frame
    void Update(Scene scene, InputSystem inputSystem, double deltaTime, UserSystems userSystems, Renderer renderer){
        
    }
}
*/