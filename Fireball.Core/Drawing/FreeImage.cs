﻿
//    Copyright (C) 2005  Sebastian Faltoni <sebastian@dotnetfireball.net>
//
//    Copyright (C) 2005  Riccardo Marzi  <riccardo@dotnetfireball.net>
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using Fireball.Drawing.Internal;
using System.Runtime.InteropServices;
using System.Threading;

namespace Fireball.Drawing
{

	/// <summary>
	/// For use this class you need to have on your system the native library of 
	/// FreeImage you can grab it from http://freeimage.sourceforge.net/
	/// 
	/// This class is a Managed Wrapper for FreeImage library with the praticity of
	/// Classes.
	/// </summary>
	public class FreeImage:IDisposable,ICloneable
	{
        ~FreeImage()
        {
            Dispose();
        }

        static FreeImage()
        {
            FreeImageApi.Initialise(false);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            FreeImageApi.DeInitialise();
        }

 


        public event EventHandler TransformationCompleted;

		#region API Declarations

		[DllImport("gdi32.dll")]
		private static extern int SetDIBitsToDevice(IntPtr hdc, int x, int y, int dx, int dy, int SrcX, int SrcY, int Scan, int NumScans, IntPtr Bits, IntPtr BitsInfo, int wUsage);

		#endregion

		#region Enumerations
        /*
		public enum FreeImageFormat
		{
			Unknown = -1,
			Bmp = 0,
			Ico = 1,
			Jpg = 2,
			Jng = 3,
			Koala = 4,
			Lbm = 5,
			Iff = Lbm,
			Mng = 6,
			Pbm = 7,
			PbmRaw = 8,
			Pcd = 9,
			Pcx = 10,
			Pgm = 11,
			PgmRaw = 12,
			Png = 13,
			Ppm = 14,
			PpmRaw = 15,
			Ras = 16,
			Tga = 17,
			Tif = 18,
			Wbmp = 19,
			Psd = 20,
			Cut = 21,
			Xbm = 22,
			Xpm = 23,
			Dds = 24,
			Gif = 25
		}
		public enum FreeImageQuantize
		{
			WUQuant = 0,
			NNQuant = 1
		}
		public enum FreeImageDither
		{
			FS = 0,
			BAYER4x4 = 1,
			BAYER8x8 = 2,
			CLUSTER6x6 = 3,
			CLUSTER8x8 = 4,
			CLUSTER16x16 = 5
		}
		public enum FreeImageFilter
		{
		FILTER_BOX		  = 0,	// Box, pulse, Fourier window, 1st order (constant) b-spline
		FILTER_BICUBIC	  = 1,	// Mitchell & Netravali's two-param cubic filter
		FILTER_BILINEAR   = 2,	// Bilinear filter
		FILTER_BSPLINE	  = 3,	// 4th order (cubic) b-spline
		FILTER_CATMULLROM = 4,	// Catmull-Rom spline, Overhauser spline
		FILTER_LANCZOS3	  = 5	// Lanczos3 filter
		}
		public enum FreeImageColorChannel
		{
			RGB = 0,
			RED = 1,
			GREEN = 2,
			BLUE = 3,
			ALPHA = 4,
			BLACK = 5
		}
		public enum FreeImageType
		{
			UNKNOWN = 0,
			BITMAP = 1,
			UINT16 = 2,
			INT16 = 3,
			UINT32 = 4,
			INT32 = 5,
			FLOAT = 6,
			DOUBLE = 7,
			COMPLEX = 8
		}
		public enum FreeImageColorType
		{
			MINISBLACK = 0,
			MINISWHITE = 1,
			PALETTE = 2,
			RGB = 3,
			RGBALPHA = 4,
			CMYK = 5
		}*/

		#endregion

		#region Static Members

		public static string GetVersion()
		{
			return FreeImageApi.GetVersion();
		}
		public static string GetCopyright()
		{
			return FreeImageApi.GetCopyrightMessage();
		}

		#endregion

		#region Private Members

		private uint m_Handle = 0;
		private IntPtr m_MemPtr = IntPtr.Zero;

        private FREE_IMAGE_FORMAT ImageFormatToFIF(ImageFormat imageFormat)
		{
			string fmt = imageFormat.ToString().ToLower();
			if (fmt == "bmp")
			{
				return FREE_IMAGE_FORMAT.FIF_BMP;
			}
			if (fmt == "jpg")
			{
				return FREE_IMAGE_FORMAT.FIF_JPEG;
			}
            return FREE_IMAGE_FORMAT.FIF_UNKNOWN;
		}

		#endregion

		#region Constructors

