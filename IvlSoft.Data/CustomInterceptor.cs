using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using INTUSOFT.Data.NewDbModel;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Type;

namespace INTUSOFT.Data
{
    //CustomInterceptor class has been added to resolve the issue when arised to save proxy data.
    public class CustomInterceptor : EmptyInterceptor
    {
        public override string GetEntityName(object entity)
        {
            string path="INTUSOFT.Data.NewDbModel.";
            string name = null;
            string cName = entity.GetType().ToString();
            if (cName.Contains("Proxy"))
            {
                name = cName.Replace("Proxy", "");
                name = path + name;
            }
            else
            {
                name = base.GetEntityName(entity);
            }
            return name;
        }
    }
}
