class GameObject{
    public int index; //El indice del objeto en la escena para facilitar el acceso a su referencia
    public bool isActive = true; // Indica si el objeto está activo o no

    //Componentes del objeto
    public Transform transform;
    public Material material;

    //Funcion estandarizada que se ejecuta cada frame en el bucle principal
    public virtual void update(Scene scene, InputSystem inputSystem, double deltaTime, UserSystems userSystems, Renderer renderer){}
}