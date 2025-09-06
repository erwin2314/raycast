using System;
using Microsoft.Xna.Framework;
public class Mapa
{
    public int[,] mapa_terreno;

    public Mapa
    (
        int[,] mapa_terreno = null
    )
    {
        if (mapa_terreno == null) //por alguna razon que no entiendo del todo al momento de dibujarlo esta invertido
        { 
            this.mapa_terreno = new int[,] {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,1,0,0,0,1,1,0,0,0,1,1,0,0,0,1},
                {1,1,1,1,0,1,1,1,1,1,1,1,0,1,1,1,1,0,0,1},
                {1,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,1,1,0,1},
                {1,0,1,1,0,1,0,1,1,0,1,1,1,1,1,0,1,0,0,1},
                {1,0,1,0,0,1,0,1,0,0,0,0,0,0,1,0,0,0,1,1},
                {1,0,0,0,1,1,0,1,0,1,1,1,0,1,1,1,1,0,0,1},
                {1,0,1,1,1,1,0,1,0,1,0,0,0,0,0,1,1,1,0,1},
                {1,0,1,0,0,0,0,1,0,1,0,1,1,1,0,1,0,0,0,1},
                {1,0,1,0,1,1,1,1,0,1,0,0,0,1,0,1,0,1,0,1},
                {1,0,0,0,0,0,0,1,1,1,1,0,1,1,0,0,0,0,0,1},
                {1,1,0,1,1,1,0,1,0,0,0,0,0,1,1,1,1,1,1,1},
                {1,0,0,1,1,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            };
        }
    }

    public float RayCast(Vector2 posicion, float angulo, float distanciaMaxima =10f) //la posicion es del jugador
    {
        // Calcular la dirección del rayo
    Vector2 direccion = new Vector2((float)Math.Cos(angulo), (float)Math.Sin(angulo));
    
    // Encontrar en qué celda de la grilla estamos
    int mapX = (int)posicion.X;  // Coordenada X de la celda actual
    int mapY = (int)posicion.Y;  // Coordenada Y de la celda actual
    
    // Calcular deltaDistX y deltaDistY
    // Esto representa: "¿Qué distancia recorre el rayo para moverse 1 unidad en X/Y?"
    float deltaDistX = Math.Abs(1f / direccion.X);
    float deltaDistY = Math.Abs(1f / direccion.Y);
    
    // Determinar dirección y calcular distancia inicial a las próximas líneas de grilla
    float sideDistX, sideDistY;
    int stepX, stepY;
    
    if (direccion.X < 0) // Rayo va hacia la izquierda
    {
        stepX = -1;  // Nos moveremos hacia celdas con X menor
        // Distancia desde nuestra posición hasta el borde izquierdo de la celda actual
        sideDistX = (posicion.X - mapX) * deltaDistX;
    }
    else // Rayo va hacia la derecha
    {
        stepX = 1;   // Nos moveremos hacia celdas con X mayor
        // Distancia desde nuestra posición hasta el borde derecho de la celda actual
        sideDistX = (mapX + 1.0f - posicion.X) * deltaDistX;
    }
    
    if (direccion.Y < 0) // Rayo va hacia abajo
    {
        stepY = -1;
        sideDistY = (posicion.Y - mapY) * deltaDistY;
    }
    else // Rayo va hacia arriba
    {
        stepY = 1;
        sideDistY = (mapY + 1.0f - posicion.Y) * deltaDistY;
    }
    
    
    // El bucle principal - saltar entre líneas de grilla
    bool hit = false;
    int side = 0; // 0 = cruzamos una línea vertical, 1 = cruzamos una línea horizontal
    int pasos = 0;
    
    while (!hit && pasos < 50) // Limitamos pasos para el ejemplo
    {
        pasos++;
        
        // DECISIÓN CLAVE: ¿Qué línea de grilla está más cerca?
        if (sideDistX < sideDistY)
        {
            // La próxima línea vertical está más cerca
            sideDistX += deltaDistX;  // Preparar para la siguiente línea vertical
            mapX += stepX;            // Moverse a la siguiente celda en X
            side = 0;                 // Recordar que cruzamos una línea vertical
        }
        else
        {
            // La próxima línea horizontal está más cerca
            sideDistY += deltaDistY;  // Preparar para la siguiente línea horizontal
            mapY += stepY;            // Moverse a la siguiente celda en Y
            side = 1;                 // Recordar que cruzamos una línea horizontal
        }
        
        // Verificar si esta celda es una pared
        if (EsPared(mapX, mapY))
        {
            hit = true;
        }
        
        // Verificar distancia máxima
        float distanciaActual;
        if (side == 0)
        {
            // Cruzamos una línea vertical (pared en lado X)
            distanciaActual = Math.Abs((mapX - posicion.X + (1 - stepX) / 2) / direccion.X);
        }
        else
        {
            // Cruzamos una línea horizontal (pared en lado Y)
            distanciaActual = Math.Abs((mapY - posicion.Y + (1 - stepY) / 2) / direccion.Y);
        }
            
        if (distanciaActual > distanciaMaxima)
        {
            return distanciaMaxima;
        }
    }
    
    // Calcular la distancia exacta
    float distanciaFinal;
    if (side == 0) // Golpeamos una pared vertical
    {
        distanciaFinal = Math.Abs((mapX - posicion.X + (1 - stepX) / 2) / direccion.X);
    }
    else // Golpeamos una pared horizontal
    {
        distanciaFinal = Math.Abs((mapY - posicion.Y + (1 - stepY) / 2) / direccion.Y);
    }
    
    
    return distanciaFinal;
    }

    //la posicion es del jugador
    //hitDistance es la distancia que se utiliza para comprobar si el raycast da al objetivo
    public bool RayCast(Vector2 posicion, Vector2 objetivo, float tamañoPaso = 0.005f, float distanciaMaxima = 10f, float hitDistance = 0.05f)
    {
        Vector2 anguloVector = objetivo - posicion;
        Vector2 posicionRayo = posicion;

        //verdadero si toca al objetivo

        while (true)
        {
            posicionRayo = posicionRayo + (anguloVector * tamañoPaso);

            if (EsPared(posicionRayo.X, posicionRayo.Y) || Vector2.Distance(posicion, posicionRayo) > distanciaMaxima)
            {
                return false;
            }
            else if (Vector2.Distance(posicionRayo, objetivo) < hitDistance)
            {
                return true;
            }
        }

    }

    public float[] RayCastFov(Jugador jugador, int resolucion = 640, float distanciaMaxima = 10f)
    {
        float anguloMinimo = jugador.angulo - (jugador.campoDeVision / 2);
        float anguloActual = anguloMinimo;
        float pasoAngulo = jugador.campoDeVision / resolucion;
        float[] distancias = new float[resolucion];
        float distanciaReal = 0;

        for (int i = 0; i < resolucion; i++)
        {
            distanciaReal = RayCast(jugador.posicion, anguloActual);
            distancias[i] = distanciaReal;

            anguloActual += pasoAngulo;
        }

        return distancias;

    }

    public bool EsPared(float x, float y)
    {
        try
        {
            if (mapa_terreno[(int)y, (int)x] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

    }

}