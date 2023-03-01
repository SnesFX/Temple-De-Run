using System;
using System.Collections.Generic;
using System.Text;

internal class Warble
{
	private byte[] prs = new byte[256];

	private byte prsPosition;

	public Warble(int seed)
	{
		Random random = new Random(seed);
		random.NextBytes(prs);
	}

	public byte[] Blabidy(ref byte[] data)
	{
		prsPosition = 0;
		int num = data.Length;
		byte[] array = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = (byte)(data[i] ^ prs[prsPosition++]);
		}
		return array;
	}

	public byte[] Blop(ref byte[] data)
	{
		return Blabidy(ref data);
	}

	public byte[] Honkus(ref byte[] data)
	{
		byte b = prs[0];
		byte b2 = prs[64];
		byte b3 = prs[128];
		byte b4 = prs[192];
		int num = data.Length;
		for (int i = 0; i < num; i++)
		{
			byte b5 = data[i];
			b = (byte)(b + b5);
			if (i % 4 == 0)
			{
				b2 = (byte)(b2 + (byte)(b5 ^ b));
			}
			b3 = (byte)(b3 + (byte)(b5 ^ b5));
			b4 = (byte)(b4 + (byte)((b3 ^ b5) + prs[255 - b2]));
		}
		return new byte[4] { b, b2, b3, b4 };
	}

	public byte[] Bonkus(ref byte[] data)
	{
		byte b = prs[0];
		byte b2 = prs[64];
		byte b3 = prs[128];
		byte b4 = prs[192];
		int num = data.Length;
		for (int i = 0; i < num; i++)
		{
			byte b5 = data[i];
			b = (byte)(b + (byte)((int)b5 % 32 + 32));
			b2 = ((i % 3 != 0) ? ((byte)(b2 + b5)) : ((byte)(b2 + (byte)(b5 ^ b))));
			b3 = (byte)(b3 + (byte)(b5 ^ prs[b]));
			b4 = (byte)(b4 + (byte)((b3 ^ b2) + b5));
		}
		return new byte[4] { b, b2, b3, b4 };
	}

	public byte[] Jibber(ref byte[] data, bool alt)
	{
		byte[] collection = ((!alt) ? Bonkus(ref data) : Honkus(ref data));
		List<byte> list = new List<byte>(data);
		list.AddRange(collection);
		return list.ToArray();
	}

	public byte[] Jabber(ref byte[] data, bool alt)
	{
		List<byte> list = new List<byte>(data);
		byte[] data2 = list.GetRange(0, list.Count - 4).ToArray();
		byte[] A = list.GetRange(list.Count - 4, 4).ToArray();
		List<byte> list2 = new List<byte>((!alt) ? Bonkus(ref data2) : Honkus(ref data2));
		byte[] B = list2.ToArray();
		if (!BytesEqual(ref A, ref B))
		{
			return null;
		}
		return data2;
	}

	public byte[] BlabidyJibber(ref byte[] data)
	{
		byte[] data2 = Jibber(ref data, false);
		byte[] data3 = Blabidy(ref data2);
		byte[] data4 = Willy(ref data3);
		return Jibber(ref data4, true);
	}

	public byte[] BlopJabber(ref byte[] data)
	{
		byte[] data2 = Jabber(ref data, true);
		if (data2 == null)
		{
			return null;
		}
		byte[] data3 = Wonka(ref data2);
		byte[] data4 = Blop(ref data3);
		return Jabber(ref data4, false);
	}

	public byte[] Willy(ref byte[] data)
	{
		byte[] array = new byte[256];
		for (int i = 0; i < 256; i++)
		{
			array[i] = (byte)i;
		}
		byte[] array2 = new List<byte>(data).ToArray();
		int num = 0;
		int num2 = data.Length;
		int num3 = 0;
		while (num3 < num2)
		{
			byte b = array2[num];
			byte b2 = 0;
			while (array[b2] != b)
			{
				b2 = (byte)(b2 + 1);
			}
			array2[num] = b2;
			while (b2 != 0)
			{
				array[b2] = array[b2 - 1];
				b2 = (byte)(b2 - 1);
			}
			array[0] = b;
			num3++;
			num++;
		}
		return array2;
	}

	public byte[] Wonka(ref byte[] data)
	{
		byte[] array = new byte[256];
		for (int i = 0; i < 256; i++)
		{
			array[i] = (byte)i;
		}
		byte[] array2 = new List<byte>(data).ToArray();
		int num = 0;
		int num2 = data.Length;
		int num3 = 0;
		while (num3 < num2)
		{
			byte b = array2[num];
			byte b2 = (array2[num] = array[b]);
			while (b != 0)
			{
				array[b] = array[b - 1];
				b = (byte)(b - 1);
			}
			array[0] = b2;
			num3++;
			num++;
		}
		return array2;
	}

	private static bool BytesEqual(ref byte[] A, ref byte[] B)
	{
		if (A.Length != B.Length)
		{
			return false;
		}
		for (int i = 0; i < A.Length; i++)
		{
			if (A[i] != B[i])
			{
				return false;
			}
		}
		return true;
	}

	public static byte[] BytesFromString(string data)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetBytes(data);
	}

	public static string StringFromBytes(ref byte[] data)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(data, 0, data.Length);
	}
}
