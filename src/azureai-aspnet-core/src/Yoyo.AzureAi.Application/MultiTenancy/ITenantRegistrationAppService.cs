using Yoyo.AzureAi.MultiTenancy.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Yoyo.AzureAi.MultiTenancy
{
    public interface ITenantRegistrationAppService
    {
        /// <summary>
        /// 注册租户信息
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        Task<TenantDto> RegisterTenantAsync(CreateTenantDto input);
    }
}
