namespace Eudic.Qwerty.Console
{
    using Eudic.Qwerty.Infrastructure.Services;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Unicode;

    /// <summary>
    /// 筛选出COCA单词表中的动词、形容词
    /// </summary>
    public class CocaFilter
    {
        private readonly IDictService _dictService;
        private readonly string _textFile;

        /// <summary>
        /// CocaFilter 构造函数
        /// </summary>
        /// <param name="dictService">SQLite数据库服务</param>
        /// <param name="textFile">TXT文件(使用绝对路径)</param>
        public CocaFilter(IDictService dictService, string textFile)
        {
            _dictService = dictService;
            _textFile = textFile;
        }

        /// <summary>
        /// 筛选出形容词, 写入到网站使用的Json文件
        /// </summary>
        public void FilterVerbToJsonDict()
        {
            var words = ProcessAllLine(_textFile);
            var list = new List<WordObject>();

            foreach (string word in words)
            {
                if(string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word) || word.Length < 3) continue;
                
                var dict = _dictService?.GetDict(word);

                
                if (dict != null && dict.Translation != null 
                                 && (dict.Translation.Contains("vi.") || dict.Translation.Contains("vt.")))
                {
                    list.Add(new WordObject()
                    {
                        Word = word,
                        Translation = new List<string>(){dict.Translation}
                    });
                }

            }

            WriteToJson(list, _textFile.Replace(".txt", "-verb.json"));
        }


        /// <summary>
        /// 筛选出动词,写入到TXT文件-verb
        /// </summary>
        public void FilterVerbToText()
        {
            List<string> allVerb = new List<string>();
            var words = ProcessAllLine(_textFile);
            
            foreach (string word in words)
            {
                if(string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word) || word.Length < 3) continue;
                
                var dict = _dictService?.GetDict(word);

                if (dict != null && dict.Translation != null && (dict.Translation.Contains("vi.") || dict.Translation.Contains("vt.")))
                {
                    allVerb.Add(word);

                    Console.WriteLine($"Word selected ：{word}");
                }
            }


            WriteToText(allVerb, _textFile.Replace(".txt", "-verb.txt"));
        }

        /// <summary>
        /// 筛选出形容词, 写入到网站使用的Json文件
        /// </summary>
        public void FilterAdjectiveToJsonDict()
        {
            var words = ProcessAllLine(_textFile);
            var list = new List<WordObject>();

            foreach (string word in words)
            {
                if(string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word) || word.Length < 3) continue;
                
                var dict = _dictService?.GetDict(word);

                if(dict is null || dict.Translation is null || !dict.Translation.Contains("adj.")) continue;

                list.Add(new WordObject()
                {
                    Word = word,
                    Translation = new List<string>(){dict.Translation}
                });
            }


            WriteToJson(list, _textFile.Replace(".txt", "-adj.json"));
        }

        /// <summary>
        /// 筛选出形容词,写入到TXT文件-adj
        /// </summary>
        public void FilterAdjectiveToText()
        {
            List<string> allVerb = new List<string>();
            var words = ProcessAllLine(_textFile);
            
            foreach (string word in words)
            {
                if(string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word) || word.Length < 3) continue;
                
                var dict = _dictService?.GetDict(word);

                if (dict is { Translation: { } } && (dict.Translation.Contains("adj.")))
                {
                    allVerb.Add(word);

                    Console.WriteLine($"Word selected ：{word}");
                }
            }


            WriteToText(allVerb, _textFile.Replace(".txt", "-adj.txt"));
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

        /// <summary>
        /// 筛选出的单词写入到TXT文件
        /// </summary>
        /// <param name="words"></param>
        /// <param name="filename"></param>
        private void WriteToText(List<string> words, string filename)
        {
            var contents = string.Empty;

            foreach (var word in words)
            {
                contents += word + "\n";
            }

            File.WriteAllText(filename, contents);
        }

    }
}
