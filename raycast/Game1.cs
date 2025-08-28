using System;
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

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferHeight = 512;
        _graphics.PreferredBackBufferWidth = 1200;
    }

    protected override void Initialize()
    {
        jugador = new Jugador();
        mapa = new Mapa();
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        rayCastRenderer = new RayCastRenderer(_spriteBatch, jugador, _graphics.PreferredBackBufferHeight, _graphics.PreferredBackBufferWidth);

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
        Console.WriteLine(jugador.posicion);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        rayCastRenderer.DibujarFrame(mapa);
        _spriteBatch.End();

        

        base.Draw(gameTime);
    }
}
