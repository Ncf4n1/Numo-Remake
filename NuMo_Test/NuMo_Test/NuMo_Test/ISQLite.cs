﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuMo_Test
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
