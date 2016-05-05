using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Data
{
    public interface IAzureTableBase
    {
        [PrimaryKey, AutoIncrement]
        int Id { get; set; }

    }
}
