using Microsoft.Xna.Framework;

public class Proyectil : Entidad
{
    public Vector2 velocidad;
    public float distanciaDeColision;
    public int dueño;

    public Proyectil
    (
        Vector2 posicion = new Vector2(),
        GestorTexturas.IdTextura idTextura = GestorTexturas.IdTextura.placeHolder,
        Vector2 velocidad = new Vector2(),
        float distanciaDeColision = 1f,
        int dueño = 0
    )
    : base
    (
        posicion,
        idTextura: idTextura
    )
    {
        if (velocidad == Vector2.Zero)
        {
            this.velocidad = Vector2.One;
        }
        else
        {
            this.velocidad = velocidad;
        }

        this.distanciaDeColision = distanciaDeColision;
        this.dueño = ClienteManager.instancia.client.Id;
    }

    public Proyectil(Proyectil proyectil):base(proyectil)
    {
        this.velocidad = proyectil.velocidad;
        this.distanciaDeColision = proyectil.distanciaDeColision;
        this.dueño = proyectil.dueño;
    }
}
