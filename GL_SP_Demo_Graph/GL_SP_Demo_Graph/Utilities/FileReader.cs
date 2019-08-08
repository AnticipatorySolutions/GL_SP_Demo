using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GL_SP_Demo_Graph
{
    public interface IFileReader
    {
        IReturnValue Read(ReadDelegate reader, string fileName);
    }

    public delegate IReturnValue ReadDelegate(string fileName);

    public class Reader : IFileReader
    {
        public IReturnValue Read(ReadDelegate reader, string fileName)
        {
            return reader(fileName);
        }
    }

    public interface IReturnValue
    {
        Dictionary<string, List<string>> FileValues { get; set; }
        string[] Headers { get; set; }
        string FileLocation { get; set; }
    }

    public class CSV_Return_Value : IReturnValue
    {
        public Dictionary<string, List<string>> FileValues { get; set; }
        public string[] Headers { get; set; }
        public string FileLocation { get; set; }
    }


    public class ReaderStore
    {
        IReturnValue ReturnValue;
        string BaseHeader { get; set; }
        private string _FileName;

        private delegate void readerDel();

        public IReturnValue CSV_Reader_Headless(string fileName)
        {
            return ReaderShellEmptyHead(fileName);
        }

        public IReturnValue CSV_Reader_FullHead(string fileName)
        {
            return ReaderShellFullHead(fileName);
        }

        private void Init(string fileName)
        {
            _FileName = fileName;
            var t = Directory.GetCurrentDirectory();
            if (!File.Exists(fileName))
            {
                throw new System.Exception("FILE_NAME_FROM_GRAPH_READER_STORE_IS_INVALID");
            }
            ReturnValue = new CSV_Return_Value
            {
                FileValues = new Dictionary<string, List<string>>(),
                FileLocation = fileName,
                Headers = new string[0]
            };

        }

        private IReturnValue ReaderShellFullHead(string fileName)
        {
            Init(fileName);
            return Read_FullHead_Control();           
        }

        private IReturnValue ReaderShellEmptyHead(string fileName)
        {
            Init(fileName);
            return Read_EmptyHead_Control();
        }


        private void HeaderAction()
        {
            foreach (var entry in ReturnValue.Headers)
            {
                ReturnValue.FileValues.Add(entry, new List<string>());
            }
        }

        private void HeadlessAction()
        {
            string header;
            if (BaseHeader == null) { header = "H"; }
            else { header = BaseHeader; }

            int index = 0;

            foreach (var entry in ReturnValue.Headers)
            {
                index++;
                string tempHeader = $"{header}{index}";
                ReturnValue.FileValues.Add(tempHeader, new List<string>());
                ReturnValue.FileValues[tempHeader].Add(entry);
            }
        }

        private IReturnValue Read_EmptyHead_Control()
        {
            using (StreamReader reader = new StreamReader(_FileName))
            {
                ReturnValue.FileValues = new Dictionary<string, List<string>>();

                var readValues = reader
                    .ReadLine()
                    .Split(',')
                    .ToArray();
                int index = 0;
                foreach (string val in readValues)
                {
                    index++;
                    ReturnValue.FileValues.Add($"H{index}", new List<string>());
                    ReturnValue.FileValues[$"H{index}"].Add(val);
                }


                while (!reader.EndOfStream)
                {
                    var readValues2 = reader
                    .ReadLine()
                    .Split(',')
                    .ToArray();
                    index = 0;
                    foreach (string val in readValues2)
                    {
                        index++;
                        ReturnValue.FileValues[$"H{index}"].Add(val);
                    }

                }
            }
            return ReturnValue;
        }



        private IReturnValue Read_FullHead_Control()
        {
  
            using (StreamReader reader = new StreamReader(_FileName))
            {
                var tempVal = new Dictionary<string, List<string>>();

                var readValues = reader
                        .ReadLine()
                        .Split(',')
                        .ToArray();

                ReturnValue.Headers = readValues;

                foreach (var entry in readValues)
                {
                    tempVal.Add(entry, new List<string>());
                }

                while (!reader.EndOfStream)
                {
                    var line = reader
                        .ReadLine();

                    //HACK: Workaround double quotes wrapping extra commas
                    if (line.Contains("\""))
                    {
                        int p1 = line.IndexOf("\"");
                        int p2 = line.LastIndexOf("\"");
                        int p3 = line.IndexOf(',', p1);
                        if (p3 < p2)
                        {
                            line = line.Remove(p3, 1);
                        };
                    };

                    var line2 = line
                        .Split(',')
                        .ToArray();

                    for (int i = 0; i < readValues.Length; i++)
                    {
                        tempVal[readValues[i]].Add(line2[i]);
                    }
                }
                ReturnValue.FileValues = tempVal;
            }

            return ReturnValue;
        }
    }
}
