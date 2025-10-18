using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public static class GestorTexturas
{
    public static Dictionary<IdTextura, Texture2D> diccionarioTexturas = new Dictionary<IdTextura, Texture2D>();

    public static void AñadirTexturas(IdTextura idTextura, Texture2D textura)
    {
        diccionarioTexturas.Add(idTextura, textura);
    }
    public static void CargarTexturas(ContentManager content)
    {
        foreach (IdTextura item in System.Enum.GetValues(typeof(IdTextura)))
        {
            try
            {
                AñadirTexturas(item, content.Load<Texture2D>($"Assets/Sprites/{item}"));
            }
            catch
            {
                AñadirTexturas(item, content.Load<Texture2D>($"Assets/Sprites/placeHolder"));
            }
            
        }
    }
    
    public static Texture2D ObtenerTextura(IdTextura idTextura)
    {
        return diccionarioTexturas[idTextura];
    }
    public static Texture2D ObtenerTextura(int intIdTextura)
    {
        return diccionarioTexturas[(IdTextura)intIdTextura];
    }

    public enum IdTextura
    {
        //vacio -1
        //Placeholder 0
        //Jugadores 1-11
        //vacio
        vacio = -1,
        //PlaceHolder
        placeHolder = 0,
        //Jugadores
        jugador1 = 1,
        jugador2 = 2,
        jugador3 = 3,
        jugador4 = 4,
        jugador5 = 5,
        jugador6 = 6,
        jugador7 = 7,
        jugador8 = 8,
        jugador9 = 9,
        jugador10 = 10,
        //Paredes 100 - 110
        paredLadrillos = 100,
        paredPrueba = 101

    }
}