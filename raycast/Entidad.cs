using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Riptide;
public class Entidad
{
    public Vector2 posicion;
    public float velociadDeRotacion;
    public float campoDeVision;
    public float angulo;
    public float velocidadDeMovimiento;
    public Texture2D sprite;
    public GestorTexturas.IdTextura idTextura;
    public float anchoSprite;
    public float alturaSprite;
    public float distanciaAJugador;
    public PosYEnum posYEnum;
    public bool existeEnLocal;
    public bool seDibujaComoBilldoard;

    public Entidad
    (
        Vector2 posicion = new Vector2(),
        float velociadDeRotacion = 1f,
        float campoDeVision = 90,
        float angulo = 0,
        float velocidadDeMovimiento = 1,
        Texture2D sprite = null,
        GestorTexturas.IdTextura idTextura = GestorTexturas.IdTextura.placeHolder,
        float anchoSprite = 256,
        float alturaSprite = 256,
        float distanciaAJugador = 0,
        PosYEnum posYEnum = PosYEnum.centro,
        bool existeEnLocal = true,
        bool seDibujaComoBilldoard = true
    )
    {
        if (posicion == Vector2.Zero)
        {
            this.posicion = new Vector2(2, 2);
        }
        else
        {
            this.posicion = posicion;
        }

        this.velociadDeRotacion = velociadDeRotacion;
        this.campoDeVision = MathHelper.ToRadians(campoDeVision);
        this.angulo = MathHelper.ToRadians(angulo);
        this.velocidadDeMovimiento = velocidadDeMovimiento;

        this.idTextura = idTextura;
        if (sprite == null)
        {
            sprite = GestorTexturas.ObtenerTextura(idTextura);
        }
        this.sprite = sprite;
        this.anchoSprite = anchoSprite;
        this.alturaSprite = alturaSprite;
        this.distanciaAJugador = distanciaAJugador;
        this.posYEnum = posYEnum;
        this.existeEnLocal = existeEnLocal;
        this.seDibujaComoBilldoard = seDibujaComoBilldoard;
    }

    public Entidad
    (
        Entidad entidad
    )
    {
        if (entidad.posicion == Vector2.Zero)
        {
            this.posicion = new Vector2(2, 2);
        }
        else
        {
            this.posicion = entidad.posicion;
        }

        this.velociadDeRotacion = entidad.velociadDeRotacion;
        this.campoDeVision = MathHelper.ToRadians(entidad.campoDeVision);
        this.angulo = MathHelper.ToRadians(entidad.angulo);
        this.velocidadDeMovimiento = entidad.velocidadDeMovimiento;

        this.idTextura = entidad.idTextura;
        this.sprite = entidad.sprite;
        this.anchoSprite = entidad.anchoSprite;
        this.alturaSprite = entidad.alturaSprite;

        this.distanciaAJugador = entidad.distanciaAJugador;
        this.posYEnum = entidad.posYEnum;
        this.existeEnLocal = entidad.existeEnLocal;
        this.seDibujaComoBilldoard = entidad.seDibujaComoBilldoard;
    }

    public virtual void SerializarObjetoCompleto(Message mensaje)
    {
        mensaje.Add(this.posicion.X);
        mensaje.Add(this.posicion.Y);
        mensaje.Add(this.velociadDeRotacion);
        mensaje.Add(this.campoDeVision);
        mensaje.Add(this.angulo);
        mensaje.Add(this.velocidadDeMovimiento);
        mensaje.Add((int)this.idTextura);
        mensaje.Add(this.anchoSprite);
        mensaje.Add(this.alturaSprite);
        mensaje.Add(this.distanciaAJugador);
        mensaje.Add((float)this.posYEnum);
        mensaje.Add(existeEnLocal);

    }
    public virtual void SerializarObjetoParcial(Message mensaje)
    {
        mensaje.Add(this.posicion.X);
        mensaje.Add(this.posicion.Y);
    }

    public virtual Entidad DeserializarObjetoCompleto(Message mensaje)
    {
        return new Entidad(
            new Vector2(mensaje.GetFloat(), mensaje.GetFloat()),
            velociadDeRotacion: mensaje.GetFloat(),
            campoDeVision: mensaje.GetFloat(),
            angulo: mensaje.GetFloat(),
            velocidadDeMovimiento: mensaje.GetFloat(),
            idTextura: (GestorTexturas.IdTextura)mensaje.GetInt(),
            anchoSprite: mensaje.GetFloat(),
            alturaSprite: mensaje.GetFloat(),
            distanciaAJugador: mensaje.GetFloat(),
            posYEnum: (PosYEnum)mensaje.GetFloat(),
            existeEnLocal: mensaje.GetBool()
            );
    }

    public virtual void DeserializarObjetoParcial(Message mensaje)
    {
        posicion.X = mensaje.GetFloat();
        posicion.Y = mensaje.GetFloat();
    }

    public void Rotar(float deltaTime, int rotarDerecha = 1 , float escalaDeRotacion = 1f) //1 = derecha, -1 = izquierda
    {
        escalaDeRotacion = Math.Clamp(escalaDeRotacion, 0, 1);
        
        angulo = angulo + (rotarDerecha * velociadDeRotacion) * deltaTime * escalaDeRotacion;
        angulo %= MathF.Tau; // MathF.Tau es igual a 2π
    }

    public void Mover(Vector2 vectorObjetivo, float deltaTime)
    {
        posicion = Vector2.Lerp(posicion, vectorObjetivo, 0.2f * deltaTime);
    }

    public enum PosYEnum
    {
        arriba = 300,
        centro = 0,
        abajo = -300
    }
}