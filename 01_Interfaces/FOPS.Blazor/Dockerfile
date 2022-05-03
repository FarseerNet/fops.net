# 添加基础镜像
FROM farseernet/dotnet:net6_kubectl_tool.1
WORKDIR /app
COPY . ./

#设置时区
RUN cp /usr/share/zoneinfo/GMT /etc/localtime
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai    /etc/localtime

#容器中使用 ["dotnet","系统启动的dll"] 来运行应用程序
ENTRYPOINT ["dotnet", "FOPS.Blazor.dll"]