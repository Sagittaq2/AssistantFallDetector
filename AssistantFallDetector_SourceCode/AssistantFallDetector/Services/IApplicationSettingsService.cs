﻿using AssistantFallDetector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantFallDetector.Services
{
    public interface IApplicationSettingsService
    {
        void LoadApplicationSettings();

        bool AddOrUpdateValue(string Key, Object value);

        valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue);

        void Save();
    }
}
