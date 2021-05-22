# 添加基础镜像
FROM farseernet/dotnet:sdk_kubectl-5.0
WORKDIR /app
COPY . ./

#容器中使用 ["dotnet","系统启动的dll"] 来运行应用程序
ENTRYPOINT ["dotnet", "FOPS.Blazor.dll"]