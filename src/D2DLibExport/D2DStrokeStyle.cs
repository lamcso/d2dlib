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
	public class D2DStrokeStyle : D2DObject
	{
		public D2DDevice Device { get; }

		public float[]? Dashes { get; }

		public float DashOffset { get; }

		public D2DCapStyle StartCap { get; } = D2DCapStyle.Flat;

		public D2DCapStyle EndCap { get; } = D2DCapStyle.Flat;

		internal D2DStrokeStyle(D2DDevice Device, HANDLE handle, float[]? dashes, float dashOffset, D2DCapStyle startCap, D2DCapStyle endCap)
			: base(handle)
		{
			this.Device = Device;
			this.Dashes = dashes;
			this.DashOffset = dashOffset;
			this.StartCap = startCap;
			this.EndCap = endCap;
		}
	}
}
