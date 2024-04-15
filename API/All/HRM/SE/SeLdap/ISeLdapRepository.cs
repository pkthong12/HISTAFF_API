using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SeLdap
{
    public interface ISeLdapRepository: IGenericRepository<SE_LDAP, SeLdapDTO>
    {
       Task<GenericPhaseTwoListResponse<SeLdapDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeLdapDTO> request);
    }
}

