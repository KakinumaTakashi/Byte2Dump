using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class ByteToDump
    {
        /// <summary>
        /// <para>ヘッダ行を出力する間隔(行数)</para>
        /// <para>0以下の数値を指定した場合はヘッダは出力されません。</para>
        /// </summary>
        public int HeaderSpan { get; set; } = 16;
        /// <summary>
        /// <para>ヘッダ行をダンプの１行目のみに出力する。</para>
        /// <para>true - 有効, false - 無効</para>
        /// </summary>
        public bool HeaderFirstLineOnly { get; set; } = true;

        /// <summary>
        /// 内部バッファ
        /// </summary>
        public byte[] ByteData = new byte[0];


        public ByteToDump()
        {
        }

        public ByteToDump(byte[] byteData)
        {
            this.SetByte(byteData);
        }

        /// <summary>
        /// バイトデータをセットする。
        /// </summary>
        /// <param name="byteData"></param>
        public void SetByte(byte[] byteData)
        {
            this.ByteData = byteData;
        }

        /// <summary>
        /// バイトデータを末尾に追加する。
        /// </summary>
        /// <param name="byteData"></param>
        public void Add(byte[] byteData)
        {
            int _destOffset = this.ByteData.Length;
            Array.Resize(ref this.ByteData, this.ByteData.Length + byteData.Length);
            Array.Copy(byteData, 0, this.ByteData, _destOffset, byteData.Length);
        }

        /// <summary>
        /// ヘッダ定義
        /// </summary>
        private const string DUMP_HEADER = "\n" +
            "ADDRESS  +0 +1 +2 +3 +4 +5 +6 +7  +8 +9 +A +B +C +D +E +F ASCII\n" +
            "--------------------------------------------------------------------------";

        /// <summary>
        /// バイトデータのダンプ文字列を返す。
        /// </summary>
        /// <returns>バイトデータのダンプ文字列</returns>
        public override string ToString()
        {
            StringBuilder _dumpBuilder = new StringBuilder();

            int _lineCount = 0;
            bool _firstHeader = true;
            for (long _offset = 0; _offset < this.ByteData.Length; _offset += 16)
            {
                if (this.HeaderFirstLineOnly)
                {
                    if (_firstHeader)
                    {
                        _dumpBuilder.AppendLine(DUMP_HEADER);
                        _firstHeader = false;
                    }
                }
                else
                {
                    if (this.HeaderSpan > 0 && _lineCount % this.HeaderSpan == 0)
                    {
                        _dumpBuilder.AppendLine(DUMP_HEADER);
                    }
                }

                byte[] _line = new byte[16];
                int _lineLength = (int)Math.Min(this.ByteData.Length - _offset, 16);
                Array.Copy(this.ByteData, _offset, _line, 0, _lineLength);

                string _buffer1 = BitConverter.ToString(_line, 0, (_lineLength < 8) ? _lineLength : 8).Replace('-', ' ');
                string _buffer2 = (_lineLength < 8) ? "" : BitConverter.ToString(_line, 8, _lineLength - 8).Replace('-', ' ');
                string _ascii = this.GetAsciiString(_line);

                _dumpBuilder.AppendLine($"{_offset:X8} {_buffer1, -24} {_buffer2, -24}{_ascii}");
                _lineCount++;
            }

            return _dumpBuilder.ToString();
        }

        /// <summary>
        /// バイトデータのASCII文字列を返す。
        /// </summary>
        /// <param name="byteData">バイトデータ</param>
        /// <returns>バイトデータのASCII文字列</returns>
        private string GetAsciiString(byte[] byteData)
        {
            ASCIIEncoding _encoding = new ASCIIEncoding();
            StringBuilder _asciiBuilder = new StringBuilder();

            for (int _index = 0; _index < byteData.Length; _index++)
            {
                // 制御文字判定
                if (byteData[_index] >= 0x20 && byteData[_index] <= 0x7E)
                {
                    // ASCIIに変換
                    _asciiBuilder.Append(_encoding.GetString(new byte[] { byteData[_index] }));
                }
                else
                {
                    // 制御文字はドットに変換
                    _asciiBuilder.Append('.');
                }
            }

            return _asciiBuilder.ToString();
        }
    }
}
