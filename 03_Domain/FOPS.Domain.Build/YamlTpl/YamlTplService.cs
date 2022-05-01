using FOPS.Domain.Build.YamlTpl.Repository;

namespace FOPS.Domain.Build.YamlTpl;

public class YamlTplService: ISingletonDependency
{
    public IYamlTplRepository YamlTplRepository { get; set; }
}