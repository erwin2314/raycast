using System.Collections.Generic;
using Riptide;
public class ServidorManger
{
    Server server;
    public Dictionary<int, Jugador> jugadores;
    public ServidorManger()
    {
        server = new Server();
        jugadores = new Dictionary<int, Jugador>();
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

    public void EnClienteConectado(object sender, ServerConnectedEventArgs e)
    {

    }
    public void EnClienteDesconectado(object sender, ServerDisconnectedEventArgs e)
    {

    }

}