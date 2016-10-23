using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

namespace Monogame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Paddle PaddleBottom { get; private set; }
        public Paddle PaddleTop { get; private set; }
        public Ball Ball { get; private set; }
        public Background Background { get; private set; }
        public SoundEffect HitSound { get; private set; }
        public Song Music { get; private set; }
        private IGenericList<Sprite> SpritesForDrawList = new GenericList<Sprite>();

        public List<Wall> Walls { get; set; }
        public List<Wall> Goals { get; set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 500
            };
            Content.RootDirectory = "Content";
        }
       


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
      

        protected override void Initialize()
    {

        var screenBounds = GraphicsDevice.Viewport.Bounds;
        PaddleBottom = new Paddle(GameConstants.PaddleDefaultWidth,
        GameConstants.PaddleDefaultHeight, GameConstants.PaddleDefaultSpeed);
        PaddleBottom.X = 200;
        PaddleBottom.Y = 400;
        PaddleTop = new Paddle(GameConstants.PaddleDefaultWidth,
        GameConstants.PaddleDefaultHeight, GameConstants.PaddleDefaultSpeed);
        PaddleTop.X = 300;
        PaddleTop.Y = 500;
        Ball = new Ball(GameConstants.DefaultBallSize,
        GameConstants.DefaultInitialBallSpeed,
        GameConstants.DefaultBallBumpSpeedIncreaseFactor)
        {
            X = 250,
            Y = 450
        };
        Background = new Background(500, 900);
        SpritesForDrawList.Add(Background);
        SpritesForDrawList.Add(PaddleBottom);
        SpritesForDrawList.Add(PaddleTop);
        SpritesForDrawList.Add(Ball);
            base.Initialize();
            Walls = new List<Wall>()
            {
                new  Wall(-GameConstants.WallDefaultSize ,0,
                GameConstants.WallDefaultSize , screenBounds.Height),
                new  Wall(screenBounds.Right ,0,  GameConstants.WallDefaultSize ,
                screenBounds.Height), };
            Goals = new List<Wall>()
            { 
            new  Wall(0,  screenBounds.Height , screenBounds.Width ,
           GameConstants.WallDefaultSize),
           new  Wall(screenBounds.Top ,-GameConstants.WallDefaultSize ,
           screenBounds.Width , GameConstants.WallDefaultSize),
           };

           // base.Initialize();
    }
    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        Texture2D paddleTexture = Content.Load<Texture2D>("paddle");
        PaddleBottom.Texture = paddleTexture;
        PaddleTop.Texture = paddleTexture;
        Ball.Texture = Content.Load<Texture2D>("ball");
        Background.Texture = Content.Load<Texture2D>("background");
        // Load  sounds
        // Start  background  music
        HitSound = Content.Load<SoundEffect>("hit");
        Music = Content.Load<Song>("music");
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(Music);
        // TODO: use this.Content to load your game content here
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        var touchState = Keyboard.GetState();
        if (touchState.IsKeyDown(Keys.Left))
        {
           float offset= PaddleBottom.X - (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            PaddleBottom.X = MathHelper.Clamp(offset, GraphicsDevice.Viewport.Bounds.Left, GraphicsDevice.Viewport.Bounds.Right - PaddleBottom.Width);
        }
        if (touchState.IsKeyDown(Keys.Right))
        {
           float offset = PaddleBottom.X + (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            PaddleBottom.X = MathHelper.Clamp(offset, GraphicsDevice.Viewport.Bounds.Left, GraphicsDevice.Viewport.Bounds.Right - PaddleBottom.Width);
        }
        if (touchState.IsKeyDown(Keys.A))
        {
            float offset = PaddleTop.X - (float)(PaddleTop.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            PaddleTop.X = MathHelper.Clamp(offset, GraphicsDevice.Viewport.Bounds.Left, GraphicsDevice.Viewport.Bounds.Right - PaddleTop.Width);
        }
        if (touchState.IsKeyDown(Keys.D))
        {
           float offset = PaddleTop.X + (float)(PaddleTop.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            PaddleTop.X = MathHelper.Clamp(offset, GraphicsDevice.Viewport.Bounds.Left, GraphicsDevice.Viewport.Bounds.Right - PaddleTop.Width);
        }
            var BallPositionChange = Ball.Direction * (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Ball.Speed);
            Ball.X += BallPositionChange.X;
            Ball.Y += BallPositionChange.Y;
            if (Walls.Any(w => CollisionDetector.Overlaps(Ball, w)))
            {
                Ball.Direction = new Vector2(-Ball.Direction.X, Ball.Direction.Y);
                Ball.Speed = Ball.Speed * Ball.BumpSpeedIncreaseFactor;
            }
            // Ball - winning  walls
            if (Goals.Any(w => CollisionDetector.Overlaps(Ball, w)))
            {
                Ball.X = GraphicsDevice.Viewport.Bounds.Center.ToVector2().X;
                Ball.Y = GraphicsDevice.Viewport.Bounds.Center.ToVector2().Y;
                Ball.Speed = GameConstants.DefaultInitialBallSpeed;
                HitSound.Play();
            }
            //  Paddle  - ball  collision
            if (CollisionDetector.Overlaps(Ball, PaddleTop) && Ball.Direction.Y < 0
            || (CollisionDetector.Overlaps(Ball, PaddleBottom) && Ball.Direction.Y > 0))
            {
                Ball.Direction = new Vector2(Ball.Direction.X, -Ball.Direction.Y);
                Ball.Speed *= Ball.BumpSpeedIncreaseFactor;
            }
            base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        spriteBatch.Begin();
        for (int i = 0; i < SpritesForDrawList.Count; i++)
        {
            SpritesForDrawList.GetElement(i).DrawSpriteOnScreen(spriteBatch);
        }
        spriteBatch.End();
        base.Draw(gameTime);
    }
}
    public abstract class Sprite: IPhysicalObject2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Texture2D Texture { get; set; }

        protected Sprite(int width, int height, float x=0, float y=0)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }
        public virtual void DrawSpriteOnScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0, Width, Height), Color.White);
        }
    }
    public class Background: Sprite
    {
        public Background(int width, int height): base(width, height)
        {

        }
    }
    public class Ball: Sprite
    {
        public float Speed { get; set; }
        public float BumpSpeedIncreaseFactor { get; set; }
        public Vector2 Direction { get; set; }
        public Ball(int size, float speed, float defaultBallBumpSpeedIncreaseFactor): base(size,size)
        {
            Speed = speed;
            BumpSpeedIncreaseFactor = defaultBallBumpSpeedIncreaseFactor;
            Direction = new Vector2(1, 1);
        }
    }
    public class Paddle:Sprite
    {
        public float Speed { get; set; }
        public Paddle(int width, int height, float initialSpeed): base(width,height)
        {
            Speed = initialSpeed;
        }
        public override void DrawSpriteOnScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0, Width, Height), Color.GhostWhite);
        }
    }
    public class GameConstants
    {
        public const float PaddleDefaultSpeed = 0.9f;
        public const int PaddleDefaultWidth = 200;
        public const int PaddleDefaultHeight = 20;
        public const float DefaultInitialBallSpeed = 0.4f;
        public const float DefaultBallBumpSpeedIncreaseFactor= 1.05f;
        public const int DefaultBallSize = 40;
        public const int WallDefaultSize = 100;

    }
    public interface IPhysicalObject2D
    {
        float X { get; set; }
        float Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }
    }
    public class CollisionDetector
    {
        public static bool Overlaps(IPhysicalObject2D a, IPhysicalObject2D b)
        {
            Rectangle aRect = new Rectangle((int)a.X, (int)a.Y, a.Width, a.Height);
            Rectangle bRect = new Rectangle((int)b.X, (int)b.Y, b.Width, b.Height);
            return aRect.Intersects(bRect);
        }
    }
    public class Wall: IPhysicalObject2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Wall(float x,float y,int width,int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
  
}
