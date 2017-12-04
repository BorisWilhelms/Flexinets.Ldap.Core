﻿using System;
using System.Collections;

namespace Flexinets.Ldap.Core
{
    public class Tag
    {
        /// <summary>
        /// Tag in byte form
        /// </summary>
        public Byte TagByte { get; internal set; }


        public Boolean IsConstructed
        {
            get
            {
                return new BitArray(new byte[] { TagByte }).Get(5);
            }
            set
            {
                var foo = new BitArray(new byte[] { TagByte });
                foo.Set(5, value);
                var temp = new byte[1];
                foo.CopyTo(temp, 0);
                TagByte = temp[0];
            }
        }

        public TagClass Class => (TagClass)(TagByte >> 6);
        public UniversalDataType DataType => (UniversalDataType)(TagByte & 31);
        public LdapOperation LdapOperation => (LdapOperation)(TagByte & 31);
        public Byte ContextType => (byte)(TagByte & 31);


        /// <summary>
        /// Create an application tag
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="isSequence"></param>
        public Tag(LdapOperation operation)
        {
            TagByte = (byte)((byte)operation + ((byte)TagClass.Application << 6));
        }


        /// <summary>
        /// Create a universal tag
        /// </summary>
        /// <param name="isSequence"></param>
        /// <param name="operation"></param>
        public Tag(UniversalDataType dataType)
        {
            TagByte = (byte)(dataType + ((byte)TagClass.Universal << 6));
        }


        /// <summary>
        /// Create a context tag
        /// </summary>
        /// <param name="isSequence"></param>
        /// <param name="operation"></param>
        public Tag(Byte context)
        {
            TagByte = (byte)(context + ((byte)TagClass.Context << 6));
        }


        /// <summary>
        /// Parses a raw tag byte
        /// </summary>
        /// <param name="tagByte"></param>
        /// <returns></returns>
        public static Tag Parse(Byte tagByte)
        {
            return new Tag { TagByte = tagByte };
        }


        private Tag()
        {
        }
    }
}