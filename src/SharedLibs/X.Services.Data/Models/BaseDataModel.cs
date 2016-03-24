using GalaSoft.MvvmLight;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public class BaseDataModel 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Uid { get; set; }

        
    }
}
