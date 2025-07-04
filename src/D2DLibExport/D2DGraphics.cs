﻿/*
 * MIT License
 *
 * Copyright (c) 2009-2021 Jingwood, unvell.com. All right reserved.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace unvell.D2DLib
{
	public class D2DGraphics
	{
		internal HANDLE Handle { get; }

		public D2DDevice? Device { get; }

		public D2DGraphics(D2DDevice context)
			: this(context.Handle)
		{
			this.Device = context;
		}

		public D2DGraphics(HANDLE handle)
		{
			this.Handle = handle;
		}

		public void BeginRender()
		{
			D2D.BeginRender(this.Handle);
		}

		public void BeginRender(D2DColor color)
		{
			D2D.BeginRenderWithBackgroundColor(this.Handle, color);
		}

		public void BeginRender(D2DBitmap bitmap)
		{
			D2D.BeginRenderWithBackgroundBitmap(this.Handle, bitmap.Handle);
		}

		public void EndRender()
		{
			D2D.EndRender(this.Handle);
		}

		public void Flush()
		{
			D2D.Flush(this.Handle);
		}

		private bool antialias = true;

		public bool Antialias
		{
			get { return this.antialias; }
			set
			{
				if (this.antialias != value)
				{
					D2D.SetContextProperties(this.Handle,
							value ? D2DAntialiasMode.PerPrimitive : D2DAntialiasMode.Aliased);

					this.antialias = value;
				}
			}
		}

		public void SetTextAntialiasMode(D2DTextAntialiasMode textAntialiasMode)
		{
			D2D.SetTextAntialiasMode(this.Handle, textAntialiasMode);
		}

		public void DrawLine(FLOAT x1, FLOAT y1, FLOAT x2, FLOAT y2, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid,
			D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat, 
			D2DCapStyle dashCap = D2DCapStyle.Round, float miterLimit = 10, float dashOffset = 0)
		{
			DrawLine(new D2DPoint(x1, y1), new D2DPoint(x2, y2), color, weight, dashStyle, startCap, endCap, dashCap, miterLimit, dashOffset);
		}

		public void DrawLine(D2DPoint start, D2DPoint end, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid,
			D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat,
			D2DCapStyle dashCap = D2DCapStyle.Round, float miterLimit = 10, float dashOffset = 0)
		{
			D2D.DrawLine(this.Handle, start, end, color, weight, dashStyle, startCap, endCap, dashCap, miterLimit, dashOffset);
		}

		public unsafe void DrawLines(ReadOnlySpan<D2DPoint> points, D2DColor color, ReadOnlySpan<float> pattern, FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid,
			D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat,
			D2DCapStyle dashCap = D2DCapStyle.Round, float miterLimit = 10, float dashOffset = 0)
		{
			fixed (D2DPoint* p = points)
			{
				if (!pattern.IsEmpty)
				{
					fixed (float* pDash = pattern)
					{
						D2D.DrawLines(this.Handle, p, (uint)points.Length,
							color, weight, dashStyle,
							startCap, endCap, dashCap,
							miterLimit, dashOffset,
							pDash);
					}
				}
				else
				{
					D2D.DrawLines(this.Handle, p, (uint)points.Length,
						color, weight, dashStyle,
						startCap, endCap, dashCap,
						miterLimit, dashOffset,
						null);
				}
			}
		}

		public unsafe void DrawLines(ReadOnlySpan<D2DPoint> points, D2DColor color, FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid,
			D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat,
			D2DCapStyle dashCap = D2DCapStyle.Round, float miterLimit = 10, float dashOffset = 0)
		{
				DrawLines(points, color, ReadOnlySpan<float>.Empty, weight, dashStyle, startCap, endCap, dashCap, miterLimit, dashOffset);
				
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawUnconnectedLines(D2DPoint[] points, D2DColor color, FLOAT weight = 1)
		{
			D2D.DrawUnconnectedLines(this.Handle, points, (uint)points.Length, color, weight);
		}

		public void DrawEllipse(FLOAT x, FLOAT y, FLOAT width, FLOAT height, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			var ellipse = new D2DEllipse(x, y, width / 2f, height / 2f);
			ellipse.origin.X += ellipse.radiusX;
			ellipse.origin.Y += ellipse.radiusY;

			this.DrawEllipse(ellipse, color, weight, dashStyle);
		}

		public void DrawEllipse(D2DPoint origin, D2DSize radial, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			var ellipse = new D2DEllipse(origin, radial);
			this.DrawEllipse(ellipse, color, weight, dashStyle);
		}

		public void DrawEllipse(D2DPoint origin, FLOAT radialX, FLOAT radialY, D2DColor color,
			FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			var ellipse = new D2DEllipse(origin, radialX, radialY);
			this.DrawEllipse(ellipse, color, weight, dashStyle);
		}

		public void DrawEllipse(D2DEllipse ellipse, D2DColor color, FLOAT weight = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2D.DrawEllipse(this.Handle, ref ellipse, color, weight, dashStyle);
		}

		public void FillEllipse(D2DPoint p, FLOAT radial, D2DColor color)
		{
			this.FillEllipse(p, radial, radial, color);
		}

		public void FillEllipse(D2DPoint p, FLOAT w, FLOAT h, D2DColor color)
		{
			D2DEllipse ellipse = new D2DEllipse(p, w / 2, h / 2);
			ellipse.origin.X += ellipse.radiusX;
			ellipse.origin.Y += ellipse.radiusY;

			this.FillEllipse(ellipse, color);
		}

		public void FillEllipse(FLOAT x, FLOAT y, FLOAT radial, D2DColor color)
		{
			this.FillEllipse(new D2DPoint(x, y), radial, radial, color);
		}

		public void FillEllipse(FLOAT x, FLOAT y, FLOAT w, FLOAT h, D2DColor color)
		{
			this.FillEllipse(new D2DPoint(x, y), w, h, color);
		}

		public void FillEllipse(D2DEllipse ellipse, D2DColor color)
		{
			D2D.FillEllipse(this.Handle, ref ellipse, color);
		}

		public void FillEllipse(D2DEllipse ellipse, D2DBrush brush)
		{
			D2D.FillEllipseWithBrush(this.Handle, ref ellipse, brush.Handle);
		}

		public unsafe void DrawBeziers(ReadOnlySpan<D2DBezierSegment> bezierSegments,
			D2DColor strokeColor, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			fixed (D2DBezierSegment* s = bezierSegments)
			{
				D2D.DrawBeziers(Handle, s, (uint)bezierSegments.Length, strokeColor, strokeWidth, dashStyle);
			}
		}

		public void DrawPolygon(ReadOnlySpan<D2DPoint> points,
			D2DColor strokeColor, FLOAT strokeWidth = 1f, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			this.DrawPolygon(points, strokeColor, strokeWidth, dashStyle, D2DColor.Transparent);
		}

		public unsafe void DrawPolygon(ReadOnlySpan<D2DPoint> points,
			D2DColor strokeColor, FLOAT strokeWidth, D2DDashStyle dashStyle, D2DColor fillColor)
		{
			fixed (D2DPoint* p = points)
			{
				D2D.DrawPolygon(Handle, p, (uint)points.Length, strokeColor, strokeWidth, dashStyle, fillColor);
			}
		}

		public unsafe void DrawPolygon(ReadOnlySpan<D2DPoint> points,
			D2DColor strokeColor, FLOAT strokeWidth, D2DDashStyle dashStyle, D2DBrush fillBrush)
		{
			fixed (D2DPoint* p = points)
			{
				D2D.DrawPolygonWithBrush(Handle, p, (uint)points.Length, strokeColor, strokeWidth, dashStyle, fillBrush.Handle);
			}
		}

		[Obsolete("FillPolygon will be removed from later versions. Use DrawPolygon instead")]
    public void FillPolygon(ReadOnlySpan<D2DPoint> points, D2DColor fillColor)
		{
			this.DrawPolygon(points, D2DColor.Transparent, 0, D2DDashStyle.Solid, fillColor);
		}

		[Obsolete("FillPolygon will be removed from later versions. Use DrawPolygon instead")]
		public unsafe void FillPolygon(ReadOnlySpan<D2DPoint> points, D2DBrush brush)
		{
			fixed (D2DPoint* p = points)
			{
				D2D.DrawPolygonWithBrush(this.Handle, p, (uint)points.Length, D2DColor.Transparent, 0, D2DDashStyle.Solid, brush.Handle);
			}
		}

#if DEBUG
		public void TestDraw()
		{
			D2D.TestDraw(this.Handle);
		}
#endif // DEBUG

		public void PushClip(D2DRect rect)
		{
			D2D.PushClip(this.Handle, ref rect);
		}

		public void PopClip()
		{
			D2D.PopClip(this.Handle);
		}

		public D2DLayer PushLayer(D2DGeometry? geometry = null)
		{
			return PushLayer(D2DRect.Infinite, geometry);
		}

		public D2DLayer PushLayer(D2DRect rectBounds, D2DGeometry? geometry = null)
		{
			Assumes.NotNull(this.Device);
			var layer = this.Device.CreateLayer();
			return PushLayer(layer, rectBounds, geometry);
		}

		public D2DLayer PushLayer(D2DLayer layer, D2DRect rectBounds, D2DGeometry? geometry = null, D2DBrush? opacityBrush = null)
		{
			D2D.PushLayer(this.Handle, layer.Handle, rectBounds, geometry != null ? geometry.Handle : IntPtr.Zero, opacityBrush != null ? opacityBrush.Handle : IntPtr.Zero, layerOptions: LayerOptions.None);
			return layer;
		}

		public void PopLayer()
		{
			D2D.PopLayer(this.Handle);
		}

		public void SetTransform(Matrix3x2 mat)
		{
			D2D.SetTransform(this.Handle, ref mat);
		}

		public Matrix3x2 GetTransform()
		{
			Matrix3x2 mat;
			D2D.GetTransform(this.Handle, out mat);
			return mat;
		}

		public void PushTransform()
		{
			D2D.PushTransform(this.Handle);
		}

		public void PopTransform()
		{
			D2D.PopTransform(this.Handle);
		}

		public void ResetTransform()
		{
			D2D.ResetTransform(this.Handle);
		}

		public void RotateTransform(FLOAT angle)
		{
			D2D.RotateTransform(this.Handle, angle);
		}

		public void RotateTransform(FLOAT angle, D2DPoint center)
		{
			D2D.RotateTransform(this.Handle, angle, center);
		}

		public void TranslateTransform(FLOAT x, FLOAT y)
		{
			D2D.TranslateTransform(this.Handle, x, y);
		}

		public void ScaleTransform(FLOAT sx, FLOAT sy, FLOAT originX, FLOAT originY)
		{
			D2D.ScaleTransform(this.Handle, sx, sy, new Vector2(originX, originY));
		}

		public void ScaleTransform(FLOAT sx, FLOAT sy, [Optional] D2DPoint center)
		{
			D2D.ScaleTransform(this.Handle, sx, sy, center);
		}

		public void SkewTransform(FLOAT angleX, FLOAT angleY, [Optional] D2DPoint center)
		{
			D2D.SkewTransform(this.Handle, angleX, angleY, center);
		}

		public void DrawRectangle(FLOAT x, FLOAT y, FLOAT w, FLOAT h, D2DColor color, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2DRect rect = new D2DRect(x, y, w, h);
			D2D.DrawRectangle(this.Handle, ref rect, color, strokeWidth, dashStyle);
		}

		public void DrawRectangle(D2DRect rect, D2DColor color, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2D.DrawRectangle(this.Handle, ref rect, color, strokeWidth, dashStyle);
		}

		public void DrawRectangle(D2DPoint origin, D2DSize size, D2DColor color, FLOAT strokeWidth = 1,
			D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			this.DrawRectangle(new D2DRect(origin, size), color, strokeWidth, dashStyle);
		}

		public void DrawRectangle(D2DRect rect, D2DPen strokePen, FLOAT strokeWidth = 1)
		{
			D2D.DrawRectangleWithPen(this.Handle, ref rect, strokePen.Handle, strokeWidth);
		}

		public void FillRectangle(float x, float y, float width, float height, D2DColor color)
		{
			var rect = new D2DRect(x, y, width, height);
			this.FillRectangle(rect, color);
		}

		public void FillRectangle(D2DPoint origin, D2DSize size, D2DColor color)
		{
			this.FillRectangle(new D2DRect(origin, size), color);
		}

		public void FillRectangle(D2DRect rect, D2DColor color)
		{
			D2D.FillRectangle(this.Handle, ref rect, color);
		}

		public void FillRectangle(D2DRect rect, D2DBrush brush)
		{
			D2D.FillRectangleWithBrush(this.Handle, ref rect, brush.Handle);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawFillRectangle(ref D2DRect rect, D2DBrush fillBrush, D2DPen outlineBrush, float width )
		{
			D2D.DrawFillRectangle(this.Handle, ref rect, fillBrush.Handle, outlineBrush.Handle, width );
		}

		public void DrawRoundedRectangle(D2DRoundedRect roundedRect, D2DColor strokeColor, D2DColor fillColor,
			FLOAT strokeWidth = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2D.DrawRoundedRect(this.Handle, ref roundedRect, strokeColor, fillColor, strokeWidth, dashStyle);
		}

		public void DrawRoundedRectangle(D2DRoundedRect roundedRect, D2DPen strokePen, D2DBrush fillBrush, FLOAT strokeWidth = 1)
		{
			D2D.DrawRoundedRectWithBrush(this.Handle, ref roundedRect, strokePen.Handle, fillBrush.Handle, strokeWidth);
		}

		public void DrawBitmap(D2DBitmap bitmap, D2DRect destRect, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			var srcRect = new D2DRect(0, 0, bitmap.Width, bitmap.Height);
			this.DrawBitmap(bitmap, destRect, srcRect, opacity, interpolationMode);
		}

		public void DrawBitmap(D2DBitmap bitmap, D2DRect destRect, D2DRect srcRect, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			D2D.DrawD2DBitmap(this.Handle, bitmap.Handle, ref destRect, ref srcRect, opacity, interpolationMode);
		}

		public void DrawBitmap(D2DBitmapGraphics bg, D2DRect rect, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			D2D.DrawBitmapRenderTarget(this.Handle, bg.Handle, ref rect, opacity, interpolationMode);
		}

		public void DrawBitmap(D2DBitmapGraphics bg, FLOAT width, FLOAT height, FLOAT opacity = 1,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			this.DrawBitmap(bg, new D2DRect(0, 0, width, height), opacity, interpolationMode);
		}

		public void DrawGDIBitmap(HANDLE hbitmap, D2DRect rect, D2DRect srcRect, FLOAT opacity = 1, bool alpha = false,
			D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
		{
			D2D.DrawGDIBitmapRect(this.Handle, hbitmap, ref rect, ref srcRect, opacity, alpha, interpolationMode);
		}

		public void DrawTextCenter(string text, D2DColor color, string fontName, float fontSize, D2DRect rect)
		{
			this.DrawText(text, color, fontName, fontSize, rect,
				DWriteTextAlignment.Center, DWriteParagraphAlignment.Center);
		}

		public void DrawText(string text, D2DColor color, string fontName, float fontSize, FLOAT x, FLOAT y,
			DWriteTextAlignment halign = DWriteTextAlignment.Leading,
			DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near)
		{
			D2DRect rect = new D2DRect(x, y, 9999999, 9999999);  // FIXME: avoid magic number
			D2D.DrawText(this.Handle, text, color, fontName, fontSize, rect,
				D2DFontWeight.Normal,
				D2DFontStyle.Normal,
				D2DFontStretch.Normal,
				halign, valign);
		}

		public void DrawText(string text, D2DColor color, string fontName, float fontSize, D2DRect rect,
			DWriteTextAlignment halign = DWriteTextAlignment.Leading,
			DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near)
		{
			D2D.DrawText(this.Handle, text, color, fontName, fontSize, rect,
				D2DFontWeight.Normal,
				D2DFontStyle.Normal,
				D2DFontStretch.Normal,
				halign, valign);
		}

		public void DrawText(string text, D2DColor color, string fontName, float fontSize,
			D2DFontWeight fontWeight, D2DRect rect,
			DWriteTextAlignment halign = DWriteTextAlignment.Leading,
			DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near)
		{
			D2D.DrawText(this.Handle, text, color, fontName, fontSize, rect,
				fontWeight, D2DFontStyle.Normal, D2DFontStretch.Normal, halign, valign);
		}

		public void DrawText(string text, D2DColor color, string fontName, float fontSize,
			D2DFontWeight fontWeight, D2DFontStyle fontStyle, D2DFontStretch fontStretch,
			D2DRect rect,
			DWriteTextAlignment halign = DWriteTextAlignment.Leading,
			DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near)
		{
			D2D.DrawText(this.Handle, text, color, fontName, fontSize, rect,
				fontWeight, fontStyle, fontStretch, halign, valign);
		}

		public void DrawText(string text, D2DBrush brush, D2DTextFormat textFormat, D2DRect rect)
		{
			D2D.DrawStringWithBrushAndTextFormat(this.Handle, text, brush.Handle, textFormat.Handle, ref rect);
		}

		public void DrawText(D2DSolidColorBrush brush, D2DTextLayout textLayout, D2DPoint origin)
		{
			D2D.DrawStringWithLayout(this.Handle, brush.Handle, textLayout.Handle, origin);
		}

		public void DrawStrokedText(string text, D2DPoint location,
			D2DColor strokeColor, float strokeWidth,
			D2DColor fillColor,
			string fontName, float fontSize,
			D2DFontWeight fontWeight = D2DFontWeight.Normal,
			D2DFontStyle fontStyle = D2DFontStyle.Normal,
			D2DFontStretch fontStretch = D2DFontStretch.Normal)
		{
			this.DrawStrokedText(text, location.X, location.Y, strokeColor, strokeWidth, fillColor,
				fontName, fontSize, fontWeight, fontStyle, fontStretch);
		}

		public void DrawStrokedText(string text, float x, float y,
			D2DColor strokeColor, float strokeWidth,
			D2DColor fillColor,
			string fontName, float fontSize,
			D2DFontWeight fontWeight = D2DFontWeight.Normal,
			D2DFontStyle fontStyle = D2DFontStyle.Normal,
			D2DFontStretch fontStretch = D2DFontStretch.Normal)
		{
			using (var textPath = this.Device.CreateTextPathGeometry(text, fontName, fontSize,
				fontWeight, fontStyle, fontStretch))
			{
				this.TranslateTransform(x, y);

				this.FillPath(textPath, fillColor);
				this.DrawPath(textPath, strokeColor, strokeWidth);

				this.TranslateTransform(-x, -y);
			}
		}

		public D2DSize MeasureText(string text, string fontName, float fontSize, D2DSize placeSize)
		{
			D2DSize outputSize = placeSize;
			D2D.MeasureText(this.Handle, text, fontName, fontSize, ref outputSize);
			return outputSize;
		}

		public D2DSize MeasureText(D2DTextLayout textLayout)
		{
			D2DSize outputSize = new D2DSize();
			D2D.MeasureTextWithLayout(this.Handle, textLayout.Handle, ref outputSize);
			return outputSize;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MeasureText(string text, D2DTextFormat textFormat, ref D2DSize placeSize)
		{
			D2D.MeasureTextWithFormat(this.Handle, text, textFormat.Handle, ref placeSize);
		}


		public void DrawPath(D2DGeometry path, D2DColor strokeColor,
			FLOAT strokeWidth = 1f, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			D2D.DrawPath(path.Handle, strokeColor, strokeWidth, dashStyle);
		}

		public void DrawPath(D2DGeometry path, D2DPen strokePen, FLOAT strokeWidth = 1f)
		{
			D2D.DrawPathWithPen(path.Handle, strokePen.Handle, strokeWidth);
		}

		public void FillPath(D2DGeometry path, D2DColor fillColor)
		{
			D2D.FillPathD(path.Handle, fillColor);
		}

		public void Clear(D2DColor color)
		{
			D2D.Clear(Handle, color);
		}

		public void GetDPI(out float dpiX, out float dpiY)
		{
			Assumes.NotNull(this.Device);
			D2D.GetDPI(this.Device.Handle, out dpiX, out dpiY);
		}

		public void SetDPI(float dpiX, float dpiY)
		{
			Assumes.NotNull(this.Device);
			D2D.SetDPI(this.Device.Handle, dpiX, dpiY);
		}
	}

}
