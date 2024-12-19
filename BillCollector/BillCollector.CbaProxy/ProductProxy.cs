using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillCollector.CbaProxy.Dto;
using BillCollector.Infrastructure.HttpClient;

namespace BillCollector.CbaProxy
{
    public interface IProductProxy
    {
        int TestNum();
    }

    public class ProductProxy : IProductProxy
    {
        private readonly IRestSharpClient _restSharpClient;

        public ProductProxy(IRestSharpClient restSharpClient)
        {
            _restSharpClient = restSharpClient;
        }

        public int TestNum()
        {
            return 111;
        }

    }
}
