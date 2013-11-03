using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assembly.IO;

namespace HscTool.Blam
{
    public class TagHeader
    {
        private Int32 _classCount;
        private UInt32 _classTableAddress;
        private Int32 _tagCount;
        private UInt32 _tagTableAddress;

        public Int32 classCount { get { return _classCount; } }
        public UInt32 classTableAddress { get { return _classTableAddress; } }
        public Int32 tagCount { get { return _tagCount; } }
        public UInt32 tagTableAddress { get { return _tagTableAddress; } }

        public TagHeader(IReader reader)
        {
            Load(reader);
        }


        public void Load(IReader reader)
        {
            _classCount = reader.ReadInt32();
            _classTableAddress = reader.ReadUInt32();
            _tagCount = reader.ReadInt32();
            _tagTableAddress = reader.ReadUInt32();
        }
    }
}
