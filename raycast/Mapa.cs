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
                {1,0,0,0,0,0,1,0,0,1},
                {1,0,0,1,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,1,0,0,1},
                {1,0,0,1,0,1,0,0,0,1},
                {1,0,1,0,1,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,1},
                {1,1,1,1,1,1,1,1,1,1}
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

    public float[] RayCastFov(Jugador jugador, int resolucion = 320, float distanciaMaxima = 20f)
    {
        float anguloMinimo = jugador.angulo - (jugador.campoDeVision / 2);
        float anguloActual = anguloMinimo;
        float pasoAngulo = jugador.campoDeVision / resolucion;
        float[] distancias = new float[resolucion];

        for (int i = 0; i < resolucion; i++)
        {
            distancias[i] = RayCast(jugador.posicion, anguloActual);
            anguloActual += pasoAngulo;
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