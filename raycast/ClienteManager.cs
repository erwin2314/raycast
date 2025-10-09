using System;
using System.Collections.Generic;
using Riptide;

public class ClienteManager
{
    public static ClienteManager instancia; //para referirse al objeto desde funciones estaticas se utiliza esta variable
    public Client client;
    public Jugador jugadorLocal;
    public Dictionary<int, Jugador> jugadores;

    public ClienteManager(Jugador jugadorLocal)
    {
        instancia = this;
        client = new Client();
        this.jugadorLocal = jugadorLocal;
        jugadores = new Dictionary<int, Jugador>();

        client.Disconnected += DesconeccionForzoza;
    }

    public void Conectarse(string IpPuerto)// debe ir la ip y el puerto ej(127.0.0.1:7777)
    {
        client.Connect(IpPuerto);
    }

    public void Desconectarse()
    {
        client.Disconnect();
    }

    public void DesconeccionForzoza(Object sender, DisconnectedEventArgs e)
    {

    }

    public void Update()
    {
        client.Update();
    }

    public void ActualizarJugador()
    {
        if (jugadorLocal != null)
        {
            Message mensaje = Message.Create(MessageSendMode.Unreliable, ServidorManager.IdMensaje.ActualizarJugador);
            jugadorLocal.SerializarObjetoParcial(mensaje);
            client.Send(mensaje);
        }
    }

    public void AñadirJugadoresARenderer()
    {
        foreach (KeyValuePair<int, Jugador> item in jugadores)
        {
            if (!RayCastRenderer.instancia.listaEntidades.Contains(item.Value) && item.Value.existeEnLocal == false)
            {
                RayCastRenderer.instancia.listaEntidades.Add(item.Value);
            }
        }
    }

    [MessageHandler((ushort)ServidorManager.IdMensaje.PedirJugador)]
    public static void MandarJugadorAlServidor(Message mensaje)
    {
        if (ClienteManager.instancia.jugadorLocal == null)
        {
            return;
        }

        Message _mensaje = Message.Create(MessageSendMode.Reliable, ServidorManager.IdMensaje.MandarJugador);
        ClienteManager.instancia.jugadorLocal.SerializarObjetoCompleto(_mensaje, false);
        ClienteManager.instancia.client.Send(_mensaje);
    }

    [MessageHandler((ushort)ServidorManager.IdMensaje.CrearJugador)]
    public static void CrearJugadorDeServidor(Message mensaje)
    {
        int key = mensaje.GetInt();
        

        if (!ClienteManager.instancia.jugadores.ContainsKey(key))
        {
            ClienteManager.instancia.jugadores.Add(key, new Jugador());
            ClienteManager.instancia.jugadores[key] = ClienteManager.instancia.jugadores[key].DeserializarObjetoCompleto(mensaje);
            ClienteManager.instancia.AñadirJugadoresARenderer();
        }
    }

    [MessageHandler((ushort)ServidorManager.IdMensaje.ActualizarJugadorServidor)]
    public static void ActualizarJugadoresServidor(Message mensaje)
    {
        int key = mensaje.GetInt();

        if (ClienteManager.instancia.jugadores.ContainsKey(key))
        {
            ClienteManager.instancia.jugadores[key].DeserializarObjetoParcial(mensaje);
        }
    }

}