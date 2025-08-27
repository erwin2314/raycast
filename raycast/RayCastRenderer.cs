using Microsoft.Xna.Framework.Graphics;

public class RayCastRenderer
{
    public SpriteBatch spriteBatch;
    public Jugador jugador;
    public int alturaVentana;
    public int anchoVentana;

    public RayCastRenderer
    (
        SpriteBatch spriteBatch,
        Jugador jugador,
        int alturaVentana,
        int anchoVentana
    )
    {
        this.spriteBatch = spriteBatch;
        this.jugador = jugador;
    }

    public void DibujarFrame(Mapa mapa)
    {
        float[] distancias = mapa.RayCastFov(jugador);
    }
}