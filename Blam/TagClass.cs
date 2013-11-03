using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assembly.IO;

namespace HscTool.Blam
{
    public class TagClass
    {
        private string _name;
        private string _parent;
        private string _grandparent;
        private UInt32 _unknown;
        private Int16 _classIndex;

        public string Name { get { return _name; } }
        public string Parent { get { return _parent; } }
        public string Grandparent { get { return _grandparent; } }
        public UInt32 Unknown { get { return _unknown; } }
        public Int16 ClassIndex { get { return _classIndex; } }

        public void Load(IReader reader)
        {
            _name = reader.ReadAscii(4);
            _parent = reader.ReadAscii(4);
            _grandparent = reader.ReadAscii(4);
            _unknown = reader.ReadUInt32();
        }

        public TagClass(IReader reader, Int16 Index)
        {
            Load(reader);
            _classIndex = Index;
        }
    }
}
