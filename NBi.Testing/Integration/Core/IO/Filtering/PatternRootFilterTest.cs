﻿using NBi.Core.IO.File;
using NBi.Core.IO.Filtering;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.Core.IO.Filtering
{
    public class PatternRootFilterTest
    {
        private string DirectoryName { get => $@"Temp\{GetType().Name}\"; }

        [SetUp]
        public void Setup()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, true);
            Directory.CreateDirectory(DirectoryName);

        }

        [TearDown]
        public void Cleanup()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, true);
        }

        [Test]
        [TestCase("*.*", 5)]
        [TestCase("*.txt", 4)]
        [TestCase("foo-*.txt", 3)]
        [TestCase("foo-?.txt", 2)]
        [TestCase("foo-0.txt", 1)]
        public void GetFiles_Pattern_CorrectCount(string pattern, int count)
        {
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");

            var fileLister = new FileLister(DirectoryName);
            var filters = new List<IFileFilter>() { new PatternRootFilter(pattern) };

            var dir = new DirectoryInfo(DirectoryName);
            Assert.That(fileLister.Execute(filters).Count(), Is.EqualTo(count));
        }
    }
}
