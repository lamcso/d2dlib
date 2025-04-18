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

namespace unvell.D2DLib
{
	public class D2DPathGeometry : D2DGeometry
	{
		internal D2DPathGeometry(D2DDevice device, HANDLE pathHandle)
			: base(device, pathHandle)
		{
		}

		public void SetStartPoint(FLOAT x, FLOAT y)
		{
			this.SetStartPoint(new D2DPoint(x, y));
		}

		public void SetStartPoint(D2DPoint startPoint)
		{
			D2D.SetPathStartPoint(this.Handle, startPoint);
		}

		public unsafe void AddLines(ReadOnlySpan<D2DPoint> points)
		{
			fixed (D2DPoint* p = points)
			{
				D2D.AddPathLines(this.Handle, p, (UINT)points.Length);
			}
		}

		public unsafe void AddBeziers(ReadOnlySpan<D2DBezierSegment> bezierSegments)
		{
			fixed (D2DBezierSegment* s = bezierSegments)
			{
				D2D.AddPathBeziers(this.Handle, s, (UINT)bezierSegments.Length);
			}
		}

		// TODO: unnecessary API and it doesn't work very well, consider to remove
		//public void AddEllipse(D2DEllipse ellipse)
		//{
		//	D2D.AddPathEllipse(this.Handle, ref ellipse);
		//}

		public void AddArc(D2DPoint endPoint, D2DSize size, FLOAT sweepAngle,
			D2DArcSize arcSize = D2DArcSize.Small,
			D2DSweepDirection sweepDirection = D2DSweepDirection.Clockwise)
		{
			D2D.AddPathArc(this.Handle, endPoint, size, sweepAngle, arcSize, sweepDirection);
		}

		public bool FillContainsPoint(D2DPoint point)
		{
			return D2D.PathFillContainsPoint(this.Handle, point);
		}

		public bool StrokeContainsPoint(D2DPoint point, FLOAT width = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
		{
			return D2D.PathStrokeContainsPoint(this.Handle, point, width, dashStyle);
		}

		public void ClosePath()
		{
			D2D.ClosePath(this.Handle);
		}
		public void ClosePathOpen()
		{
			D2D.ClosePathOpen(this.Handle);
		}
		public override void Dispose()
		{
			if (this.Handle != IntPtr.Zero) D2D.DestroyPathGeometry(this.Handle);
			this.handle = IntPtr.Zero;
		}
	}
}
