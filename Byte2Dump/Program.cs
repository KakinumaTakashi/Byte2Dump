using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Byte2Dump
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ByteToDump _byteToDump = new ByteToDump();
            //_byteToDump.HeaderFirstLineOnly = false;
            //_byteToDump.HeaderSpan = 10;

            using (FileStream _reader = new FileStream(
                @"xxx.jpg", FileMode.Open))
            {
                byte[] _buffer = new byte[1024];
                long _readByte = _reader.Length;

                while (_readByte > 0)
                {
                    _readByte = _reader.Read(_buffer, 0, _buffer.Length);
                    _byteToDump.Add(_buffer);
                }
            }

            //_byteToDump.Add(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 });
            //_byteToDump.Add(new byte[] { 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F });

            //_byteToDump.Add(new byte[] { 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17 });
            //_byteToDump.Add(new byte[] { 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F });

            //_byteToDump.Add(new byte[] { 0x30, 0x31, 0x32, 0x33, 0x41, 0x42, 0x43, 0x44 });

            Console.WriteLine(_byteToDump.ToString());

            Console.ReadLine();
        }
    }
}
