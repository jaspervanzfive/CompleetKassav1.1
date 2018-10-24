﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleetKassa.Models;
using CompleetKassa.ViewModels.Commands;

namespace CompleetKassa.ViewModels
{
    public class LockViewModel : BaseViewModel
    {
        public LockViewModel() : base("Lock", "#1F2E3C", "Icons/lock.png")
        {

        }
    }
}
