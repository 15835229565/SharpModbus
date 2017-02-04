﻿
using System;

namespace SharpModbus
{
	public static class ModbusHelper
	{
		public static ushort CRC16(byte[] bytes, int offset, int count)
		{
			ushort crc = 0xFFFF;
			for (int pos = offset; pos < count; pos++) {
				crc ^= (ushort)bytes[pos];
				for (int i = 8; i > 0; i--) {
					if ((crc & 0x0001) != 0) {
						crc >>= 1;
						crc ^= 0xA001;
					} else
						crc >>= 1;
				}
			}
			return (ushort)((crc >> 8) & 0x00ff | (crc << 8) & 0xff00);
		}
		
		public static byte[] EncodeBools(bool[] bools)
		{
			var count = BytesForBools(bools.Length);
			var bytes = new byte[count];
			for (var i = 0; i < count; i++) {
				bytes[i] = 0;
			}
			for (var i = 0; i < bools.Length; i++) {
				var v = bools[i];
				if (v) {
					var bi = i / 8;
					bytes[bi] |= (byte)(1 << (i % 8));
				}
			}
			return bytes;
		}
		
		public static byte[] EncodeWords(ushort[] words)
		{
			var count = 2 * words.Length;
			var bytes = new byte[count];
			for (var i = 0; i < count; i++) {
				bytes[i] = 0;
			}
			for (var i = 0; i < words.Length; i++) {
				bytes[2 * i + 0] = (byte)((words[i] >> 8) & 0xff);
				bytes[2 * i + 1] = (byte)((words[i] >> 0) & 0xff);
			}
			return bytes;
		}
		
		public static ushort BytesForWords(int count)
		{
			return (ushort)(2 * count);
		}
		
		public static byte BytesForBools(int count)
		{
			return (byte)(count == 0 ? 0 : (count - 1) / 8 + 1);
		}
		
		public static byte High(int value)
		{
			return (byte)((value >> 8) & 0xff);
		}
		
		public static byte Low(int value)
		{
			return (byte)((value >> 0) & 0xff);
		}
		/*
		public static void Set(byte[] bytes, int offset, ushort value)
		{
			bytes[offset + 0] = (byte)((value >> 8) & 0xff);
			bytes[offset + 1] = (byte)((value >> 0) & 0xff);
		}
		
		public static ushort GetUShort(byte[] bytes, int offset)
		{
			return (ushort)(
			    ((bytes[offset + 0] << 8) & 0xFF00)
			    | (bytes[offset + 1] & 0xff)
			);
		}*/
		
		public static void Copy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
		{
			for (var i = 0; i < count; i++)
				dst[dstOffset + i] = src[srcOffset + i];
		}
		
		public static bool[] Clone(bool[] values)
		{
			var clone = new bool[values.Length];
			for (var i = 0; i < values.Length; i++)
				clone[i] = values[i];
			return clone;
		}
		
		public static ushort[] Clone(ushort[] values)
		{
			var clone = new ushort[values.Length];
			for (var i = 0; i < values.Length; i++)
				clone[i] = values[i];
			return clone;
		}
	}
}