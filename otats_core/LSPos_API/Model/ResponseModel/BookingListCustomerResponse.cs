using System;
using System.Collections.Generic;

namespace Model.ResponseModel
{

    public class BookingListCustomerResponse
    {
        public List<BookingCustomerResponse> customers { get; set; }
    }
}