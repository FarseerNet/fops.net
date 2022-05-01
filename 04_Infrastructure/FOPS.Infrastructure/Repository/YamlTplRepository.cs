using FOPS.Domain.Build.YamlTpl.Repository;
using FOPS.Infrastructure.Repository.YamlTpl;

namespace FOPS.Infrastructure.Repository;

public class YamlTplRepository : IYamlTplRepository
{
    public YamlTplAgent YamlTplAgent { get; set; }
}