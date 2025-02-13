﻿using EPiServer.Core;
using EPiServer.Logging;
using Newtonsoft.Json;
using System;

namespace Imageshop.Optimizely.Plugin
{
    public abstract class PropertyJsonSerializedObject<T> : PropertyLongString where T : class
    {
        protected T _value;
        private readonly EPiServer.Logging.ILogger _log = LogManager.GetLogger(typeof(PropertyJsonSerializedObject<T>));

        public override Type PropertyValueType => typeof(T);

        public override object Value
        {
            get
            {
                try
                {
                    var value = LongString;

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return null!;
                    }

                    _value = System.Text.Json.JsonSerializer.Deserialize<T>(value, new System.Text.Json.JsonSerializerOptions() {  AllowTrailingCommas= true});

                    return _value!;
                }
                catch (Exception ex)
                {
                    _log.Error("There was exception whilst deserialising object", ex);

                    return null!;
                }
            }
            set
            {
                if (value is T)
                {
                    _value = null;
                    base.Value = JsonConvert.SerializeObject(value);
                    return;
                }

                base.Value = value;
            }
        }

        public override object SaveData(PropertyDataCollection properties)
        {
            return LongString;
        }
    }
}