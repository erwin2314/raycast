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
    public float[] paredesConDistancias;

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
        this.paredesConDistancias = new float[anchoVentana];
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
            paredesConDistancias[i] = distanciasIdTexturaWallx[i].distancias;
            var textura = GestorTexturas.ObtenerTextura(mapa.diccionarioTexturas[distanciasIdTexturaWallx[i].IdTexturas]); // toma la textura de la pared
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
        if (listaEntidades.Count <= 0)
        {
            return;
        }

        Vector2 vectorRelativo; //angulo relativo de la entidad y el jugador
        float anguloSprite = 0; //angulo de donde esta el sprite con respecto a donde mira el jugador (centro de la pantalla)
        float posicionRelativa = 0; //es la posicion en la pantalla
        float alturaSprite = 0;
        float anchoSprite = 0;
        float columnaCentro = 0; //posicion en la pantalla de la columna central
        float columnaInicio = 0;//posicion en la pantalla de la columna inicial
        float columnaFinal = 0;//posicion en la pantalla de la columna final
        float progresoEnTextura = 0;//indica en que parte de la textura una columna esta va de 0 a 1
        int pixelEnTextura = 0;// de izquierda a derecha indica en que pixel se encuentra
        int posicionY = 0; // posicion vertical del sprite al momento de dibujarse
        Rectangle rectanguloOrigen; // indica que parte de la textura se va a dibujar
        Rectangle rectanguloDestino; // indica en donde se va a dibujar en pantalla

        foreach(Entidad entidad in listaEntidades)
        {
            entidad.distanciaAJugador = Vector2.Distance(entidad.posicion, jugador.posicion);
            //calcula la distancia de cada entidad al jugador
        }

        listaEntidades = listaEntidades.OrderByDescending(x => x.distanciaAJugador).ToList<Entidad>();
        //ordena la lista de entidades segun su distancia por orden descendente (mas grande va primero)

        foreach(Entidad entidad in listaEntidades)
        {
            vectorRelativo = entidad.posicion - jugador.posicion;
            anguloSprite = Convert.ToSingle(Math.Atan2(vectorRelativo.Y, vectorRelativo.X)) - jugador.angulo;

            //Normalizar entre -Pi y Pi
            while (anguloSprite < -Math.PI) anguloSprite += 2 * (Single)Math.PI;
            while (anguloSprite > Math.PI) anguloSprite -= 2 * (Single)Math.PI;

            //el resultado va a resultar en un rango de (0 a 1)
            posicionRelativa = (anguloSprite / jugador.campoDeVision) + 0.5f;

            alturaSprite = entidad.alturaSprite / entidad.distanciaAJugador;
            anchoSprite = entidad.anchoSprite  / entidad.distanciaAJugador;

            columnaCentro = posicionRelativa * anchoVentana;
            columnaInicio = columnaCentro - (anchoSprite / 2);
            columnaFinal = columnaCentro + (anchoSprite / 2);

            if(columnaFinal < 0 || columnaInicio > anchoVentana)
            {
                return;
            }
            
            for (int i = (int)columnaInicio; i < columnaFinal;i++)
            {

                if (i > -1 && i < anchoVentana && paredesConDistancias[i] > entidad.distanciaAJugador)
                {
                    progresoEnTextura = (i - columnaInicio) / (columnaFinal - columnaInicio);
                    // se multiplica por el ancho de la imagen para obtener el pixel correcto
                    pixelEnTextura = (int)(progresoEnTextura * entidad.sprite.Width);
                    pixelEnTextura = Math.Clamp(pixelEnTextura, 0, entidad.sprite.Width - 1);

                    posicionY = (int)((alturaVentana / 2f) + (int)entidad.posYEnum - (alturaSprite/2));
                    rectanguloOrigen = new Rectangle(pixelEnTextura, 0, 1, entidad.sprite.Height);
                    rectanguloDestino = new Rectangle(i, posicionY, 1, (int)alturaSprite);

                    spriteBatch.Draw(entidad.sprite, rectanguloDestino, rectanguloOrigen, Color.White);
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