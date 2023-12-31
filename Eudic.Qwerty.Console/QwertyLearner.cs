﻿namespace Eudic.Qwerty.Console
{
    using Eudic.Qwerty.Infrastructure.Services;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Text.Unicode;

    /// <summary>
    /// TXT文件转换成 https://qwerty.kaiyi.cool/ 网站使用的Json词典
    /// </summary>
    public class QwertyLearner
    {
        private readonly IDictService _dictService;
        private readonly string _textFile;

        /// <summary>
        /// QwertyLearner 构造函数
        /// </summary>
        /// <param name="dictService">SQLite数据库服务</param>
        /// <param name="textFile">TXT文件(使用绝对路径)</param>
        public QwertyLearner(IDictService dictService, string textFile)
        {
            _dictService = dictService ?? throw new ArgumentNullException(nameof(dictService));
            _textFile = textFile;
        }

        /// <summary>
        /// TXT文件转换成JSON词典, 写入到 TXT文件名-qwerty-learner.json
        /// </summary>
        public void ConvertToJsonDict()
        {
            var words = ProcessAllLine(_textFile);
            var list = new List<WordObject>();

            foreach (string word in words)
            {
                if(string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word)) continue;
                
                var dict = _dictService?.GetDict(word);

                list.Add(new WordObject()
                {
                    Word = word,
                    Translation = dict is null || dict.Translation is null
                        ? new List<string>(){"词典内找不到翻译"}
                        : new List<string>(){dict.Translation}
                });

                Console.WriteLine($"Word selected ：{word}");
            }


            WriteToJson(list, _textFile.Replace(".txt", "-qwerty-learner.json"));
        }

        /// <summary>
        /// 读取TXT文件中的所有单词, 一行一个单词
        /// </summary>
        /// <param name="filename">TXT文件(使用绝对路径)</param>
        /// <returns></returns>
        private List<string> ProcessAllLine(string filename)
        {
            var list = new List<string>();

            const Int32 bufferSize = 128;
            using var fileStream = File.OpenRead(filename);
            using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize);
            while (streamReader.ReadLine() is { } line)
            {
                list.Add(line);
            }

            return list;
        }

        /// <summary>
        /// 词典写入到JSON文件
        /// </summary>
        /// <param name="list">词典</param>
        /// <param name="filename">JSON文件(使用绝对路径)</param>
        private void WriteToJson(List<WordObject> list, string filename)
        {
            var encoderSettings = new TextEncoderSettings();
            encoderSettings.AllowRanges(UnicodeRanges.All);
            var options = new JsonSerializerOptions();
            options.Encoder = JavaScriptEncoder.Create(encoderSettings);
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(list, options);
            File.WriteAllText(filename, json);
        }
    }

    /// <summary>
    /// Json词典的单词对象模型; 词典由WordObject数组构成;
    /// </summary>
    public class WordObject
    {
        [JsonPropertyName("name")]
        public string Word { get; set; }

        [JsonPropertyName("trans")]
        public List<string> Translation { get; set; }
    }
}
