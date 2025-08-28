using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class RayCastRenderer
{
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
    }
}