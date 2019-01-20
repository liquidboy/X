﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popcorn.Models.Shows
{
    public interface IShow
    {
        string ImdbId { get; set; }

        bool IsFavorite { get; set; }
    }
}
