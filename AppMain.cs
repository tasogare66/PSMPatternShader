//# -*- coding: utf-8 -*-

using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sample;

namespace PSMPatternShader
{
	public class AppMain
	{
		private static GraphicsContext graphics;
		
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
		}

		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
		}

		public static void Render ()
		{
			// Clear the screen
			graphics.SetViewport( 0, 0, graphics.Screen.Width, graphics.Screen.Height ) ;
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();

			{
				
			}

			SampleDraw.DrawText( "PSM MassParticle", 0xffffffff, 0, 0 ) ;
			var msg = string.Format( "FrameRate {0:F2} fps / FrameTime {1:F2} ms",
									 SampleTimer.AverageFrameRate, SampleTimer.AverageFrameTime * 1000 ) ;
			SampleDraw.DrawText( msg, 0xffffffff, 0, graphics.Screen.Height - 24 ) ;

			SampleTimer.EndFrame() ;
			// Present the screen
			graphics.SwapBuffers ();
		}
	}
}
