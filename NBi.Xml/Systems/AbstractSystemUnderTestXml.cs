﻿using System;
using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml.Systems
{
    public abstract class AbstractSystemUnderTestXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public DefaultXml Default { get; set; }
        public SettingsXml Settings { get; set; }

        public AbstractSystemUnderTestXml()
        {
            Default = new DefaultXml();
        }
    }
}