		internal FreeImage(uint handle)
			:this(handle,IntPtr.Zero)
		{
           // FreeImageApi.Initialise(false);
		}
		internal FreeImage(uint handle,IntPtr memPtr)
		{
           // FreeImageApi.Initialise(true);

			m_Handle = handle;
			m_MemPtr = memPtr;
		}

        public FreeImage(byte[] bts,int width,int height,int pitch,uint bpp,uint redMask,
            uint greenMask,uint blueMask,bool topDown)
        {
            m_Handle = FreeImageApi.ConvertFromRawBits(bts, width, height, 
                pitch, bpp, redMask, greenMask, blueMask, topDown);
        }

        /// <summary>
        /// Initialise a new FreeImage class from a file
        /// </summary>
        /// <param name="filename">The image filename</param>
		public FreeImage(string filename)
		{
            

			FREE_IMAGE_FORMAT fif = FreeImageApi.GetFIFFromFilename(filename);
            if (fif == FREE_IMAGE_FORMAT.FIF_UNKNOWN) throw new Exception("Unknown file format");

			m_Handle = FreeImageApi.Load(fif, filename, 0);
			m_MemPtr=IntPtr.Zero;
		}


        /// <summary>
        /// Initialise a new FreeImage class from a System.Drawing.Bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="imageFormat"></param>
		public FreeImage(Bitmap bitmap, ImageFormat imageFormat)
		{

            FREE_IMAGE_FORMAT fif = ImageFormatToFIF(imageFormat);
            if (fif == FREE_IMAGE_FORMAT.FIF_UNKNOWN)
			{
				throw new Exception("Image format \"" + imageFormat.ToString() + "\" is not supported");
			}

			MemoryStream ms = new MemoryStream();
			bitmap.Save(ms, imageFormat);
			ms.Flush();

			byte[] buffer = new byte[((int)(ms.Length - 1)) + 1];
			ms.Position = 0;
			ms.Read(buffer, 0, (int)ms.Length);
			ms.Close();

			IntPtr dataPtr = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, dataPtr, buffer.Length);

			m_MemPtr = (IntPtr)FreeImageApi.OpenMemory(dataPtr, buffer.Length);
			m_Handle = FreeImageApi.LoadFromMemory(fif, (uint)m_MemPtr, 0);

		}

		#endregion

        public byte[] ConvertToRaw()
        {
            IntPtr bits = IntPtr.Zero;

            bits = Marshal.AllocHGlobal(this.Height * this.Pitch);

            FreeImageApi.ConvertToRawBits(bits, m_Handle, this.Pitch, (uint)this.Bpp, (uint)0, (uint)0, (uint)0, false);
            byte[] bts = new byte[this.Height * this.Pitch];
            Marshal.Copy(bits, bts, 0, bts.Length);

            return bts;
        }

		#region Public Properties

		public int Bpp
		{
			get
			{
				return (int)FreeImageApi.GetBPP(m_Handle);
			}
		}
		public int Pitch
		{
			get
			{
				return (int)FreeImageApi.GetPitch(m_Handle);
			}
		}
		public int DotsPerMeterX
		{
			get
			{
				return (int)FreeImageApi.GetDotsPerMeterX(m_Handle);
			}
		}
		public int DotsPerMeterY
		{
			get
			{
				return (int)FreeImageApi.GetDotsPerMeterY(m_Handle);
			}
		}
		public int Width
		{
			get
			{
				return (int)FreeImageApi.GetWidth(m_Handle);
			}
		}
		public int Height
		{
			get
			{
				return (int)FreeImageApi.GetHeight(m_Handle);
			}
		}
		public SizeF Size
		{
			get
			{
				return new SizeF(this.Width, this.Height);
			}
		}
		public int UsedColors
		{
			get
			{
				return (int)FreeImageApi.GetColorsUsed(m_Handle);
			}
		}
		public int TransparencyCount
		{
			get
			{
				return (int)FreeImageApi.GetTransparencyCount(m_Handle);
			}
		}

        public Color Background
        {
            get
            {
                var rgbquad = new RGBQUAD();

                if (!FreeImageApi.GetBackgroundColor(this.m_Handle, rgbquad))
                    return Color.Empty;

                return Color.FromArgb(rgbquad.rgbRed, 
                    rgbquad.rgbGreen, rgbquad.rgbBlue);
            }
            set
            {
                FreeImageApi.SetBackgroundColor(this.m_Handle,
                    new RGBQUAD
                    {
                        rgbBlue = value.B,
                        rgbGreen = value.G,
                        rgbRed = value.R
                    });
            }
        }

		public FREE_IMAGE_COLOR_TYPE ColorType
		{
			get
			{
                return (FREE_IMAGE_COLOR_TYPE)FreeImageApi.GetColorType(m_Handle);
			}
		}

