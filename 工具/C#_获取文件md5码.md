# 作用

MD5码是一个文件的指纹，可以根据比较MD5码来判断两个文件内容是否完全一致。

# 下载

直链接：[获取文件md5码.exe](获取文件md5码.exe)

# 源代码

```c#
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        while(true)
        {
            Console.WriteLine("请输入文件路径:");
            string filePath = Console.ReadLine().Replace("\"","");
            // 检查文件是否存在  
            if (!File.Exists(filePath))
            {
                Console.WriteLine("文件不存在。\n");
                continue;
            }
            Console.WriteLine("请选择：1.只计算MD5码  2.计算各种码");
            var key = Console.ReadKey();
            bool isAll = key.KeyChar == '2';
            Console.WriteLine();
            try
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    //计算MD5
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] hashBytes = md5.ComputeHash(stream);

                        // 将字节数组转换为十六进制字符串  
                        StringBuilder sb = new StringBuilder();
                        foreach (byte b in hashBytes)
                        {
                            sb.Append(b.ToString("x2"));
                        }

                        // 显示MD5哈希值  
                        Console.WriteLine("文件的MD5哈希值是: " + sb.ToString());
                    }
                    if (isAll)
                    {
                        //计算CRC32
                        using (CRC32 crc32 = new CRC32())
                        {
                            stream.Position = 0;
                            byte[] hashBytes = crc32.ComputeHash(stream);
                            StringBuilder sb = new StringBuilder();
                            foreach (byte b in hashBytes)
                            {
                                sb.Append(b.ToString("X2"));
                            }
                            // 显示CRC32哈希值
                            Console.WriteLine("文件的CRC32哈希值是: " + sb.ToString());
                        }

                        //计算SHA1
                        using (SHA1 sha1 = SHA1.Create())
                        {
                            stream.Position = 0;
                            byte[] sha1Hash = sha1.ComputeHash(stream);
                            Console.WriteLine("文件的SHA1哈希值是: " + BitConverter.ToString(sha1Hash).Replace("-", "").ToLower());
                        }
                        //计算SHA256
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            stream.Position = 0;
                            byte[] sha256Hash = sha256.ComputeHash(stream);
                            Console.WriteLine("文件的SHA256哈希值是: " + BitConverter.ToString(sha256Hash).Replace("-", "").ToLower());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕获并显示任何异常  
                Console.WriteLine("发生错误: " + ex.Message);
            }
            Console.WriteLine();
        }
    }



    // CRC32 计算类，CRC32校验码多见于压缩包中
    public class CRC32 : HashAlgorithm
    {
        private static readonly uint[] table;
        //静态构造函数中预计算 CRC32 表，以提高计算效率
        static CRC32()
        {
            // 预计算 CRC32 表  
            const uint polynomial = 0xEDB88320;
            table = new uint[256];
            uint value;
            for (uint i = 0; i < table.Length; ++i)
            {
                value = i;
                for (uint j = 8; j > 0; --j)
                {
                    if (((value & 0x00000001) != 0))
                    {
                        value = (value >> 1) ^ polynomial;
                    }
                    else
                    {
                        value >>= 1;
                    }
                }
                table[i] = value;
            }
        }

        public override int HashSize => 32;

        public override void Initialize()
        {
            hashValue = 0xFFFFFFFF;
        }

        //逐字节处理输入数据，并更新当前的 CRC32 值
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            unchecked
            {
                uint crc = hashValue;
                for (int i = ibStart; i < ibStart + cbSize; ++i)
                {
                    byte b = array[i];
                    byte tableIndex = (byte)(((crc) & 0xFF) ^ b);
                    crc = (crc >> 8) ^ table[tableIndex];
                }
                hashValue = crc;
            }
        }

        //返回最终的 CRC32 校验码，并反转最高位
        protected override byte[] HashFinal()
        {
            unchecked
            {
                hashValue ^= 0xFFFFFFFF;
                return BitConverter.GetBytes(hashValue);
            }
        }

        private uint hashValue;
    }
}
```

**注意：**

- 压缩包中看到的CRC32码是文件压缩之后的，不是原文件的。

- SHA1算法虽然曾经非常流行，但现在被认为在密码学上是不安全的，因为它容易受到碰撞攻击。对于新的安全需求，建议使用更安全的哈希算法，如SHA-256或SHA-3。如果你需要计算SHA-256哈希值，只需将上面的代码中的`SHA1`替换为`SHA256`，并相应地调整方法名和变量名即可。

