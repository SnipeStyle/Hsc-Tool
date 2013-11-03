using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

using Assembly.IO;

namespace HscTool.Blam
{
    public class CasheFile
    {
        private MapHeader _header;
        private TagHeader _tagheader;
        private List<TagClass> _classes;
        private List<Tag> _tags;
        private UInt32 _MapMagic;

        public List<TagClass> Classes { get { return _classes; } }

        public List<Tag> Tags { get { return _tags; } }

        public UInt32 MapMagic { get { return _MapMagic; } }

        public CasheFile(IReader reader)
        {
            Load(reader);
        }

        public void Load(IReader reader)
        {
            _header = new MapHeader(reader); // read map header
            _MapMagic = CalculateMapMagic();
            if (_MapMagic == 0)
                throw new Exception("Error: Failed to calculate the map magic.");
            reader.SeekTo(_header.tagTableHeaderAddress - _MapMagic);
            _tagheader = new TagHeader(reader); // read tag header
            ReadClasses(reader);
            ReadTags(reader);
        }

        public void ReadClasses(IReader reader)
        {
            reader.SeekTo(_tagheader.classTableAddress - _MapMagic);
            _classes = new List<TagClass>();
            for (Int16 i = 0; i < _tagheader.classCount; i++)
            {
                _classes.Add(new TagClass(reader, i));
            }
        }

        public void ReadTags(IReader reader)
        {
            reader.SeekTo(_tagheader.tagTableAddress - _MapMagic);
            _tags = new List<Tag>();
            for (int i = 0; i < _tagheader.tagCount; i++)
            {
                _tags.Add(new Tag(reader, _classes));
            }
        }

        public Tag GetTagByClass(string ClassName)
        {
            foreach (Tag tag in _tags)
            {
                if (tag != null && tag.Class != null && tag.Class.Name == ClassName)
                    return tag;
            }
            return null;
        }

        public List<Tag> GetTagsByClass(string ClassName)
        {
            var result = new List<Tag>();

            foreach (Tag tag in _tags)
            {
                if (tag != null && tag.Class != null && tag.Class.Name == ClassName)
                {
                    result.Add(tag);
                }
            }
            return result;
        }

        private UInt32 CalculateMapMagic()
        {
            UInt32 result = 0;

            // only applies to m05_prologue.map
            if (_header.rawStartOffset == 0)
            {
                result = _header.virtualBaseAddress - _header.eofIndexOffset;
            }
            // the normal approach
            else
            {
                result = _header.virtualBaseAddress - (_header.rawStartOffset + (UInt32)_header.rawSize);
            }
            return result;
        }
    }
}
