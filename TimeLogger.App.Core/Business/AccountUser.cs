﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLogger.App.Core.Business
{
    public class AccountUser
    {

        #region Properties

        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public Guid UserId { get; set; }

        public Guid AccountId { get; set; }

        #endregion

    }
}
