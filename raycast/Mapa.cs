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
        if (mapa_terreno == null)
        {
            this.mapa_terreno = new int[,] {
                {1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,0,0,0,0,0,1},
                {1,0,0,1,1,1,1,0,0,1},
                {1,0,0,1,0,0,1,0,0,1},
                {1,0,0,1,0,0,1,0,0,1},
                {1,0,0,1,0,1,1,0,0,1},
                {1,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,1},
                {1,1,1,1,1,1,1,1,1,1}
            };
        }
    }

    public float RayCast(Vector2 posicion, float angulo, float distanciaMaxima = 20f) //la posicion es del jugador
    {
        Vector2 anguloVector = new Vector2(Convert.ToSingle(Math.Cos(angulo)), Convert.ToSingle(Math.Sin(angulo)));

        int mapX = (int)posicion.X; //posicion actual del rayo
        int mapY = (int)posicion.Y;

        float deltaDistX = Math.Abs(1f / anguloVector.X); //distancia que tiene que recorrer para cruzarse con una linea
        float deltaDistY = Math.Abs(1f / anguloVector.Y);

        int stepX = 0;// direccion del paso (-1 0 +1)
        int stepY = 0;

        float sideDistX = 0; //distancia desde la posicion actual a la siguiente linea
        float sideDistY = 0;

        if (anguloVector.X < 0)
        {
            stepX = -1; //se mueve para la izquierda
            sideDistX = (posicion.X - mapX) * deltaDistX;
        }
        else
        {
            stepX = 1; //se mueve para la derecha
            sideDistX = (posicion.X + 1f - mapX) * deltaDistX;
        }

        if (anguloVector.Y < 0)
        {
            stepY = -1; //se mueve para la izquierda
            sideDistY = (posicion.Y - mapY) * deltaDistY;
        }
        else
        {
            stepY = 1; //se mueve para la derecha
            sideDistY = (posicion.Y + 1f - mapY) * deltaDistY;
        }

        bool hit = false; //se golpeo un pared
        int side = 0; // tipo de pared 0 = vertical 1 = horizontal
        //loop principal
        while (!hit)
        {
            if (sideDistX < sideDistY) //decidimos si dar un paso en X o Y
            {
                sideDistX += deltaDistX;
                mapX += stepX; // avanzamos una casilla en X
                side = 0; // indicamos que cruzamos un linea vertical
            }
            else
            {
                sideDistY += deltaDistY;
                mapY += stepY; // avanzamos una casilla en Y
                side = 1; // indicamos que cruzamos un linea horizontal
            }

            if (EsPared(mapX, mapY))
            {
                hit = true; // verifica si hay una pared en la coordenada
            }
        }

        float WallDist;

        if (side == 0)
        {
            // Golpeamos una pared vertical
            WallDist = (mapX - posicion.X + (1 - stepX) / 2) / anguloVector.X;
            return WallDist;
        }
        else
        {
            // Golpeamos una pared horizontal  
            WallDist = (mapY - posicion.Y + (1 - stepY) / 2) / anguloVector.Y;
            return WallDist;
        }
    }

    public float[] RayCastFov(Jugador jugador, float resolucion = 1024, float distanciaMaxima = 20f)
    {
        float pasoDeAngulo = jugador.campoDeVision / resolucion;
        float[] distancias = new float[(int)resolucion];
        float anguloMinimo = jugador.angulo - (jugador.campoDeVision / 2);
        float anguloActual = anguloMinimo;

        for (int i = 0; i < resolucion; i++)
        {
            distancias[i] = RayCast(jugador.posicion, anguloActual, distanciaMaxima);
            anguloActual += pasoDeAngulo;
        }

        return distancias;
    }

    public bool EsPared(float x, float y)
    {
        try
        {
            if (mapa_terreno[(int)x, (int)y] == 1)
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