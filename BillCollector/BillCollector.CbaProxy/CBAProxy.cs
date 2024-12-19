using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.CbaProxy
{
    public class CBAProxy
    {
        private readonly IProductProxy _productProxy;

        public CBAProxy(IProductProxy productProxy)
        {
            _productProxy = productProxy;
        }

        public IProductProxy Product => _productProxy;
    }
}
