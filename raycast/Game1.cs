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
    public List<Entidad> listaEntidades;
    public Entidad entidadPrueba;
    

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
        jugador = new Jugador();
        mapa = new Mapa();
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        listaEntidades = new List<Entidad>();
        rayCastRenderer = new RayCastRenderer(_spriteBatch, jugador, _graphics.PreferredBackBufferHeight, _graphics.PreferredBackBufferWidth, mapa, listaEntidades);
        entidadPrueba = new Entidad(new Vector2(3, 3), sprite: Content.Load<Texture2D>("Assets/Sprites/placeHolder"));
        for (int i = 0; i < 5; i++)
        {
            Entidad entidadT = new Entidad(entidadPrueba);
            entidadT.posicion.X += i;
           
            rayCastRenderer.listaEntidades.Add(entidadT);
        }
        
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

        keyboardState = Keyboard.GetState();
        jugador.Update((float)gameTime.ElapsedGameTime.TotalSeconds, keyboardState, mapa);

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
