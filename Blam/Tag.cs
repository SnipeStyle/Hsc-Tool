using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assembly.IO;

namespace HscTool.Blam
{
    public class Tag
    {
        private Int16 _classIndex;
        private TagClass _class;
        private UInt16 _salt;
        private UInt32 _metaAddress;

        public TagClass Class { get { return _class; } }
        public UInt16 salt { get { return _salt; } }
        public UInt32 metaAddress { get { return _metaAddress; } }

        public void Load(IReader reader, List<TagClass> Classes)
        {
            _classIndex = reader.ReadInt16();
            _salt = reader.ReadUInt16();
            _metaAddress = reader.ReadUInt32();
            _class = Classes[_classIndex];
        }

        public Tag(IReader reader, List<TagClass> Classes)
        {
            Load(reader, Classes);
        }
    }
}
