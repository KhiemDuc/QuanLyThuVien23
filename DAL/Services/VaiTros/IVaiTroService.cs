using DAL.Model;
using DAL.Services.VaiTros.VaiTroDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.VaiTros
{
    public interface IVaiTroService
    {
         IQueryable<VaiTro> QueryFilter(VaiTroFilterInput input = null);
    }
}
