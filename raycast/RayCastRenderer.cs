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
        float[] distancias = mapa.RayCastFov(jugador);
        float posicionY = 0;
        float posicionX = 0;
        float anchoRectangulo = anchoVentana / distancias.GetLength(0);
        float alturaRectangulo = 0;
        float intensidad = 1;
        Color colorConIntensidad = Color.Blue * intensidad;

        for (int i = 0; i < distancias.GetLength(0); i++)
        {
            posicionX = i * anchoRectangulo;

            alturaRectangulo = alturaVentana / distancias[i];

            posicionY = (alturaVentana - alturaRectangulo) / 2;

            intensidad = 1f - (distancias[i] / 10);
            intensidad = Math.Clamp(intensidad, 0.1f, 1f);
            colorConIntensidad = Color.Blue * intensidad;

            spriteBatch.Draw(texture2D, new Rectangle((int)posicionX, (int)posicionY, (int)anchoRectangulo, (int)alturaRectangulo), colorConIntensidad);

        }

        DibujarSprites();
    }

    public void DibujarSprites()
    {
        float anguloRelativoAlJugador = 0;
        Vector2 vectorDeDireccionJugadorAEntidad;
        float anguloAbsoluto = 0;
        float _alturaSprite = 0;
        float _anchoSprite = 0;
        float posX = 0;
        float intensidad = 1f;

        foreach (Entidad entidad in listaEntidades)
        {
            entidad.distanciaAJugador = Vector2.Distance(entidad.posicion, jugador.posicion);
        }

        listaEntidades = listaEntidades.OrderByDescending(x => x.distanciaAJugador).ToList<Entidad>();

        foreach (Entidad entidad in listaEntidades)
        {

            vectorDeDireccionJugadorAEntidad = entidad.posicion - jugador.posicion;
            anguloAbsoluto = MathF.Atan2(vectorDeDireccionJugadorAEntidad.Y, vectorDeDireccionJugadorAEntidad.X);

            anguloRelativoAlJugador = anguloAbsoluto - jugador.angulo;

            while (anguloRelativoAlJugador > MathF.PI) anguloRelativoAlJugador -= MathF.Tau;
            while (anguloRelativoAlJugador < -MathF.PI) anguloRelativoAlJugador += MathF.Tau;

            if (anguloRelativoAlJugador >= -(jugador.campoDeVision / 2) && anguloRelativoAlJugador <= (jugador.campoDeVision / 2) && entidad.distanciaAJugador <= 10f && entidad.distanciaAJugador > 0.1f)
            {
                if (mapa.RayCast(jugador.posicion, entidad.posicion))
                {
                    
                    
                    _alturaSprite = entidad.alturaSprite / ((entidad.distanciaAJugador / alturaVentana) * 720f);

                    _anchoSprite = entidad.anchoSprite / entidad.distanciaAJugador;
                    float offsetYEscalado = (float)entidad.posYEnum / ((entidad.distanciaAJugador / alturaVentana) * 720f);

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