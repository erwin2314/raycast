using Microsoft.Xna.Framework;
using System;
public class Proyectil : Entidad
{
    public Vector2 direccion;
    public float distanciaDeColision;
    public int dueño;
    public bool debeEliminarse;
    public float daño;
    public float velocidad;
    public Proyectil
    (
        Vector2 posicion = new Vector2(),
        GestorTexturas.IdTextura idTextura = GestorTexturas.IdTextura.placeHolder,
        Vector2 direccion = new Vector2(),
        float distanciaDeColision = 1f,
        int dueño = 0,
        float daño = 1,
        float velocidad = 5f
    )
    : base
    (
        posicion,
        idTextura: idTextura
    )
    {
        if (direccion == Vector2.Zero)
        {
            this.direccion = Vector2.One;
        }
        else
        {
            this.direccion = direccion;
        }
        this.velocidad = velocidad;

        this.distanciaDeColision = distanciaDeColision;
        if (ClienteManager.instancia != null && ClienteManager.instancia.client.IsConnected)
        {
            this.dueño = ClienteManager.instancia.client.Id;
        }
        else
        {
            this.dueño = 0;
        }
        this.debeEliminarse = false;
        this.dueño = dueño;
        this.daño = daño;
        
    }

    public Proyectil(Proyectil proyectil) : base(proyectil)
    {
        this.velocidad = proyectil.velocidad;
        this.distanciaDeColision = proyectil.distanciaDeColision;
        this.dueño = proyectil.dueño;
        this.debeEliminarse = proyectil.debeEliminarse;
    }

    public bool ComprobarDistanciaAEntidad(Entidad entidad)
    {
        if (distanciaDeColision < Vector2.Distance(posicion, entidad.posicion))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HacerDañoAEntidad(Entidad entidad)
    {
        if (entidad is Jugador jugador)
        {
            jugador.vidaActual -= daño;
        }
        //resto de entidades con vida
    }
    public override void Update(float deltaTime, Mapa mapa)
    {
        if(debeEliminarse == false)
        {
            if (mapa.EsPared(posicion.X + (direccion.X * velocidad * deltaTime), posicion.Y + (direccion.Y * velocidad * deltaTime)))
            {
                debeEliminarse = true;
                return;
            }
            
            MoverVelocidad(direccion, velocidad, deltaTime);
        }
        
    }
}
