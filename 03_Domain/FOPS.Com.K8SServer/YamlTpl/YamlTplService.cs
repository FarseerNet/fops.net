using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.K8S.Server;
using FOPS.Com.K8SServer.YamlTpl.Dal;
using FS.Extends;

namespace FOPS.Com.K8SServer.YamlTpl
{
    public class YamlTplService : IYamlTplService
    {
        /// <summary>
        /// Yaml模板列表
        /// </summary>
        public Task<List<YamlTplVO>> ToListAsync() => K8SContext.Data.YamlTpl.ToListAsync().MapAsync<YamlTplVO, YamlTplPO>();

        /// <summary>
        /// Yaml模板信息
        /// </summary>
        public Task<YamlTplVO> ToInfoAsync(int id) => K8SContext.Data.YamlTpl.Where(o => o.Id == id).ToEntityAsync().MapAsync<YamlTplVO, YamlTplPO>();

        /// <summary>
        /// Yaml模板数量
        /// </summary>
        public Task<int> CountAsync() => K8SContext.Data.YamlTpl.CountAsync();

        /// <summary>
        /// 添加Yaml模板
        /// </summary>
        public async Task<int> AddAsync(YamlTplVO vo)
        {
            var po = vo.Map<YamlTplPO>();
            await K8SContext.Data.YamlTpl.InsertAsync(po, true);
            vo.Id = po.Id.GetValueOrDefault();
            return vo.Id;
        }

        /// <summary>
        /// 修改Yaml模板
        /// </summary>
        public Task UpdateAsync(int id, YamlTplVO vo) => K8SContext.Data.YamlTpl.Where(o => o.Id == id).UpdateAsync(vo.Map<YamlTplPO>());

        /// <summary>
        /// 删除Yaml模板
        /// </summary>
        public Task DeleteAsync(int id) => K8SContext.Data.YamlTpl.Where(o => o.Id == id).DeleteAsync();
    }
}