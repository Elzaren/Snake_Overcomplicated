//Clase responsable de pintar los objetos en la pantalla
class Renderer{
    //Configura parametros
    public Renderer(){
        Console.CursorVisible = false;
    }

    //Dibuja el objeto en la pantalla
    public void draw(GameObject gameObject){
        Console.SetCursorPosition(gameObject.transform.x, gameObject.transform.y);
        Console.ForegroundColor = gameObject.material.color;
        Console.Write(gameObject.material.symbol);
    }

    //Dibuja un tile en la pantalla de un color especifico
    public void drawTile(int x, int y, ConsoleColor color){
        // Draw a tile on the screen
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write("X");
    }

    //Limpia un tile en la pantalla
    public void clearTile(int x, int y){
        // Clear a tile on the screen
        Console.SetCursorPosition(x, y);
        Console.Write(" ");
    }
    
    public void clear(){
        Console.Clear();
    }
}