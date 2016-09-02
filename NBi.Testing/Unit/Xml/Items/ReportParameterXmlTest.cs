﻿using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class ReportParameterXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.ReportParameterXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_NameLoaded()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<ReportParameterXml>());

            var item = (ReportParameterXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Name, Is.EqualTo("myName"));
            Assert.That(item.Caption, Is.Null.Or.Empty);
        }

        [Test]
        public void Deserialize_SampleFile_OthersLoaded()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<ReportParameterXml>());

            var item = (ReportParameterXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Report, Is.EqualTo("myReport"));
            Assert.That(item.Path, Is.EqualTo("myPath"));
        }

        [Test]
        public void Deserialize_SampleFile_CaptionLoaded()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<ReportParameterXml>());

            var item = (ReportParameterXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Name, Is.Null.Or.Empty);
            Assert.That(item.Caption, Is.EqualTo("myCaption"));
        }

        [Test]
        public void Deserialize_SampleFile_PluralLoaded()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<ReportParametersXml>());

            var item = (ReportParametersXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Report, Is.EqualTo("myReport"));
            Assert.That(item.Path, Is.EqualTo("myPath"));
        }


    }
}
