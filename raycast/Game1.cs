using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace raycast;


public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public Jugador jugador;
    public Mapa mapa;
    public RayCastRenderer rayCastRenderer;
    public KeyboardState keyboardState;
    public GamePadState gamePadState;
    public List<Entidad> listaEntidades;
    public Entidad entidadPrueba;
    public ServidorManger servidorManger;
    public ClienteManager clienteManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.PreferredBackBufferWidth = 1280;
        
        
    }

    protected override void Initialize()
    {
        GestorTexturas.CargarTexturas(Content);
        jugador = new Jugador(sprite: Content.Load<Texture2D>("Assets/Sprites/placeHolder"));
        mapa = new Mapa();
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        listaEntidades = new List<Entidad>();
        rayCastRenderer = new RayCastRenderer(_spriteBatch, jugador, _graphics.PreferredBackBufferHeight, _graphics.PreferredBackBufferWidth, mapa, listaEntidades);

        rayCastRenderer.listaEntidades.Add(jugador);

        servidorManger = new ServidorManger(jugador);
        clienteManager = new ClienteManager(jugador);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        servidorManger.Update();
        clienteManager.Update();


        keyboardState = Keyboard.GetState();
        gamePadState = GamePad.GetState(0);

        jugador.Update((float)gameTime.ElapsedGameTime.TotalSeconds, keyboardState, gamePadState, mapa);

        if (keyboardState.IsKeyDown(Keys.U) && !servidorManger.server.IsRunning)
        {
            servidorManger.Iniciar(5000, 10);
        }
        if (keyboardState.IsKeyDown(Keys.I) && !clienteManager.client.IsConnected)
        {
            clienteManager.Conectarse("127.0.0.1:5000");
        }



        if (servidorManger.server.IsRunning)
        {
            servidorManger.MandarActualizarJugadores();
        }

        if (clienteManager.client.IsConnected)
        {
            clienteManager.ActualizarJugador();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        rayCastRenderer.DibujarFrame();
        _spriteBatch.End();

        

        base.Draw(gameTime);
    }
}
