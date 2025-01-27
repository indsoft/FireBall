#region Licenze

//    Copyright (C) 2004  Sebastian Faltoni <sebastian@dotnetfireball.net>
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
//
//    NB: Original Code Grabbed From sevenzip lzma sdk http://www.7-zip.org/
#endregion

// ICoder.h

using System;

namespace Fireball.IO.Compression.SevenZip
{
	/// <summary>
	/// The exception that is thrown when an error in input stream occurs during decoding.
	/// </summary>
	class DataErrorException : ApplicationException
	{
		public DataErrorException(): base("Data Error") { }
	}

	/// <summary>
	/// The exception that is thrown when the value of an argument is outside the allowable range.
	/// </summary>
	class InvalidParamException : ApplicationException
	{
		public InvalidParamException(): base("Invalid Parameter") { }
	}

	public interface ICodeProgress
	{
		/// <summary>
		/// Callback progress.
		/// </summary>
		/// <param name="inSize">
		/// input size. -1 if unknown.
		/// </param>
		/// <param name="outSize">
		/// output size. -1 if unknown.
		/// </param>
		void SetProgress(Int64 inSize, Int64 outSize);
	};

	public interface ICoder
	{
		/// <summary>
		/// Codes streams.
		/// </summary>
		/// <param name="inStream">
		/// input Stream.
		/// </param>
		/// <param name="outStream">
		/// output Stream.
		/// </param>
		/// <param name="inSize">
		/// input Size. -1 if unknown.
		/// </param>
		/// <param name="outSize">
		/// output Size. -1 if unknown.
		/// </param>
		/// <param name="progress">
		/// callback progress reference.
		/// </param>
		/// <exception cref="Fireball.IO.Compression.SevenZip.DataErrorException">
		/// if input stream is not valid
		/// </exception>
		void Code(System.IO.Stream inStream, System.IO.Stream outStream,
			Int64 inSize, Int64 outSize, ICodeProgress progress);
	};

	/*
	public interface ICoder2
	{
		 void Code(ISequentialInStream []inStreams,
				const UInt64 []inSizes, 
				ISequentialOutStream []outStreams, 
				UInt64 []outSizes,
				ICodeProgress progress);
	};
  */

	/// <summary>
	/// Provides the fields that represent properties idenitifiers for compressing.
	/// </summary>
	public enum CoderPropID
	{
		/// <summary>
		/// Specifies size of dictionary.
		/// </summary>
		DictionarySize = 0x400,
		/// <summary>
		/// Specifies size of memory for PPM*.
		/// </summary>
		UsedMemorySize,
		/// <summary>
		/// Specifies order for PPM methods.
		/// </summary>
		Order,
		/// <summary>
		/// Specifies number of postion state bits for LZMA (0 <= x <= 4).
		/// </summary>
		PosStateBits = 0x440,
		/// <summary>
		/// Specifies number of literal context bits for LZMA (0 <= x <= 8).
		/// </summary>
		LitContextBits,
		/// <summary>
		/// Specifies number of literal position bits for LZMA (0 <= x <= 4).
		/// </summary>
		LitPosBits,
		/// <summary>
		/// Specifies number of fast bytes for LZ*.
		/// </summary>
		NumFastBytes = 0x450,
		/// <summary>
		/// Specifies match finder. LZMA: "BT2", "BT4" or "BT4B".
		/// </summary>
		MatchFinder,
		/// <summary>
		/// Specifies number of passes.
		/// </summary>
		NumPasses = 0x460,
		/// <summary>
		/// Specifies number of algorithm.
		/// </summary>
		Algorithm = 0x470,
		/// <summary>
		/// Specifies multithread mode.
		/// </summary>
		MultiThread = 0x480,
		/// <summary>
		/// Specifies mode with end marker.
		/// </summary>
		EndMarker = 0x490
	};


	public interface ISetCoderProperties
	{
		void SetCoderProperties(CoderPropID[] propIDs, object[] properties);
	};

	public interface IWriteCoderProperties
	{
		void WriteCoderProperties(System.IO.Stream outStream);
	}

	public interface ISetDecoderProperties
	{
		void SetDecoderProperties(byte[] properties);
	}
}
