using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linka;

namespace Linka
{
    public static class Terms
    {
        private static string _file = "term.txt";

        public static bool IsFile
        {
            get
            {
                string fp = Directory.GetCurrentDirectory() + "\\" + _file;
                return File.Exists(fp);
            }
        }

        public static string Read()
        {
            string fp = Directory.GetCurrentDirectory() + "\\" + _file;
            if (File.Exists(fp) == false) { throw new Exception("No term file"); }
            FileStream fs = new FileStream(fp, FileMode.Open);
            try
            {
                int sum = 0;
                int count;
                int length = (int)fs.Length;
                byte[] buffer = new byte[length];
                while ((count = fs.Read(buffer, sum, length - sum)) > 0) { sum += count; } // Does this have to be a loop?
                string t = ASCIIEncoding.ASCII.GetString(buffer);
                return t;
            }
            catch (Exception e) { throw new Exception("Loading error", e); }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
        }

        public static void Save(string term)
        {
            if (term == "") { throw new Exception("No term provided."); }
            string fp = Directory.GetCurrentDirectory() + "\\" + _file;
            FileStream fs = new FileStream(fp, FileMode.Create);
            try
            {
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(term);
                fs.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e) { throw new Exception("Saving error", e); }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
        }
    }
}
