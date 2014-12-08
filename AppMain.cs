//# -*- coding: utf-8 -*-

using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Game.Framework;
using Sample;

namespace PSMPatternShader
{
	public class AppMain
	{
		private static GraphicsContext graphics;
		public static Texture2D pixel_texture;

		public static Pattern m_current;
		public static List<Pattern> m_pattern = new List<Pattern>();
		public static int m_id = 0;

		public static void Main (string[] args)
		{
			Initialize ();

			while (true) {
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
			SampleDraw.Init( graphics );
			SampleTimer.Init();

			pixel_texture = new Texture2D( 1, 1, false, PixelFormat.Rgba ) ;
			Rgba[] pixels = { new Rgba( 255, 255, 255, 255 ) } ;
			pixel_texture.SetPixels( 0, pixels ) ;

			m_pattern.Add( new Pattern( graphics, graphics.Screen.Width, graphics.Screen.Height, "PatternRing.cgx" ) );
			m_pattern.Add( new Pattern( graphics, graphics.Screen.Width, graphics.Screen.Height, "PatternHex2.cgx" ) );
			m_pattern.Add( new Pattern( graphics, graphics.Screen.Width, graphics.Screen.Height, "PatternHex.cgx" ) );
			m_pattern.Add( new Pattern( graphics, graphics.Screen.Width, graphics.Screen.Height, "PatternCircle2.cgx" ) );
			m_pattern.Add( new Pattern( graphics, graphics.Screen.Width, graphics.Screen.Height, "PatternCircle.cgx" ) );
			foreach( var fb in m_pattern ){
				fb.InitDebug();
			}
			m_id = 0;

			m_current = m_pattern[0];
		}

		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);

			if( (gamePadData.ButtonsUp & GamePadButtons.Left) != 0 ){
				NextPatter( -1 );
			}else if( (gamePadData.ButtonsUp & GamePadButtons.Right) != 0 ){
				NextPatter( 1 );
			}
		}

		public static void NextPatter( int i )
		{
			m_id += i;
			if( m_id >= m_pattern.Count ){
				m_id = 0;
			}
			if( m_id < 0 ){
				m_id = m_pattern.Count - 1;
			}
			m_current = m_pattern[ m_id ];
		}

		public static void Render ()
		{
			SampleTimer.StartFrame() ;

			// Clear the screen
			graphics.SetViewport( 0, 0, graphics.Screen.Width, graphics.Screen.Height ) ;
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();

			{
				m_current.UpdateTexture( 1.0f/60.0f );	//FIXME:time	

				m_current.RenderDebug();
			}

			SampleDraw.DrawText( "PSM PatternShader", 0xffffffff, 0, 0 ) ;
			var msg = string.Format( "FrameRate {0:F2} fps / FrameTime {1:F2} ms",
									 SampleTimer.AverageFrameRate, SampleTimer.AverageFrameTime * 1000 ) ;
			SampleDraw.DrawText( msg, 0xffffffff, 0, graphics.Screen.Height - 24 ) ;

			SampleTimer.EndFrame() ;
			// Present the screen
			graphics.SwapBuffers ();
		}
	}
}
