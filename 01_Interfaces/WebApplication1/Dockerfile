# 注意，这里的构建上下文，是在git源代码的根目录
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

${dotnet_restore}

COPY . .

WORKDIR /src/${git_name}/${project_path}
RUN dotnet publish -c Release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY --from=build /app .

#设置时区
RUN cp /usr/share/zoneinfo/GMT /etc/localtime
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai    /etc/localtime

EXPOSE ${entry_port}
EXPOSE 443

ENTRYPOINT ["dotnet", "${entry_point}"]