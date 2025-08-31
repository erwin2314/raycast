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
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            };
        }
    }

    public float RayCast(Vector2 posicion, float angulo, float tamañoPaso = 0.005f, float distanciaMaxima = 10f) //la posicion es del jugador
    {
        Vector2 anguloVector = new Vector2(Convert.ToSingle(Math.Cos(angulo)), Convert.ToSingle(Math.Sin(angulo)));
        Vector2 posicionRayo = posicion;

        bool hit = false;

        while (!hit)
        {
            posicionRayo = posicionRayo + (anguloVector * tamañoPaso);

            if (EsPared(posicionRayo.X, posicionRayo.Y) || Vector2.Distance(posicion, posicionRayo) > distanciaMaxima)
            {
                hit = true;
            }
        }

        return Vector2.Distance(posicion, posicionRayo);
    }
    
    public bool RayCast(Vector2 posicion, Vector2 objetivo, float tamañoPaso = 0.005f, float distanciaMaxima = 10f) //la posicion es del jugador
    {
        Vector2 anguloVector = objetivo - posicion;
        Vector2 posicionRayo = posicion;

        bool hit = false; //falso si no choca con ninguna pared

        while (!hit)
        {
            posicionRayo = posicionRayo + (anguloVector * tamañoPaso);

            if (EsPared(posicionRayo.X, posicionRayo.Y) || Vector2.Distance(posicion, posicionRayo) > distanciaMaxima)
            {
                hit = true;
            }
            else if (Vector2.Distance(posicionRayo, objetivo) < 0.05f)
            {
                return false;
            }
        }

        return true;
    }

    public float[] RayCastFov(Jugador jugador, int resolucion = 256, float distanciaMaxima = 10f)
    {
        float anguloMinimo = jugador.angulo - (jugador.campoDeVision / 2);
        float anguloActual = anguloMinimo;
        float pasoAngulo = jugador.campoDeVision / resolucion;
        float[] distancias = new float[resolucion];
        float distanciaReal = 0;

        for (int i = 0; i < resolucion; i++)
        {

            distanciaReal = RayCast(jugador.posicion, anguloActual, distanciaMaxima: 10f);

            //correcion del ojo de pez
            //float diferenciaAngular = anguloActual - jugador.angulo;
            //float distanciaCorregida = distanciaReal * (float)Math.Cos(diferenciaAngular);

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