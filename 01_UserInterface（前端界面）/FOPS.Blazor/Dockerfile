# 添加基础镜像
FROM farseernet/dotnet:sdk5.0_kubectl_npm6.14.13_tools.2
WORKDIR /app
COPY . ./

#设置时区
RUN cp /usr/share/zoneinfo/GMT /etc/localtime
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai    /etc/localtime

#容器中使用 ["dotnet","系统启动的dll"] 来运行应用程序
ENTRYPOINT ["dotnet", "FOPS.Blazor.dll"]