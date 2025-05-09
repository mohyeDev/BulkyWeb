﻿using Builky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builky.DataAccess.Repository.IRepository
{
    public  interface IOrderHeaderRespoitory : IRepository<OrderHeader>
    {

        void Update(OrderHeader orderHeader);

        void UpdateStatus(int id , string orderStatus , string? paymentStatus = null);

        void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);
    }
}
