//# -*- coding: utf-8 -*-

using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Game.Framework;
using PSMPatternShader;

namespace Game.Framework
{
	// SimpleSpriteのFrameBuffer
	public class SimpleFrameBuffer : IDisposable
	{
		protected GraphicsContext graphics;	// 参照

		protected FrameBuffer m_frameBuffer;
		protected Texture2D m_Texture;
		protected SimpleSprite m_Spr;
#if DEBUG
		public SimpleSprite m_dbgSpr = null;
#endif

//		public FrameBuffer FrameBuf {
//			get { return m_frameBuffer; }
//		}

		public Texture2D Texture {
			get { return m_Texture; }
		}

		public void SetVFlip() {
			m_Spr.SetTextureVFlip();
		}

		public SimpleFrameBuffer( GraphicsContext gc, int width, int height, ShaderProgram shader=null )
		{
			graphics = gc;
			m_frameBuffer = new FrameBuffer();
			m_Texture = new Texture2D( width, height, false, PixelFormat.Rgba, PixelBufferOption.Renderable );
			m_frameBuffer.SetColorTarget( m_Texture, 0 );
			m_Spr = new SimpleSprite( gc, AppMain.pixel_texture, m_frameBuffer, shader );
			m_Spr.Scale = new Vector2( width, height );
		}

		public void InitDebug( bool vflip=true )
		{
#if DEBUG
			//for debug
			m_dbgSpr = new SimpleSprite( graphics, m_Texture );
			if( vflip ){
				m_dbgSpr.SetTextureVFlip();
			}
#endif
		}

		public void RenderDebug( Vector2 pos=default(Vector2) )
		{
#if DEBUG
			if( m_dbgSpr != null ){
				m_dbgSpr.Position.Xy = pos;
				m_dbgSpr.Render();
			}
#endif
		}

		public virtual void Dispose()
		{
#if DEBUG
			if( m_dbgSpr != null ){
				m_dbgSpr.Dispose();
				m_dbgSpr = null;
			}
#endif
			if( m_Spr != null ){
				m_Spr.Dispose();
				m_Spr = null;
			}
			if( m_Texture != null ){
				m_Texture.Dispose();
				m_Texture = null;
			}
			if( m_frameBuffer != null ){
				m_frameBuffer.Dispose();
				m_frameBuffer = null;
			}
		}

		public void RenderTexture()
		{
			FrameBuffer prev = graphics.GetFrameBuffer( );
			var prev_vp = graphics.GetViewport();
			graphics.SetFrameBuffer( m_frameBuffer );
			graphics.SetViewport( 0, 0, m_frameBuffer.Width, m_frameBuffer.Height );
			graphics.SetClearColor( 0.0f, 0.0f, 0.0f, 1.0f );
			graphics.Clear();

			m_Spr.Render();

			// frame buffer戻す
			graphics.SetFrameBuffer( prev );
			graphics.SetViewport( prev_vp );
		}

		public void ClearTexture()
		{
			FrameBuffer prev = graphics.GetFrameBuffer( );
			var prev_vp = graphics.GetViewport();
			graphics.SetFrameBuffer( m_frameBuffer );
			graphics.SetViewport( 0, 0, m_frameBuffer.Width, m_frameBuffer.Height );
			graphics.SetClearColor( 0.0f, 0.0f, 0.0f, 1.0f );
			graphics.Clear();

			// frame buffer戻す
			graphics.SetFrameBuffer( prev );
			graphics.SetViewport( prev_vp );
		}
	}


	// menger sponge test、重い
	class Menger : SimpleFrameBuffer
	{
		ShaderProgram m_shader;
		float m_timer = 0.0f;

		public Menger( GraphicsContext gc, int width, int height )
			: base( gc, width, height )
		{
			var filename = "/Application/shaders/Menger.cgx" ;
			m_shader = new ShaderProgram( filename );
			m_shader.SetUniformBinding( 1, "Delta" );

			m_Spr.shaderProgram = m_shader;
		}

		public void UpdateTexture( float delta )
		{
			m_timer += delta;
			m_shader.SetUniformValue( 1, m_timer );

			this.RenderTexture();
		}
	}

	class Raymarching : SimpleFrameBuffer
	{
		ShaderProgram m_shader;
		float m_timer = 0.0f;

		public Raymarching( GraphicsContext gc, int width, int height )
			: base( gc, width, height )
		{
			var filename = "/Application/shaders/Kishimen.cgx" ;
			m_shader = new ShaderProgram( filename );
			//m_shader = ShaderManager.GetShader( "Kishimen" );
			m_shader.SetUniformBinding( 1, "Delta" );

			m_Spr.shaderProgram = m_shader;
		}

		public void UpdateTexture( float delta )
		{
			m_timer += delta;
			m_shader.SetUniformValue( 1, m_timer );

			this.RenderTexture();
		}
	}
}
