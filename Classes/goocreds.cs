using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Linka;


namespace Linka
{
    class goocreds : Creds
    {
        public void Save(string id, string pw, string domain)
        {
            // Set values
            this._id = id; this._pw = pw; this._svr = domain;
            // Save file
            this.Save();
        }

        public goocreds()
        {
            _file = "goocreds.txt";
            _db = "google";
            _fullpath = Directory.GetCurrentDirectory() + "\\" + _file;
        }

    }
}
