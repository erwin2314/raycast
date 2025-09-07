using System;
using System.Collections.Generic;
using Riptide;
public class ServidorManger
{
    public static ServidorManger instancia; //para referirse al objeto desde funciones estaticas se utiliza esta variable
    public Server server;
    public Jugador jugadorLocal;
    public Dictionary<int, Jugador> jugadores;
    public ServidorManger(Jugador jugadorLocal = null)
    {
        this.jugadorLocal = jugadorLocal;
        instancia = this;
        server = new Server();
        jugadores = new Dictionary<int, Jugador>();

        jugadores.Add(-1, jugadorLocal);
    }

    public void Iniciar(ushort puerto, ushort maximoJugadores)
    {
        try
        {
            server.Start(puerto, maximoJugadores);
            server.ClientConnected += EnClienteConectado;
            server.ClientDisconnected += EnClienteDesconectado;
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public void Parar()
    {
        server.Stop();
    }

    public void Update()
    {
        server.Update();
    }

    public void EnClienteConectado(object sender, ServerConnectedEventArgs e)
    {
        if (!jugadores.ContainsKey(e.Client.Id))
        {
            Jugador jugador = new Jugador();
            jugadores.Add(e.Client.Id, jugador);
        }
        
        Message mensaje = Message.Create(MessageSendMode.Reliable, IdMensaje.PedirJugador);
        server.Send(mensaje, e.Client.Id);
    }
    public void EnClienteDesconectado(object sender, ServerDisconnectedEventArgs e)
    {
        RayCastRenderer.instancia.listaEntidades.Remove(jugadores[e.Client.Id]);
        jugadores.Remove(e.Client.Id);
    }

    public void MandarACrearJugadores()
    {
        
        foreach (KeyValuePair<int, Jugador> item in jugadores)
        {
            Message mensaje = Message.Create(MessageSendMode.Reliable, IdMensaje.CrearJugador);
            mensaje.AddInt(item.Key);
            item.Value.SerializarObjetoCompleto(mensaje);
            server.SendToAll(mensaje);
        }
        
    }
    
    public void MandarActualizarJugadores()
    {
        
        foreach (KeyValuePair<int, Jugador> item in jugadores)
        {
            Message mensaje = Message.Create(MessageSendMode.Reliable, IdMensaje.ActualizarJugadorServidor);
            mensaje.AddInt(item.Key);
            item.Value.SerializarObjetoParcial(mensaje);
            server.SendToAll(mensaje);
        }
        
    }

    public void AñadirJugadoresARenderer()
    {

        foreach (KeyValuePair<int, Jugador> item in jugadores)
        {
            if (!RayCastRenderer.instancia.listaEntidades.Contains(item.Value))
            {
                RayCastRenderer.instancia.listaEntidades.Add(item.Value);
            }
        }
    }

    [MessageHandler((ushort)IdMensaje.MandarJugador)]
    public static void RecibirJugadorDeCliente(ushort fromClientId, Message mensaje)
    {
        ServidorManger.instancia.jugadores[fromClientId] = ServidorManger.instancia.jugadores[fromClientId].DeserializarObjetoCompleto(mensaje);
        ServidorManger.instancia.MandarACrearJugadores();
        ServidorManger.instancia.AñadirJugadoresARenderer();
    }
    [MessageHandler((ushort)IdMensaje.ActualizarJugador)]
    public static void ActualizarJugadorCliente(ushort fromClientId, Message mensaje)
    {
        if (ServidorManger.instancia.jugadores.ContainsKey(fromClientId))
        {
            ServidorManger.instancia.jugadores[fromClientId].DeserializarObjetoParcial(mensaje);
        }
        
    }

    public enum IdMensaje : ushort
    {
        PedirEntidad = 0,
        MandarEntidad = 1,
        CrearEntidad = 2,
        ActualizarEntidad = 3,
        ActualizarJugador = 4,
        PedirJugador = 5,
        MandarJugador = 6,
        CrearJugador = 7,
        ActualizarJugadorServidor = 8,
    }
}