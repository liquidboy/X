using X.CoreLib.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public interface ISqliteBase
    {
        [PrimaryKey, AutoIncrement]
        int Id { get; set; }
        string Uid { get; set; }
        string Index1 { get; set; }

    }
}
