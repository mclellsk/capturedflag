using System.IO;

namespace CapturedFlag.Engine
{
    public abstract class BaseStreamParser
    {
        public BaseStreamParser(string path)
        {
            StreamReader r = new StreamReader(path);

            string line;
            while ((line = r.ReadLine()) != null)
            {
                ParseLine(line);
            }

            r.Close();
        }

        public virtual void ParseLine(string line)
        {
            //Parse text...
        }
    }
}

