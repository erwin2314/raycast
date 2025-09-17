using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class RayCastRenderer
{
    public static RayCastRenderer instancia;
    public SpriteBatch spriteBatch;
    public Jugador jugador;
    public int alturaVentana;
    public int anchoVentana;
    public Texture2D texture2D;
    public Mapa mapa;
    public List<Entidad> listaEntidades;

    public RayCastRenderer
    (
        SpriteBatch spriteBatch,
        Jugador jugador,
        int alturaVentana,
        int anchoVentana,
        Mapa mapa,
        List<Entidad> listaEntidades
    )
    {
        this.spriteBatch = spriteBatch;
        this.jugador = jugador;
        this.alturaVentana = alturaVentana;
        this.anchoVentana = anchoVentana;
        this.mapa = mapa;
        this.listaEntidades = listaEntidades;
        instancia = this;

        texture2D = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        Color[] colorData = new Color[1];
        colorData[0] = Color.White;
        this.texture2D.SetData<Color>(colorData);
    }

    public void DibujarFrame()
    {
        (float distancias, int IdTexturas, float wallx)[] distanciasIdTexturaWallx = mapa.RayCastFov(jugador);
        float anchoRectangulo = anchoVentana / distanciasIdTexturaWallx.GetLength(0);
        float posicionY = 0;
        float posicionX = 0;
        float alturaRectangulo = 0;
        float intensidad = 1;
        Color colorConIntensidad = Color.Blue * intensidad;

        for (int i = 0; i < distanciasIdTexturaWallx.GetLength(0); i++)
        {
            var textura = GestorTexturas.ObtenerTextura(distanciasIdTexturaWallx[i].IdTexturas); // toma la textura de la pared
            int columnaDeLatextura = (int)(distanciasIdTexturaWallx[i].wallx * textura.Width); // revisa en que parte de la textura esta la columna
            Rectangle rectanguloDeTextura = new Rectangle(columnaDeLatextura, 0, 1, textura.Height); // crea el rectangulo con el pedazo de la textura

            posicionX = i * anchoRectangulo;

            alturaRectangulo = alturaVentana / distanciasIdTexturaWallx[i].distancias;

            posicionY = (alturaVentana - alturaRectangulo) / 2;

            intensidad = 1f - (distanciasIdTexturaWallx[i].distancias / 10);
            intensidad = Math.Clamp(intensidad, 0.01f, 1f);
            colorConIntensidad = Color.White * intensidad;

            if (distanciasIdTexturaWallx[i].IdTexturas == -1)
            {
                spriteBatch.Draw(texture2D, new Rectangle((int)posicionX, (int)posicionY, (int)anchoRectangulo, (int)alturaRectangulo), colorConIntensidad);
            }
            else
            {
                spriteBatch.Draw(textura,new Rectangle((int)posicionX, (int)posicionY, (int)anchoRectangulo, (int)alturaRectangulo),rectanguloDeTextura,colorConIntensidad);
            }
            

        }

        DibujarSprites();
    }

    public void DibujarSprites()
    {
        float anguloRelativoAlJugador = 0; // es el resultado de restar el angulo del jugador al angulo absoluto, este representa el angulo entre jugador-entidad teniendo en cuenta tambien el angulo del jugador
        Vector2 vectorDeDireccionJugadorAEntidad; // es el vector resultante de restar la posicion de la entidad y del jugador, este vector apunta del jugador a la entidad
        float anguloAbsoluto = 0; //es el angulo absoluto en el plano del mundo entre el jugador y una entidad
        float _alturaSprite = 0; //la altura que se utiliza al momento de dibujar el sprite de una entidad
        float _anchoSprite = 0; //el ancho que se utiliza al momento de dibujar el sprite de una entidad
        float posX = 0; // posicion x en que se dibuja el sprite
        float intensidad = 1f; // intensidad del color/sprite al momento de dibujarlo

        foreach (Entidad entidad in listaEntidades)
        {
            entidad.distanciaAJugador = Vector2.Distance(entidad.posicion, jugador.posicion);
            //calcula la distancia de cada entidad al jugador
        }

        listaEntidades = listaEntidades.OrderByDescending(x => x.distanciaAJugador).ToList<Entidad>();
        //ordena la lista de entidades segun su distancia por orden descendente (mas grande va primero)

        foreach (Entidad entidad in listaEntidades)
        {

            vectorDeDireccionJugadorAEntidad = entidad.posicion - jugador.posicion;
            anguloAbsoluto = MathF.Atan2(vectorDeDireccionJugadorAEntidad.Y, vectorDeDireccionJugadorAEntidad.X);

            anguloRelativoAlJugador = anguloAbsoluto - jugador.angulo;

            while (anguloRelativoAlJugador > MathF.PI) anguloRelativoAlJugador -= MathF.Tau; // mantiene el angulo relativo al jugador entre 2pi y -2pi
            while (anguloRelativoAlJugador < -MathF.PI) anguloRelativoAlJugador += MathF.Tau;

            // comprobación para ver su una entidad se dibuja
            //primero revisa su el angulo relativo se encuentra dentro del angulo de visión y que la distancia de la entidad al jugador este entre 10 y 0.1
            if (anguloRelativoAlJugador >= -(jugador.campoDeVision / 2) && anguloRelativoAlJugador <= (jugador.campoDeVision / 2) && entidad.distanciaAJugador <= 10f && entidad.distanciaAJugador > 0.1f)
            {
                // lanza un rayCast a la entidad para ver si hay una pared en el camino
                if (mapa.RayCast(jugador.posicion, entidad.posicion))
                {
                    // la altura con la que se dibuja el sprite es una relacion inversa de la altura base del sprite 
                    // entre la distancia divididad entre la altura de la ventana multiplicada con una constante
                    _alturaSprite = entidad.alturaSprite / ((entidad.distanciaAJugador / alturaVentana) * 720f);

                    // el ancho es una relación inversa entre el ancho base entre la distancia al jugador
                    _anchoSprite = entidad.anchoSprite / entidad.distanciaAJugador;

                    // esto indica el offset vertical al dibujar un sprite para que tenga una "altura" constante
                    // se calcula con una relación inversa de la altura base entre
                    // la distancia al jugador dividido por la altura de la ventana multiplicado por una constante
                    float offsetYEscalado = (float)entidad.posYEnum / ((entidad.distanciaAJugador / alturaVentana) * 720f);

                    // la posicion x en donde se dibuja
                    // Calcula la posición X en pantalla del sprite según el ángulo relativo al jugador.
                    // Normaliza el ángulo al rango (lo normaliza a [0, campoDeVision] ), lo convierte a una proporción de pantalla,
                    // y centra el sprite restando la mitad de su ancho.
                    posX = ((anguloRelativoAlJugador + jugador.campoDeVision / 2) / jugador.campoDeVision * anchoVentana) - (_anchoSprite / 2);
                    float posY = ((alturaVentana / 2) - (_alturaSprite / 2) - offsetYEscalado);

                    intensidad = (1f / entidad.distanciaAJugador) * 3.5f;
                    intensidad = Math.Clamp(intensidad, 0.01f, 1f);

                    Color colorConIntensidad = Color.White * intensidad;

                    spriteBatch.Draw(entidad.sprite, new Rectangle((int)posX, (int)posY, (int)_anchoSprite, (int)_alturaSprite), colorConIntensidad);
                }

            }

        }

    }

    public void AñadirEntidadAListaDeEntidades(Entidad entidad)
    {
        listaEntidades.Add(entidad);
    }

    public void AñadirEntidadAListaDeEntidades(List<Entidad> entidades)
    {
        foreach (Entidad item in entidades)
        {
            listaEntidades.Add(item);
        }
    }
}