        //public bool AdjustContrast(double percentage)
        //{
        //     return FreeImageApi.AdjustContrast(m_Handle, percentage);
        //}

        //public bool AdjustBrightness(double percentage)
        //{
        //    return FreeImageApi.AdjustBrightness(m_Handle, percentage);
        //}

        //public bool AdjustGamma(double percentage)
        //{
        //    return FreeImageApi.AdjustGamma(m_Handle, percentage);
        //}

		public FREE_IMAGE_TYPE ImageType
		{
			get
			{
				return FreeImageApi.GetImageType(m_Handle);
			}
		}

		#endregion

		#region Public Methods

		public Bitmap GetBitmap()
		{
			Bitmap bmp = new Bitmap(this.Width, this.Height);
			Graphics gfx = Graphics.FromImage(bmp);
			IntPtr ptrHDC = gfx.GetHdc();

			this.PaintToDevice(ptrHDC, 0, 0, bmp.Width, bmp.Height, 0, 0, 0, bmp.Height, 0);

			gfx.ReleaseHdc(ptrHDC);

			return bmp;
		}

        public enum ConvertFormat
        {
            Bits4,
            Bits8,
            Bits16_555,
            Bits16_565,
            Bits24,
            Bits32
        }

        /// <summary>
        /// Convert The current Image to the specified format
        /// </summary>
        /// <param name="format">The destination format of the new image</param>
        /// <returns></returns>
        public FreeImage ConvertTo(ConvertFormat format)
        {
            switch (format)
            {
                case ConvertFormat.Bits4:
                    return new FreeImage(FreeImageApi.ConvertTo4Bits(this.m_Handle));
                case ConvertFormat.Bits8:
                    return new FreeImage(FreeImageApi.ConvertTo8Bits(this.m_Handle));
                case ConvertFormat.Bits16_565:
                    return new FreeImage(FreeImageApi.ConvertTo16Bits565(this.m_Handle));
                case ConvertFormat.Bits16_555:
                    return new FreeImage(FreeImageApi.ConvertTo16Bits555(this.m_Handle));
                case ConvertFormat.Bits24:
                    return new FreeImage(FreeImageApi.ConvertTo24Bits(this.m_Handle));
                case ConvertFormat.Bits32:
                    return new FreeImage(FreeImageApi.ConvertTo32Bits(this.m_Handle));
            }
            return null;
        }

        /// <summary>
        /// Get the native FreeImage Handle
        /// </summary>
        /// <returns></returns>
        public uint GetFreeImageHwnd()
        {
        
            return m_Handle;
        }
		
