using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assembly.IO;

namespace HscTool.Blam
{
    public class ScriptNode
    {
        #region Fields
        private UInt16 _salt;
        private UInt16 _identity;
        private UInt16 _siblingIndex;
        private UInt16 _siblingSalt;
        private UInt32 _value;
        private UInt32 _stringOffset;
        private UInt16 _zero;
        private UInt16 _returnType;
        private UInt16 _expressionTypeNum;
        private ExpressionType _expressionType;
        private UInt16 _sourceFileIndex;
        private UInt32 _lineNumber;
        #endregion

        #region Public Access
        public UInt16 Salt { get { return _salt; } }
        public UInt16 Identity { get { return _identity; } }
        public UInt16 Siblingindex { get { return _siblingIndex; } }
        public UInt32 Value { get { return _value; } }
        public UInt32 StringOffset { get { return _stringOffset; } }
        public UInt16 Zero { get { return _zero; } }
        public UInt16 ReturnType { get { return _returnType; } }
        public UInt16 ExpressionTypeNum { get { return _expressionTypeNum; } }
        public ExpressionType Expression { get { return _expressionType; } }
        public UInt16 SourceFileIndex { get { return _sourceFileIndex; } }
        public UInt32 LineNumber { get { return _lineNumber; } }
        #endregion

        public ScriptNode(IReader reader)
        {
            Read(reader);
        }

        public void Read(IReader reader)
        {
            _salt = reader.ReadUInt16();
            _identity = reader.ReadUInt16();
            _siblingIndex = reader.ReadUInt16();
            _siblingSalt = reader.ReadUInt16();
            _value = reader.ReadUInt32();
            _stringOffset = reader.ReadUInt32();
            _zero = reader.ReadUInt16();
            _returnType = reader.ReadUInt16();
            _expressionTypeNum = reader.ReadUInt16();
            _expressionType = (ExpressionType)_expressionTypeNum;
            _sourceFileIndex = reader.ReadUInt16();
            _lineNumber = reader.ReadUInt32();
        }

        public bool UnkIsZero()
        {
            return _zero == 0;
        }
    }

    public enum ExpressionType
    {
        Syntax = 0x20,
        Expression = 0x21,
        ScriptReference = 0x22,
        Local = 0x24,
        GlobalReference = 0x29,
        ScriptParameter = 0x69,
        ExternalScript = 0x222,
        ExternalGlobal = 0x229,
        LocalReference = 0x429,
    }
}
