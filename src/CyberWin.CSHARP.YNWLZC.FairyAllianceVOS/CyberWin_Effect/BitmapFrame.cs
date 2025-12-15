using Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWin_Effect
{
    public class BitmapFrame : IBitmapFrame
    {
        private readonly byte[] _pixelData;
        private readonly int _width;
        private readonly int _height;
        private readonly TimeSpan _timestamp;

        public BitmapFrame(byte[] pixelData, int width, int height, TimeSpan timestamp)
        {
            _pixelData = pixelData;
            _width = width;
            _height = height;
            _timestamp = timestamp;
        }

        public int Width => _width;
        public int Height => _height;
        public TimeSpan Timestamp => _timestamp;

        public void CopyTo(byte[] buffer)
        {
            if (buffer == null || buffer.Length < _pixelData.Length)
                throw new ArgumentException("缓冲区大小不足", nameof(buffer));
            Array.Copy(_pixelData, buffer, _pixelData.Length);
        }

        public void CopyTo(IntPtr buffer)
        {
            Marshal.Copy(_pixelData, 0, buffer, _pixelData.Length);
        }

        public void Dispose()
        {
            // 释放资源（如果有非托管资源）
        }
    }
}
