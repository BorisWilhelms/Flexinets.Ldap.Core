﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Flexinets.Ldap.Core.Tests
{
    [TestClass]
    public class LdapPacketTests
    {       
        [TestMethod]
        public void TestLdapAttributeSequenceGetBytesString()
        {
            var packet = new LdapPacket(1);

            // Bind request
            var bindrequest = new LdapAttribute(LdapOperation.BindRequest, true);
            bindrequest.ChildAttributes.Add(new LdapAttribute(UniversalDataType.Integer, false, (Byte)3));
            bindrequest.ChildAttributes.Add(new LdapAttribute(UniversalDataType.OctetString, false, "cn=bindUser,cn=Users,dc=dev,dc=company,dc=com"));
            bindrequest.ChildAttributes.Add(new LdapAttribute((byte)0, false, "bindUserPassword")); 

            packet.ChildAttributes.Add(bindrequest);

            var expected = "304c0204000000016044020103042d636e3d62696e64557365722c636e3d55736572732c64633d6465762c64633d636f6d70616e792c64633d636f6d801062696e645573657250617373776f7264"; // "30490201016044020103042d636e3d62696e64557365722c636e3d55736572732c64633d6465762c64633d636f6d70616e792c64633d636f6d801062696e645573657250617373776f7264";
            Assert.AreEqual(expected, Utils.ByteArrayToString(packet.GetBytes()));
        }


        [TestMethod]
        public void TestLdapAttributeSequenceGetBytes2()
        {
            var packet = new LdapPacket(1);

            // Bind request
            var bindresponse = new LdapAttribute(LdapOperation.BindResponse, true);

            var resultCode = new LdapAttribute(UniversalDataType.Enumerated, false, (Byte)LdapResult.success);  
            bindresponse.ChildAttributes.Add(resultCode);

            var matchedDn = new LdapAttribute(UniversalDataType.OctetString, false);
            var diagnosticMessage = new LdapAttribute(UniversalDataType.OctetString, false);

            bindresponse.ChildAttributes.Add(matchedDn);
            bindresponse.ChildAttributes.Add(diagnosticMessage);

            packet.ChildAttributes.Add(bindresponse);

            var expected = "300f02040000000161070a010004000400"; // "300c02010161070a010004000400";
            Assert.AreEqual(expected, Utils.ByteArrayToString(packet.GetBytes()));
        }


        [TestMethod]
        public void TestLdapAttributeParse()
        {
            var expected = "30490201016044020103042d636e3d62696e64557365722c636e3d55736572732c64633d6465762c64633d636f6d70616e792c64633d636f6d801062696e645573657250617373776f7264";
            var packetBytes = Utils.StringToByteArray(expected);
            var packet = LdapPacket.ParsePacket(packetBytes);
            Assert.AreEqual(expected, Utils.ByteArrayToString(packet.GetBytes()));
        }


        [TestMethod]
        public void TestLdapAttributeParse2()
        {
            var expected = "041364633d6b6172616b6f72756d2c64633d6e6574";
            var packetBytes = Utils.StringToByteArray(expected);
            var packet = LdapPacket.ParsePacket(packetBytes);
            Assert.AreEqual(expected, Utils.ByteArrayToString(packet.GetBytes()));
        }


        [TestMethod]
        public void TestLdapAttributeParse3()
        {
            var expected = "30620201026340041164633d636f6d70616e792c64633d636f6d0a01020a010302010202010b010100a31a040e73414d4163636f756e744e616d65040876666f7274656c693000a01b30190417322e31362e3834302e312e3131333733302e332e342e32";
            var packetBytes = Utils.StringToByteArray(expected);
            var packet = LdapPacket.ParsePacket(packetBytes);
            Assert.AreEqual(expected, Utils.ByteArrayToString(packet.GetBytes()));
        }


        [TestMethod]
        public void TestLdapAttributeParse4()
        {
            var bytes = "30620201026340041164633d636f6d70616e792c64633d636f6d0a01020a010302010202010b010100a31a040e73414d4163636f756e744e616d65040876666f7274656c693000a01b30190417322e31362e3834302e312e3131333733302e332e342e3200000000";
            var expected = "30620201026340041164633d636f6d70616e792c64633d636f6d0a01020a010302010202010b010100a31a040e73414d4163636f756e744e616d65040876666f7274656c693000a01b30190417322e31362e3834302e312e3131333733302e332e342e32";
            var packetBytes = Utils.StringToByteArray(bytes);
            var packet = LdapPacket.ParsePacket(packetBytes);
            Assert.AreEqual(expected, Utils.ByteArrayToString(packet.GetBytes()));
        } 


        [TestMethod]
        public void TestPacketMessageId()
        {
            var packet = new LdapPacket(Int32.MaxValue);
            Assert.AreEqual(Int32.MaxValue, packet.MessageId);
        }
    }
}