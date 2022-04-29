docker kill fops
docker rm fops
docker rmi fops:latest

# 编译应用
dotnet publish -c release

# 打包
cd bin/release/net6.0/publish
docker build -t fops:latest --network=host .
# 发到内网
docker login dockerhub.abtest.ws/test -u admin -p admin
docker tag fops:latest dockerhub.abtest.ws/test:fops-dev
docker push dockerhub.abtest.ws/test:fops-dev
docker rmi dockerhub.abtest.ws/test:fops-dev

# 运行
docker run -d --name fops -p 88:88 \
--network net --network-alias fops fops:latest \
--restart=always