using System;
using LibSelection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RgxCTests
{
    [TestClass]
    public class BasicSelectionTest
    {
        [TestMethod]
        public void Test1()
        {
            Selection s = new Selection("hello this is a test");//hello this is a test
            Assert.AreEqual("hello this is a test", s.Value);
            Selection w_test = s.Sel(16, 4);
            Selection w_this = s.Sel(6, 4);
            Selection w_a = s.Sel(14, 1);
            Selection end = s.Sel(s.Value.Length, 0);
            w_this.Replace("rahaha");//hello rahaha is a test
            Assert.AreEqual("hello rahaha is a test", s.Value);
            Selection ha = w_this.Sel(2, 2);
            ha.Replace("hee");//hello raheeha is a test
            Assert.AreEqual("hello raheeha is a test", s.Value);
            w_a.Replace("the");//hello raheeha is the test
            Assert.AreEqual("hello raheeha is the test", s.Value);
            w_test.Replace("cookie");//hello raheeha is the cookie
            Assert.AreEqual("hello raheeha is the cookie", s.Value);
            end.Replace(" + meat");//hello raheeha is the cookie + meat
            Assert.AreEqual("hello raheeha is the cookie + meat", s.Value);
        }
    }
}
