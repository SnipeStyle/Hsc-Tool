using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assembly.IO;

namespace HscTool.Blam
{
    public class MapHeader
    {
        private string _fileMagic;
        private Int32 _version;
        private Int32 _fileSize;
        private UInt32 _tagTableHeaderAddress;
        private UInt32 _eofIndexOffset;
        private Int32 _virtualSize;
        private string _build;
        private Int16 _mapType;
        private Int16 _sharedType;
        private string _internalName;
        private string _scenarioPath;
        private UInt32 _virtualBaseAddress;
        private Int32 _xdkVersion;
        private UInt32 _rawStartOffset;
        private Int32 _rawSize;


        public string fileMagic { get { return _fileMagic; } }

        public Int32 version { get { return _version; } }

        public Int32 fileSize { get { return _fileSize; } }

        public UInt32 tagTableHeaderAddress { get { return _tagTableHeaderAddress; } }

        public UInt32 eofIndexOffset { get { return _eofIndexOffset; } }

        public Int32 virtualSize { get { return _virtualSize; } }

        public string build { get { return _build; } }

        public Int16 mapType { get { return _mapType; } }

        public Int16 sharedType { get { return _sharedType; } }

        public string internalName { get { return _internalName; } }

        public string scenarioPath { get { return _scenarioPath; } }

        public UInt32 virtualBaseAddress { get { return _virtualBaseAddress; } }

        public Int32 xdkVersion { get { return _xdkVersion; } }

        public UInt32 rawStartOffset { get { return _rawStartOffset; } }

        public Int32 rawSize { get { return _rawSize; } }



        public MapHeader(IReader reader)
        {
            Load(reader);
        }

        public void Load(IReader reader)
        {
            reader.SeekTo(0);
            _fileMagic = reader.ReadAscii(4);
            if (_fileMagic != "head")
            {
                throw new Exception("Invalid File Magic");
            }
            _version = reader.ReadInt32();
            _fileSize = reader.ReadInt32();
            reader.Skip(4);
            _tagTableHeaderAddress = reader.ReadUInt32();
            _eofIndexOffset = reader.ReadUInt32();
            _virtualSize = reader.ReadInt32();

            reader.SeekTo(0x11C);
            _build = reader.ReadAscii(32);
            _mapType = reader.ReadInt16();
            if (_mapType == 3 || _mapType == 4)
            {
                throw new Exception("Invalid Map Type:" + _mapType);
            }
            _sharedType = reader.ReadInt16();

            reader.SeekTo(0x18C);
            _internalName = reader.ReadAscii(32);
            reader.Skip(4);
            _scenarioPath = reader.ReadAscii(256);

            reader.SeekTo(0x2F8);
            _virtualBaseAddress = reader.ReadUInt32();
            _xdkVersion = reader.ReadInt32();
            if (xdkVersion != 21119)
            {
                throw new Exception("Invalid Xdk Version:" + xdkVersion);
            }

            reader.SeekTo(0x480);
            _rawStartOffset = reader.ReadUInt32();
            reader.Skip(20);
            _rawSize = reader.ReadInt32();
        }
    }
}
