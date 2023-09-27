﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AutoRest.CSharp.Common.Utilities;

namespace AutoRest.CSharp.Mgmt.Report
{
    internal class TransformStore
    {
        public static TransformStore Instance { get; set; } = new TransformStore();

        private Dictionary<TransformItem, List<TransformLog>> _transformItemDict = new Dictionary<TransformItem, List<TransformLog>>();
        private int _logIndex = 0;

        public TransformStore()
        {
        }

        public void Reset()
        {
            this._transformItemDict = new Dictionary<TransformItem, List<TransformLog>>();
            this._logIndex = 0;
        }

        public void AddTransformer(TransformItem item)
        {
            if (this._transformItemDict.ContainsKey(item))
            {
                AutoRestLogger.Warning($"Duplicate transform detected: {item}").Wait();
                return;
            }
            this._transformItemDict.Add(item, new List<TransformLog>());
        }

        public void AddTransformer(string type, string key, bool fromConfig, params string[] arguments)
        {
            var item = new TransformItem(type, key, fromConfig, arguments);
            this.AddTransformer(item);
        }

        public void AddTransformers(IEnumerable<TransformItem> items)
        {
            foreach (var item in items)
                this.AddTransformer(item);
        }

        private int GetNextLogIndex()
        {
            return this._logIndex++;
        }

        public void AddTransformLog(TransformItem item, string targetFullSerializedName, string logMessage)
        {
            if (!_transformItemDict.ContainsKey(item))
                this.AddTransformer(item);
            _transformItemDict[item].Add(new TransformLog(GetNextLogIndex(), targetFullSerializedName, logMessage));
        }

        public void AddTransformLogForApplyChange(TransformItem item, string targetFullSerializedName, string changeName, string? from, string? to)
        {
            this.AddTransformLog(item, targetFullSerializedName, CreateChangeMessage(changeName, from, to));
        }

        public void AddTransformLogForApplyChange(string transformType, string key, string argument, string targetFullSerializedName, string changeName, string? from, string? to)
        {
            this.AddTransformLog(new TransformItem(transformType, key, argument), targetFullSerializedName, CreateChangeMessage(changeName, from, to));
        }

        private string CreateChangeMessage(string changeName, string? from, string? to) => $"{changeName} '{from ?? "<null>"}' --> '{to ?? "<null>"}'";

        public void ForEachTransform(Action<TransformItem, ReadOnlyCollection<TransformLog>> action)
        {
            foreach (var (transform, logs) in this._transformItemDict)
                action(transform, logs.AsReadOnly());
        }

        public string ToReport()
        {
            StringBuilder sb = new StringBuilder();

            if (this._transformItemDict.Count > 0)
            {
                foreach (var group in _transformItemDict.GroupBy(item => item.Key.TransformType).OrderBy(g => g.Key))
                {
                    sb.AppendLine(group.Key);
                    foreach (var (item, logs) in group.OrderBy(kv => kv.Value.Count == 0 ? 0 : 1).ThenBy(kv => kv.Key.Key))
                    {
                        sb.AppendLine($"  - {item.Key}{(string.IsNullOrEmpty(item.ArgumentsAsString) ? "" : ": " + item.ArgumentsAsString)}{(item.IsFromConfig ? "" : "!")}{(logs.Count == 0 ? " ## <NoUsage>" : "")}");
                        foreach (var log in logs)
                        {
                            sb.AppendLine($"    - [{log.Index}][{(item.Key == log.TargetFullSerializedName ? "=" : log.TargetFullSerializedName)}]: {log.LogMessage}");
                        }
                    }
                }
            }
            else
            {
                sb.AppendLine("No Transform detedted");
            }
            return sb.ToString();
        }
    }

}