        /// <summary>
        /// Paint the image on the destination graphic context
        /// </summary>
        /// <param name="destDC"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="srcX"></param>
        /// <param name="srcY"></param>
        /// <param name="scan"></param>
        /// <param name="mumScans"></param>
        /// <param name="wUsage"></param>
		public void PaintToDevice(IntPtr destDC, int x, int y, int width, int height, int srcX, int srcY, int scan, int mumScans, int wUsage)
		{
			try
			{
				IntPtr ptrBits = FreeImageApi.GetBits(m_Handle);
				IntPtr ptrInfo = FreeImageApi.FreeImage_GetInfo(m_Handle);
				SetDIBitsToDevice(destDC, x, y, width, height, srcX, srcY, scan, mumScans, ptrBits, ptrInfo, wUsage);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
        /// <summary>
        /// Paint the current freeimage to a System.Drawing.Bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="srcX"></param>
        /// <param name="srcY"></param>
		public void PaintToBitmap(Bitmap bitmap, int x, int y, int width, int height, int srcX, int srcY)
		{
			Graphics gfx = Graphics.FromImage(bitmap);

			IntPtr ptrHDC = gfx.GetHdc();

			this.PaintToDevice(ptrHDC, x, y, width, height, 0, 0, 0, this.Height, 0);

			gfx.ReleaseHdc(ptrHDC);
		}

		/// <summary>
		/// Paint the current freeimage to a System.Drawing.Graphics
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="srcX"></param>
		/// <param name="srcY"></param>
		public void PaintToGraphics(Graphics graphics, int x, int y, int width, int height, int srcX, int srcY)
		{

			IntPtr ptrHDC = graphics.GetHdc();

			this.PaintToDevice(ptrHDC, x, y, width, height, 0, 0, 0, this.Height, 0);

			graphics.ReleaseHdc(ptrHDC);
		}

        /// <summary>
        /// Rotate the image
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public FreeImage Rotate(double angle)
		{
			uint i = FreeImageApi.RotateClassic(m_Handle, angle);

            return new FreeImage(i);
		}

        /// <summary>
        /// Rotate the image, extended version
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="xShift"></param>
        /// <param name="yShift"></param>
        /// <param name="xOrigin"></param>
        /// <param name="yOrigin"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public FreeImage RotateExtended(double angle, double xShift, double yShift, double xOrigin, double yOrigin, bool mask)
		{
			uint i = FreeImageApi.RotateEx(m_Handle, angle, xShift, yShift, xOrigin, yOrigin, mask);

            return new FreeImage(i);
		}

        /// <summary>
        /// Rotate the image, extended version
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="xShift"></param>
        /// <param name="yShift"></param>
        /// <param name="xOrigin"></param>
        /// <param name="yOrigin"></param>
		public void RotateExtended(double angle, double xShift, double yShift, double xOrigin, double yOrigin)
		{
			RotateExtended(angle, xShift, yShift, xOrigin, yOrigin, false);
		}

        /// <summary>
        /// Rescale the current freeimage
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
		public FreeImage Rescale(int width, int height)
		{
			uint newHandle = FreeImageApi.Rescale(m_Handle, width, height,FREE_IMAGE_FILTER.FILTER_BICUBIC);
			return new FreeImage(newHandle);
		}

        /// <summary>
        /// Flip che current in Image vertically
        /// </summary>
        /// <returns></returns>
        public bool FlipVertical()
        {
            return FreeImageApi.FlipVertical(m_Handle);
        }

        /// <summary>
        /// Flip the current image horizontally
        /// </summary>
        /// <returns></returns>
        public bool FlipHorizontal()
        {
            return FreeImageApi.FlipHorizontal(m_Handle);
        }

        /// <summary>
        /// Converts a bitmap to 1-bit monochrome bitmap using a threshold T between [0..255]. 
        /// The function first converts the bitmap to a 8-bit greyscale bitmap. Then, any brightness 
        /// level that is less than T is set to zero, otherwise to 1. For 1-bit input bitmaps, the 
        /// function clones the input bitmap and builds a monochrome palette.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public FreeImage Threshold(byte range)
        {            
            uint i = FreeImageApi.Threshold(m_Handle, range);

            return new FreeImage(i);
        }

        /// <summary>
        /// Invert the current Image
        /// </summary>
        /// <returns></returns>
        public bool Invert()
        {
            return FreeImageApi.Invert(m_Handle);
        }

        /// <summary>
        /// Save the image to the specified filename,
        /// this method retrevie the destination image 
        /// format automatically by checking the file extension
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
		public bool Save(string filename)
		{
			if (File.Exists(filename))
				File.Delete(filename);

			FREE_IMAGE_FORMAT fif = FreeImageApi.GetFIFFromFilename(filename);
			FreeImageApi.SetPluginEnabled(fif, true);

			return FreeImageApi.Save(fif, m_Handle, filename, 0);
		}

        /// <summary>
        /// Save the image to the specified file with the specified format
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="type"></param>
        /// <returns></returns>
		public bool Save(string filename, FreeImageFormatInfo type)
		{
			if (File.Exists(filename))
				File.Delete(filename);

			FreeImageApi.SetPluginEnabled(type.Format, true);

			return FreeImageApi.Save(type.Format, m_Handle, filename, 0);
		}

		#endregion

		#region IDisposable Members

        bool disposed = false;

		public void Dispose()
		{
            if (!disposed)
            {
                      FreeImageApi.Unload(m_Handle);
                if (m_MemPtr != IntPtr.Zero) FreeImageApi.CloseMemory(m_MemPtr);
            }
            disposed = true;
			//FreeImageApi.DeInitialize();
		}

		#endregion

		#region ICloneable Members

		public FreeImage Clone()
		{
			uint clone = FreeImageApi.Clone(m_Handle);
			return new FreeImage(clone);
		}
		
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		#endregion

        #region Adjust's methods

        public bool AdjustGamma(double gamma)
        {
            return FreeImageApi.AdjustGamma(this.m_Handle, gamma);
        }

        public bool AdjustBrightness(double percentage)
        {
            return FreeImageApi.AdjustBrightness(this.m_Handle, percentage);
        }

        public bool AdjustContrast(double percentage)
        {
            return FreeImageApi.AdjustContrast(m_Handle, percentage);
        }

        #endregion


        //public void ApplyTransformation(FreeImageTransformation transform)
        //{
        //    transform.Run(this);

        //    if (TransformationCompleted != null)
        //        TransformationCompleted(this, new EventArgs());
        //}

        internal void DisposeAndSetHandle(uint hwnd)
        {
            FreeImageApi.Unload(m_Handle);

            m_Handle = hwnd;
        }
	}
}
